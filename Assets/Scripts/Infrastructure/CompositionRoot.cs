using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Controller.NBattleSimulation;
using Controller.NDebug;
using Controller.NTile;
using Controller.Save;
using Controller.UnitDrag;
using Controller.Update;
using Model.NBattleSimulation;
using Model.NUnit;
using Shared;
using UnityEngine;
using Model;
using PlasticFloor.EventBus;
using Shared.Abstraction;
using Shared.OkwyLogging;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using View;
using View.Factories;
using View.NUnit;
using View.Presenters;
using View.UIs;
using View.Views;
using Logger = Shared.OkwyLogging.Logger;

namespace Infrastructure {
  public class CompositionRoot : MonoBehaviour {
    #region Monobehaviours
    #region Controller

    public DebugController DebugController;
    public MonoBehaviourCallBackController MonoBehaviourCallBackController;

    #endregion
    #region UI

    public BattleSetupUI BattleSetupUI;
    public BattleSaveUI BattleSaveUI;
    public BattleSimulationUI BattleSimulationUI;
    public UnitTooltipUI UnitTooltipUI;
    public ModelUI ModelUI;

    #endregion
    #region View

    public TileStartPoints TileStartPoints;
    public UnitView UnitViewPrefab;
    public TileView TileViewPrefab;

    #endregion
    #endregion

    IEnumerator Start() {
      #region Config
      
      log.Info("\n\nStart");
      var units = new UnitInfoLoader().Load();
      var saveDataLoader = new SaveInfoLoader();
      var saves = saveDataLoader.Load();
      
      #endregion
      #region Infrastructure
      
      var tickController = new TickController();
      var inputController = new InputController(tickController);
      var eventBus = new EventBus(); //TODO: stop using eventbus Ievent interface to remove reference on that library
      EventBus.Log = m => log.Info($"{m}");
      var eventHolder = new EventHolder(eventBus);

      var mainCamera = Camera.main;
      var raycastController = new RaycastController(mainCamera, 
        LayerMask.GetMask("Terrain", "GlobalCollider"), LayerMask.GetMask("Unit"));
      
      #endregion
      #region View
      
      var tileSpawner = new TileSpawner(TileStartPoints, new TileViewFactory(TileViewPrefab));
      var tilePresenter = new TilePresenter(TileStartPoints);
      var unitViewFactory = new UnitViewFactory(units, UnitViewPrefab, tilePresenter, mainCamera);
      var boardPresenter = new BoardPresenter(tilePresenter);
      
      var playerPresenterContext = new PlayerPresenterContext(
        new PlayerPresenter(tilePresenter, unitViewFactory), 
        new PlayerPresenter(tilePresenter, unitViewFactory));
      
      #endregion
      #region Model
                      
      var unitFactory = new UnitFactory(units, new DecisionFactory(eventHolder));
      var playerContext = new PlayerContext(new Player(unitFactory), new Player(unitFactory));
      var board = new Board();
      var aiHeap = new AiHeap();
      var aiContext = new AiContext(board, aiHeap);

      #endregion
      #region Context

      var worldContext = new WorldContext(playerContext, playerPresenterContext, BattleSetupUI);

      #endregion
      
      var coordFinder = new CoordFinder(tilePresenter, BattleSetupUI);
      var unitSelectionController = new UnitSelectionController(
        inputController, raycastController, coordFinder);
      var unitTooltipController = new UnitTooltipController(
        UnitTooltipUI, unitSelectionController);
      
      #region Unit drag
      
      var unitDragController = new UnitDragController(raycastController, 
        coordFinder, inputController, unitSelectionController, 
        new CanStartDrag(new BattleStateController(BattleSimulationUI), BattleSetupUI));
      
      var tileHighlightController = new TileHighlighterController(tileSpawner, 
        unitDragController);

      var unitMoveController = new UnitMoveController(worldContext, unitDragController);

      #endregion
      #region Battle simulation

      var movementController = new MovementController(boardPresenter, tilePresenter);
      var attackController = new AttackController(boardPresenter, unitTooltipController);
      eventBus.Register<StartMoveEvent>(movementController);
      eventBus.Register<FinishMoveEvent>(movementController);
      eventBus.Register<RotateEvent>(movementController);
      eventBus.Register<ApplyDamageEvent>(attackController);
      eventBus.Register<DeathEvent>(attackController);
      eventBus.Register<IdleEvent>(movementController);
      eventBus.Register<StartAttackEvent>(attackController);

      var battleSimulationPresenter = new BattleSimulationPresenter(tilePresenter, 
        boardPresenter, movementController);
      
      var battleSimulation = new BattleSimulation(aiContext, board, aiHeap);
      var realtimeBattleSimulationController = new RealtimeBattleSimulationController(
        movementController, battleSimulation, eventHolder);

      #endregion
      #region Debug
      
      var battleSetupController = new BattleSetupController(playerContext, 
        playerPresenterContext, BattleSetupUI);
      
      var battleSaveController = new BattleSaveController(playerContext, 
        playerPresenterContext, BattleSaveUI, saveDataLoader, saves);
      
      var battleSimulationController = new BattleSimulationDebugController(
        battleSimulation, BattleSimulationUI, 
        aiContext, playerContext, playerPresenterContext, realtimeBattleSimulationController,
        battleSimulationPresenter);

      var unitModelDebugController = new UnitModelDebugController(
        board, ModelUI, DebugController.Info, unitSelectionController);
      
      var takenCoordDebugController = new TakenCoordDebugController(board, DebugController,
        tileSpawner);
      
      var targetDebugController = new TargetDebugController(
        board, tilePresenter, DebugController.Info);
      
      var uiDebugController = new UIDebugController(
        BattleSetupUI, BattleSaveUI, BattleSimulationUI,
        unitModelDebugController);

      #endregion

      yield return null;

      #region View

      BattleSetupUI.Init(units.Keys.ToList());
      BattleSaveUI.Init(saves.Keys.ToList());
      tileSpawner.Init();

      #endregion
      #region Infrastructure

      eventHolder.Init(aiContext); //TODO: move parameters to constructor
      tickController.Init(takenCoordDebugController, targetDebugController, uiDebugController, 
        unitModelDebugController, realtimeBattleSimulationController, DebugController);
      inputController.Init();

      #endregion
      #region Controller

      unitSelectionController.Init();
      unitDragController.Init();
      tileHighlightController.Init();
      unitMoveController.Init();
      unitTooltipController.Init();

      #endregion
      #region Debug

      unitModelDebugController.Init();
      DebugController.Init(UnitTooltipUI);

      #endregion

      MonoBehaviourCallBackController.Init(tickController);
    }

    static readonly Logger log = MainLog.GetLogger(nameof(CompositionRoot));
  }
}
