using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Controller.DecisionTree.Data;
using Controller.DecisionTree.Visitor;
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
using Model.NAI.NDecisionTree;
using PlasticFloor.EventBus;
using Shared.Abstraction;
using Shared.Addons.OkwyLogging;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using UniRx;
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
      var decisionTreeLoader = new DecisionTreeLoader();
      var component = decisionTreeLoader.Load();
      
      #endregion
      #region Infrastructure
      
      var tickController = new TickController();
      var inputController = new InputController(tickController);
      //TODO: implement event bus that won't allocate(no delegates)
      var eventBus = new EventBus(); //TODO: stop using eventbus Ievent interface to remove reference on that library from model
      EventBus.Log = m => log.Info($"{m}"); //TODO: remove that lmao
      EventBus.IsLogOn = () => DebugController.Info.IsDebugOn;
      
      #endregion
      #region View
      
      var mainCamera = Camera.main;
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
      var decisionTreeCreatorVisitor = new DecisionTreeCreatorVisitor();
      var unitFactory = new UnitFactory(units, 
        new DecisionFactory(eventBus, d => new LoggingDecorator(d, DebugController.Info),
          decisionTreeCreatorVisitor, component));
      var playerContext = new PlayerContext(new Player(unitFactory), new Player(unitFactory));
      var board = new Board();
      var aiHeap = new AiHeap();
      var aiContext = new AiContext(board, aiHeap);

      #endregion
      #region Shared

      var worldContext = new PlayerSharedContext(playerContext, playerPresenterContext, BattleSetupUI);

      #endregion
      #region Controller

      var raycastController = new RaycastController(mainCamera, 
        LayerMask.GetMask("Terrain", "GlobalCollider"), LayerMask.GetMask("Unit"));
      
      var unitSelectionController = new UnitSelectionController(inputController, 
        raycastController, coordFinder);
      
      var unitTooltipController = new UnitTooltipController(UnitTooltipUI, 
        unitSelectionController);

      #endregion
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
      var animationController = new AnimationController(boardPresenter);
      eventBus.Register<StartMoveEvent>(movementController, animationController); //TODO: register implicitly?
      eventBus.Register<FinishMoveEvent>(movementController);
      eventBus.Register<RotateEvent>(movementController);
      eventBus.Register<UpdateHealthEvent>(attackController);
      eventBus.Register<DeathEvent>(attackController);
      eventBus.Register<IdleEvent>(animationController);
      eventBus.Register<StartAttackEvent>(animationController);

      var battleSimulationPresenter = new BattleSimulationPresenter(coordFinder, 
        boardPresenter, movementController, movementController);
      
      var battleSimulation = new BattleSimulation(aiContext, board, aiHeap);
      
      var realtimeBattleSimulationController = new RealtimeBattleSimulationController(
        movementController, battleSimulation);

      #endregion
      #region Debug
      
      var battleSimulationDebugController = new BattleSimulationDebugController(
        battleSimulation, BattleSimulationUI, 
        aiContext, playerContext, playerPresenterContext, realtimeBattleSimulationController,
        battleSimulationPresenter);
      
      var battleSaveController = new BattleSaveController(playerContext, 
        playerPresenterContext, BattleSaveUI, saveDataLoader, saves,
        battleSimulationDebugController);
      
      var battleSetupController = new BattleSetupController(playerContext, 
        playerPresenterContext, BattleSetupUI);

      var unitModelDebugController = new UnitModelDebugController(playerContext, board, ModelUI, 
        DebugController.Info, unitSelectionController);
      
      var takenCoordDebugController = new TakenCoordDebugController(board, DebugController,
        tileSpawner);
      
      var targetDebugController = new TargetDebugController(
        board, coordFinder, DebugController.Info);
      
      var uiDebugController = new UIDebugController(
        BattleSetupUI, BattleSaveUI, BattleSimulationUI,
        unitModelDebugController);

      #endregion

      yield return null;
      #region Infrastructure

      tickController.InitObservable(takenCoordDebugController, targetDebugController, 
        uiDebugController, unitModelDebugController, realtimeBattleSimulationController, 
        DebugController); //TODO: register implicitly?
      inputController.InitObservables();

      #endregion
      #region View

      BattleSetupUI.SetDropdownOptions(units.Keys.ToList());
      BattleSaveUI.SubToUI(saves.Keys.ToList());
      tileSpawner.SpawnTiles();

      #endregion
      decisionTreeCreatorVisitor.Init();
      #region Controller

      unitSelectionController.SubToInput(disposable);
      battleStateController.SubToUI();
      unitDragController.SubToUnitSelection(disposable);
      tileHighlightController.SubToDrag(disposable);
      unitMoveController.SubToDrag(disposable);
      unitTooltipController.SubToUnitSelection(disposable);

      #endregion
      #region Debug

      battleSaveController.SubToUI();
      battleSetupController.SubToUI();
      unitModelDebugController.SubToUnitSelection(disposable);
      battleSimulationDebugController.SubToUI();
      DebugController.Init(UnitTooltipUI);

      #endregion

      MonoBehaviourCallBackController.StartUpdating(tickController);
    }

    void OnDestroy() => disposable.Clear();

    static readonly Logger log = MainLog.GetLogger(nameof(CompositionRoot));
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}
