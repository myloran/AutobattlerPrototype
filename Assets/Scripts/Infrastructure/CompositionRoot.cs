using System;
using System.Collections;
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
using View;
using FibonacciHeap;
using Model;
using Model.NBattleSimulation.Commands;
using PlasticFloor.EventBus;
using Shared.OkwyLogging;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using View.Factories;
using View.NUnit;
using View.Presenters;
using View.UIs;
using View.Views;
using Logger = Shared.OkwyLogging.Logger;

namespace Infrastructure {
  public class CompositionRoot : MonoBehaviour {
    public DebugController DebugController;
    public TickInput tickInput;
    public BattleSetupUI BattleSetupUI;
    public BattleSaveUI BattleSaveUI;
    public BattleSimulationUI BattleSimulationUI;
    public UnitTooltipUI UnitTooltipUI;
    public ModelUI ModelUI;
    public TileStartPoints TileStartPoints;
    public UnitView UnitView;
    public TileView TileView;

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
      
      var raycastController = new RaycastController(Camera.main, 
        LayerMask.GetMask("Terrain", "GlobalCollider"), LayerMask.GetMask("Unit"));
      
      #endregion
      #region View
      
      var tilePresenter = new TilePresenter(TileStartPoints, new TileViewFactory(TileView));
      var unitViewFactory = new UnitViewFactory(units, UnitView, tilePresenter);

      var boardPresenter = new BoardPresenter(
        new UnitViewDict(unitViewFactory), new UnitViewDict(unitViewFactory),
        new UnitViewDict(unitViewFactory), tilePresenter);
      
      var playerPresenters = new[] {
        new PlayerPresenter(EPlayer.First, new UnitViewDict(unitViewFactory), 
          new UnitViewDict(unitViewFactory), tilePresenter), 
        new PlayerPresenter(EPlayer.Second, new UnitViewDict(unitViewFactory), 
          new UnitViewDict(unitViewFactory), tilePresenter)
      };
      
      #endregion
      #region Model

      var unitFactory = new UnitFactory(units, new DecisionFactory(eventHolder));
      var player1BoardUnits = new UnitDict(unitFactory);
      var player2BoardUnits = new UnitDict(unitFactory);
      var players = new[] {
        new Player(EPlayer.First, player1BoardUnits, new UnitDict(unitFactory)), 
        new Player(EPlayer.Second, player2BoardUnits, new UnitDict(unitFactory))
      };
      var board = new Board(new UnitDict(unitFactory), player1BoardUnits,
        player2BoardUnits);
      var aiContext = new AiContext(board);

      #endregion
      #region Context

      var worldContext = new WorldContext(players, playerPresenters, BattleSetupUI);

      #endregion

      var unitTooltipController = new UnitTooltipController(UnitTooltipUI);

      #region Debug

      var unitModelDebugController = new UnitModelDebugController(
        new ModelContext(players), ModelUI, DebugController.Info);
      
      var takenCoordDebugController = new TakenCoordDebugController(
        tilePresenter, board, DebugController.Info);
      
      var targetDebugController = new TargetDebugController(
        board, tilePresenter, DebugController.Info);
      
      var uiDebugController = new UIDebugController(
        BattleSetupUI, BattleSaveUI, BattleSimulationUI,
        unitModelDebugController);

      #endregion
      #region Unit drag

      var unitDragController = new UnitDragController(raycastController, 
        new CoordFinder(tilePresenter, BattleSetupUI), inputController, 
        new CanStartDrag(new BattleStateController(BattleSimulationUI), 
          unitModelDebugController, BattleSetupUI, unitTooltipController));
      
      var tileHighlightController = new TileHighlighterController(tilePresenter, 
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

      var battleSimulation = new BattleSimulation(aiContext);
      var realtimeBattleSimulationController = new RealtimeBattleSimulationController(
        movementController, battleSimulation, eventHolder);

      #endregion
      
      var battleSimulationController = new BattleSimulationDebugController(
        battleSimulation, BattleSimulationUI, movementController, 
        aiContext, players, boardPresenter, playerPresenters, realtimeBattleSimulationController,
        tilePresenter);

      var battleSetupController = new BattleSetupController(players, playerPresenters, 
        BattleSetupUI);
      var battleSaveController = new BattleSaveController(players, playerPresenters, 
        BattleSaveUI, saveDataLoader, saves);

      yield return null;
      
      BattleSetupUI.Init(units.Keys.ToList());
      BattleSaveUI.Init(saves.Keys.ToList());

      eventHolder.Init(aiContext); //TODO: move parameters to constructor
      tickController.Init(takenCoordDebugController, targetDebugController, uiDebugController, 
        unitModelDebugController, realtimeBattleSimulationController);
      inputController.Init();
      unitDragController.Init();
      
      tileHighlightController.Init();
      unitMoveController.Init();
      
      
      tickInput.Init(tickController);
    }

    static readonly Logger log = MainLog.GetLogger(nameof(CompositionRoot));
  }
}
