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
using View.Presenters;
using View.UIs;
using View.Views;
using Logger = Shared.OkwyLogging.Logger;

namespace Infrastructure {
  public class CompositionRoot : MonoBehaviour {
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
      // MainLog.DefaultInit();
      log.Info("\n\nStart");
      var units = new UnitInfoLoader().Load();
      var saveDataLoader = new SaveInfoLoader();
      var saves = saveDataLoader.Load();
      
      var tickController = new TickController();
      var inputController = new InputController(tickController);
      var eventBus = new EventBus(); //TODO: stop using eventbus Ievent interface to remove reference on that library
      var eventHolder = new EventHolder(eventBus);
      var unitFactory = new UnitFactory(units, new DecisionFactory(eventHolder));

      BattleSetupUI.Init(units.Keys.ToList());
      BattleSaveUI.Init(saves.Keys.ToList());
      
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
                  
      var unitTooltipController = new UnitTooltipController(UnitTooltipUI);

      var player1BoardUnits = new UnitDict(unitFactory);
      var player2BoardUnits = new UnitDict(unitFactory);
      var players = new[] {
        new Player(EPlayer.First, player1BoardUnits, new UnitDict(unitFactory)), 
        new Player(EPlayer.Second, player2BoardUnits, new UnitDict(unitFactory))
      };
      var board = new Board(new UnitDict(unitFactory), player1BoardUnits,
        player2BoardUnits);
      var aiContext = new AiContext(board);
      
      var raycastController = new RaycastController(Camera.main, 
        LayerMask.GetMask("Terrain", "GlobalCollider"), 
        LayerMask.GetMask("Unit"), 
        unitTooltipController);
      
      var unitModelDebugController = new UnitModelDebugController(
        new ModelContext(players), ModelUI);
      
      var takenCoordDebugController = new TakenCoordDebugController(tilePresenter, board);
      var targetDebugController = new TargetDebugController(board, tilePresenter);
      var uiDebugController = new UIDebugController(
        BattleSetupUI, BattleSaveUI, BattleSimulationUI,
        unitModelDebugController);
      
      var battleStateController = new BattleStateController(BattleSimulationUI);
      var tileHighlighter = new TileHighlighter(tilePresenter);

      var unitSelectionController = new UnitSelectionController(
        battleStateController, unitModelDebugController, BattleSetupUI);
      
      var unitDragController = new UnitDragController(raycastController, 
        new CoordFinder(tilePresenter, BattleSetupUI), inputController, 
        new CompositeHandler<EndDragEvent>(
          new WorldContext(players, playerPresenters, BattleSetupUI), tileHighlighter), 
        tileHighlighter, unitSelectionController);

      var movementController = new MovementController(boardPresenter, tilePresenter);
      var attackController = new AttackController(boardPresenter, UnitTooltipUI);
      eventBus.Register<StartMoveEvent>(movementController);
      eventBus.Register<EndMoveEvent>(movementController);
      eventBus.Register<ApplyDamageEvent>(attackController);
      eventBus.Register<DeathEvent>(attackController);

      var battleSimulation = new BattleSimulation(aiContext);
      var realtimeBattleSimulationController = new RealtimeBattleSimulationController(
        movementController, battleSimulation, eventHolder);
      var battleSimulationController = new BattleSimulationDebugController(
        battleSimulation, BattleSimulationUI, movementController, 
        aiContext, players, boardPresenter, playerPresenters, realtimeBattleSimulationController);

      var battleSetupController = new BattleSetupController(players, playerPresenters, 
        BattleSetupUI);
      var battleSaveController = new BattleSaveController(players, playerPresenters, 
        BattleSaveUI, saveDataLoader, saves);

      yield return null;

      eventHolder.Init(aiContext);
      tickController.Init(takenCoordDebugController, targetDebugController, uiDebugController, 
        unitModelDebugController, raycastController, realtimeBattleSimulationController);
      inputController.Init();
      unitDragController.Init();
      
      tickInput.Init(tickController);
    }

    static readonly Logger log = MainLog.GetLogger(nameof(CompositionRoot));
  }
}
