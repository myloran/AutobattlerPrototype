using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Controller.Ai;
using Controller.DecisionTree.Data;
using Controller.DecisionTree.Visitor;
using Controller.NBattleSimulation;
using Controller.NDebug;
using Controller.NDebug.CommandsDebug;
using Controller.NTile;
using Controller.NUnit;
using Controller.Save;
using Controller.Test;
using Controller.TestCases;
using Controller.UnitDrag;
using Controller.Update;
using Infrastructure.OkwyLoggingUnity;
using Model.NBattleSimulation;
using Model.NUnit;
using UnityEngine;
using Model;
using Model.NAbility;
using PlasticFloor.EventBus;
using Shared.Abstraction;
using Shared.Addons.OkwyLogging;
using Shared.Shared.Client.Events;
using SharedClient.Abstraction;
using UniRx;
using View.NTile;
using View.NUnit;
using View.Presenters;
using View.UIs;
using View.UIToolkit;
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
    public CommandDebugWindowUI CommandDebugWindowUI;
    public DebugWindowUI DebugWindowUI;

    #endregion
    #region View

    public TileStartPoints TileStartPoints;
    public UnitViewInfoHolder UnitViewInfoHolder; 
    public UnitView UnitViewPrefab;
    public TileView TileViewPrefab;

    #endregion
    #endregion

    IEnumerator Start() {
      #region Infrastructure
      
      #region Config
      
      OkwyDefaultLog.DefaultInit();
      var units = new UnitInfoLoader().Load();
      var abilities = new AbilityInfoLoader().Load();
      var saveDataLoader = new SaveInfoLoader();
      var saves = saveDataLoader.Load();
      var decisionTreeLoader = new DecisionTreeDataLoader();
      var decisionTreeComponent = decisionTreeLoader.Load();
      
      #endregion
      
      var tickController = new TickController();
      var inputController = new InputController(tickController);
      //TODO: implement event bus that won't allocate(no delegates)
      var eventBus = new EventBus { //TODO: stop using eventbus Ievent interface to remove reference on that library from model
        Log = m => log.Info($"{m}"), IsLogOn = () => DebugController.Info.IsDebugOn //TODO: remove that lmao
      };

      #endregion
      #region View
      
      var mainCamera = Camera.main;
      var tileSpawner = new TilePresenter(TileStartPoints, new TileViewFactory(TileViewPrefab));
      var coordFinder = new CoordFinder(TileStartPoints);
      var unitViewFactory = new UnitViewFactory(units, UnitViewInfoHolder, coordFinder, mainCamera);
      var unitViewCoordChangedHandler = new UnitViewCoordChangedHandler(coordFinder);
      var boardPresenter = new BoardPresenter(unitViewCoordChangedHandler);
      
      var playerPresenterContext = new PlayerPresenterContext(
        new PlayerPresenter(unitViewFactory, unitViewCoordChangedHandler), 
        new PlayerPresenter(unitViewFactory, unitViewCoordChangedHandler));
      
      var battleSimulationUI2 = new BattleSimulationUI2();
      var commandDebugUI = new CommandDebugUI();
      
      CommandDebugWindowUI.gameObject.SetActive(true);
      DebugWindowUI.gameObject.SetActive(true);
      #endregion
      #region Model
      var decisionTreeLookup = new DecisionTreeLookup();

      Action makeDecisionLog = () => {};
      var decisionTreeCreatorVisitor = new DecisionTreeCreatorVisitor(eventBus, 
        d => new LoggingDecorator(d, DebugController.Info, makeDecisionLog), decisionTreeLookup);

      var unitFactory = new UnitFactory(units, new DecisionFactory(
        decisionTreeCreatorVisitor, decisionTreeComponent), abilities, 
        new AbilityFactory(abilities, eventBus));
      
      //TODO: replace board/bench dictionaries with array?               
      var playerContext = new PlayerContext(new Player(unitFactory), new Player(unitFactory));
      var board = new Board();
      var aiHeap = new AiHeap();
      var aiContext = new AiContext(board, aiHeap);

      #endregion
      #region Shared

      var playerSharedContext = new PlayerSharedContext(playerContext, playerPresenterContext, BattleSetupUI);

      #endregion
      #region Controller
      
      #region Unit selection

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

      var unitMoveController = new UnitMoveController(playerSharedContext, unitDragController);

      #endregion
      #region Battle simulation

      var movementController = new MovementController(boardPresenter, coordFinder);
      var attackController = new AttackController(boardPresenter, unitTooltipController);
      var animationController = new AnimationController(boardPresenter);
      var silenceController = new SilenceController(boardPresenter);
      var projectileController = new ProjectileController(boardPresenter, coordFinder);

      var compositeSimulationTick = new CompositeSimulationTick(movementController, silenceController, projectileController);
      var compositeReset = new CompositeReset(movementController, silenceController, projectileController);
      
      var battleSimulationPresenter = new BattleSimulationPresenter(coordFinder, 
        boardPresenter, compositeSimulationTick, compositeReset);
      
      var battleSimulation = new BattleSimulation(aiContext, board, aiHeap);
      
      var realtimeBattleSimulationController = new RealtimeBattleSimulationController(
        compositeSimulationTick, battleSimulation);

      #endregion
      #region Debug

      var commandEvents = new EventRow(battleSimulation, eventBus, CommandDebugWindowUI.EventTemplate);
      var commandButtonStyler = new CommandButtonStyler(boardPresenter);
      var commandsHandler = new CommandRow(commandEvents, commandButtonStyler, CommandDebugWindowUI);
      var commandsDebugController = new CommandsDebugController(aiHeap, commandsHandler, CommandDebugWindowUI);
      var testFramework = new TestFramework(playerSharedContext);
      
      var battleSimulationDebugController = new BattleSimulationDebugController(battleSimulation, BattleSimulationUI, 
        aiContext, playerContext, playerPresenterContext, realtimeBattleSimulationController, battleSimulationPresenter,
        commandDebugUI, testFramework);
      
      var battleSaveController = new BattleSaveController(playerSharedContext, 
        BattleSaveUI, saveDataLoader, saves,
        battleSimulationDebugController);
      
      var battleSetupController = new BattleSetupController(playerContext, 
        playerPresenterContext, BattleSetupUI);

      var unitModelDebugController = new UnitModelDebugController(playerContext, board, ModelUI, 
        DebugController.Info, unitSelectionController, aiHeap, coordFinder);
      
      var takenCoordDebugController = new TakenCoordDebugController(board, DebugController,
        tileSpawner);
      
      var targetDebugController = new TargetDebugController(
        board, coordFinder, DebugController.Info);
      
      var uiDebugController = new UIDebugController(
        BattleSetupUI, BattleSaveUI, BattleSimulationUI,
        unitModelDebugController, CommandDebugWindowUI);

      #endregion
      
      #endregion

      #region Infrastructure

      tickController.InitObservable(takenCoordDebugController, targetDebugController, 
        uiDebugController, unitModelDebugController, realtimeBattleSimulationController, 
        DebugController); //TODO: register implicitly?
      inputController.InitObservables();

      #endregion
      #region View

      battleSimulationUI2.Init(DebugWindowUI.Document);
      commandDebugUI.FillReferences(CommandDebugWindowUI.Document);
      
      BattleSetupUI.SetDropdownOptions(units.Keys.ToList());
      BattleSaveUI.SubToUI(saves.Keys.ToList());
      tileSpawner.SpawnTiles();

      #endregion
      #region Model

      decisionTreeCreatorVisitor.Init();
      eventBus.Register<StartMoveEvent>(movementController, animationController); //TODO: register implicitly?
      eventBus.Register<FinishMoveEvent>(movementController);
      eventBus.Register<RotateEvent>(movementController);
      eventBus.Register<UpdateHealthEvent>(attackController);
      eventBus.Register<UpdateManaEvent>(attackController);
      eventBus.Register<DeathEvent>(attackController);
      eventBus.Register<IdleEvent>(animationController);
      eventBus.Register<StartAttackEvent>(animationController);
      eventBus.Register<StartCastEvent>(animationController);
      eventBus.Register<UpdateSilenceDurationEvent>(silenceController);
      eventBus.Register<SpawnProjectileEvent>(projectileController);

      #endregion
      #region Controller

      unitSelectionController.SubToInput(disposable);
      battleStateController.SubToUI();
      unitDragController.SubToUnitSelection(disposable);
      tileHighlightController.SubToDrag(disposable);
      unitMoveController.SubToDrag(disposable);
      unitTooltipController.SubToUnitSelection(disposable);
      
      #region Debug

      battleSaveController.SubToUI();
      battleSetupController.SubToUI();
      unitModelDebugController.SubToUnitSelection(disposable);
      battleSimulationDebugController.SubToUI(disposable);
      commandEvents.Init(disposable);
      commandsDebugController.Init();
      DebugController.Init(UnitTooltipUI);

      #endregion

      #endregion

      MonoBehaviourCallBackController.StartUpdating(tickController);

      yield return null;
    }

    void OnDestroy() => disposable.Clear();

    static readonly Logger log = MainLog.GetLogger(nameof(CompositionRoot));
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}
