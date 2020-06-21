using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Controller.NBattleSimulation;
using Controller.NDebug;
using Controller.NTile;
using Controller.NUnit;
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
using Shared.Addons.OkwyLogging;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using View;
using View.NTile;
using View.NUnit;
using View.Presenters;
using View.UIs;
using View.Views;
using Logger = Shared.Addons.OkwyLogging.Logger;

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
      //TODO: implement event bus that won't allocate(no delegates)
      var eventBus = new EventBus(); //TODO: stop using eventbus Ievent interface to remove reference on that library from model
      EventBus.Log = m => log.Info($"{m}");
      var eventHolder = new EventHolder(eventBus);

      var mainCamera = Camera.main;
      var raycastController = new RaycastController(mainCamera, 
        LayerMask.GetMask("Terrain", "GlobalCollider"), LayerMask.GetMask("Unit"));
      
      #endregion
      #region View
      
      var tileSpawner = new TilePresenter(TileStartPoints, new TileViewFactory(TileViewPrefab));
      var coordFinder = new CoordFinder(TileStartPoints);
      var unitViewFactory = new UnitViewFactory(units, UnitViewPrefab, coordFinder, mainCamera);
      var unitViewCoordChangedHandler = new UnitViewCoordChangedHandler(coordFinder);
      var boardPresenter = new BoardPresenter(unitViewCoordChangedHandler);
      
      var playerPresenterContext = new PlayerPresenterContext(
        new PlayerPresenter(unitViewFactory, unitViewCoordChangedHandler), 
        new PlayerPresenter(unitViewFactory, unitViewCoordChangedHandler));
      
      #endregion
      #region Model
      //TODO: replace board/bench dictionaries with array?                
      var unitFactory = new UnitFactory(units, new DecisionFactory(eventHolder));
      var playerContext = new PlayerContext(new Player(unitFactory), new Player(unitFactory));
      var board = new Board();
      var aiHeap = new AiHeap();
      var aiContext = new AiContext(board, aiHeap);

      #endregion
      #region Context

      var worldContext = new WorldContext(playerContext, playerPresenterContext, BattleSetupUI);

      #endregion
      
      var unitSelectionController = new UnitSelectionController(
        inputController, raycastController, coordFinder);
      var unitTooltipController = new UnitTooltipController(
        UnitTooltipUI, unitSelectionController);
      
      #region Unit drag

      var battleStateController = new BattleStateController(BattleSimulationUI);
      var unitDragController = new UnitDragController(raycastController, 
        new CoordFinderBySelectedPlayer(coordFinder, BattleSetupUI), 
        inputController, unitSelectionController, 
        new CanStartDrag(battleStateController, BattleSetupUI));
      
      var tileHighlightController = new TileHighlighterController(tileSpawner, 
        unitDragController);

      var unitMoveController = new UnitMoveController(worldContext, unitDragController);

      #endregion
      #region Battle simulation

      var movementController = new MovementController(boardPresenter, coordFinder);
      var attackController = new AttackController(boardPresenter, unitTooltipController);
      eventBus.Register<StartMoveEvent>(movementController); //TODO: register implicitly?
      eventBus.Register<FinishMoveEvent>(movementController);
      eventBus.Register<RotateEvent>(movementController);
      eventBus.Register<ApplyDamageEvent>(attackController);
      eventBus.Register<DeathEvent>(attackController);
      eventBus.Register<IdleEvent>(movementController);
      eventBus.Register<StartAttackEvent>(attackController);

      var battleSimulationPresenter = new BattleSimulationPresenter(coordFinder, 
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

      var unitModelDebugController = new UnitModelDebugController(playerContext, board, ModelUI, 
        DebugController.Info, unitSelectionController, battleStateController);
      
      var takenCoordDebugController = new TakenCoordDebugController(board, DebugController,
        tileSpawner);
      
      var targetDebugController = new TargetDebugController(
        board, coordFinder, DebugController.Info);
      
      var uiDebugController = new UIDebugController(
        BattleSetupUI, BattleSaveUI, BattleSimulationUI,
        unitModelDebugController);

      #endregion

      yield return null;
      //TODO: instead of init say something meaningful
      #region View

      BattleSetupUI.Init(units.Keys.ToList());
      BattleSaveUI.Init(saves.Keys.ToList());
      tileSpawner.SpawnTiles();

      #endregion
      #region Infrastructure

      eventHolder.Init(aiContext); //TODO: remove
      tickController.Init(takenCoordDebugController, targetDebugController, uiDebugController,  
        unitModelDebugController, realtimeBattleSimulationController, DebugController); //TODO: register implicitly?
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
      //TODO: add IDisposable controllers to CompositeDisposable and reverse dispose them on unity OnDestroy callback 
    }

    static readonly Logger log = MainLog.GetLogger(nameof(CompositionRoot));
  }
}
