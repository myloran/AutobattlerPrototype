using System.Linq;
using Controller;
using Controller.NBattleSimulation;
using Controller.Save;
using Model.NBattleSimulation;
using Model.NUnit;
using Shared;
using UnityEngine;
using View;
using View.UI;
using FibonacciHeap;
using Model.NBattleSimulation.Commands;
using Okwy.Logging;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Infrastructure {
  public class CompositionRoot : MonoBehaviour {
    public UpdateInput UpdateInput;
    public DebugUIController DebugUIController;
    public RaycastController RaycastController;
    public BattleSetupUI BattleSetupUI;
    public BattleSaveUI BattleSaveUI;
    public BattleSimulationUI BattleSimulationUI;
    public UnitTooltipUI UnitTooltipUI;
    public Transform BoardStartPoint;
    public BenchView BenchView1, BenchView2;
    public UnitView UnitView;
    public TileView TileView;

    void Start() {
      MainLog.DefaultInit();
      var unitDataLoader = new UnitInfoLoader();
      var units = unitDataLoader.Load();
      var saveDataLoader = new SaveInfoLoader();
      var saves = saveDataLoader.Load();

      DebugUIController.Init(BattleSetupUI, BattleSaveUI, BattleSimulationUI);
      
      var eventBus = new EventBus();
      var decisionFactory = new DecisionFactory(eventBus);
      var unitFactory = new UnitFactory(units, decisionFactory);
      var players = new[] {new Player(unitFactory), new Player(unitFactory)};
      
      var unitViewFactory = new UnitViewFactory(units, UnitView);
      var tileViewFactory = new TileViewFactory(TileView);
      
      var boardView = new BoardView();
      var closestTileFinder = new ClosestTileFinder(boardView, BenchView1, BenchView2);

      var board = new Board();
      var aiHeap = new FibonacciHeap<ICommand, TimePoint>(float.MinValue);
      var aiContext = new AiContext(board, aiHeap);
      var battleSimulation = new BattleSimulation(aiContext);
            
      var unitDebugController = new UnitDebugController(board, boardView);
      var updateController = new UpdateController(unitDebugController);
      UpdateInput.Init(updateController);
      
      var movementController = new MovementController(boardView);
      var attackController = new AttackController(boardView, UnitTooltipUI);
      eventBus.Register<StartMoveEvent>(movementController);
      eventBus.Register<EndMoveEvent>(movementController);
      eventBus.Register<ApplyDamageEvent>(attackController);
      eventBus.Register<DeathEvent>(attackController);

      var battleSimulationController = new BattleSimulationController(battleSimulation,
        BattleSimulationUI, movementController, aiContext, players);
      
      var unitTooltipController = new UnitTooltipController(battleSimulationController,
        UnitTooltipUI);
      RaycastController.Init(unitTooltipController);
      
      var unitViewFactoryDecorator = new UnitViewFactoryDecorator(closestTileFinder, 
        BattleSetupUI, players, unitTooltipController, unitViewFactory);
      
      boardView.Init(BoardStartPoint, tileViewFactory, unitViewFactoryDecorator);
      
      BattleSetupUI.Init(units.Keys.ToList());
      BattleSaveUI.Init(saves.Keys.ToList());
      BenchView1.Init(unitViewFactoryDecorator, tileViewFactory, EPlayer.First);
      BenchView2.Init(unitViewFactoryDecorator, tileViewFactory, EPlayer.Second);
      
      var benches = new[] {BenchView1, BenchView2};
      var battleSetupController = new BattleSetupController(players, benches, BattleSetupUI);
      var battleSaveController = new BattleSaveController(players, benches, boardView, 
        BattleSaveUI, saveDataLoader, saves);
    }
  }
}
