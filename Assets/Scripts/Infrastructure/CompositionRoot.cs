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
      
      var eventBus = new EventBus();
      var decisionFactory = new DecisionFactory(eventBus);
      var unitFactory = new UnitFactory(units, decisionFactory);
      var players = new[] {new Player(unitFactory), new Player(unitFactory)};
      
      var unitViewFactory = new UnitViewFactory(units, UnitView);
      var tileViewFactory = new TileViewFactory(TileView);
      
      var boardView = new BoardView();
      var closestTileFinder = new ClosestTileFinder(boardView, BenchView1, BenchView2);
                  
      var unitTooltipController = new UnitTooltipController(UnitTooltipUI);
      var battleStateController = new BattleStateController(BattleSimulationUI);
      var unitViewFactoryDecorator = new UnitViewFactoryDecorator(closestTileFinder, 
        BattleSetupUI, players, unitTooltipController, unitViewFactory, battleStateController);
      
      boardView.Init(BoardStartPoint, tileViewFactory, unitViewFactoryDecorator);
      
      var board = new Board();
      var aiHeap = new FibonacciHeap<ICommand, TimePoint>(float.MinValue);
      var aiContext = new AiContext(board, aiHeap);
      var battleSimulation = new BattleSimulation(aiContext);

      var mainCamera = Camera.main;
      var globalLayer = LayerMask.GetMask("Terrain", "GlobalCollider");
      var unitLayer = LayerMask.GetMask("Unit");
      var raycastController = new RaycastController(mainCamera, globalLayer, unitLayer, 
        unitTooltipController);
            
      var debugUIController = new DebugUIController(BattleSetupUI, BattleSaveUI, BattleSimulationUI);
      var unitDebugController = new TargetDebugController(board, boardView);
      var updateController = new UpdateController(unitDebugController, debugUIController, 
        raycastController);
      UpdateInput.Init(updateController);
      
      var movementController = new MovementController(boardView);
      var attackController = new AttackController(boardView, UnitTooltipUI);
      eventBus.Register<StartMoveEvent>(movementController);
      eventBus.Register<EndMoveEvent>(movementController);
      eventBus.Register<ApplyDamageEvent>(attackController);
      eventBus.Register<DeathEvent>(attackController);

      var battleSimulationController = new BattleSimulationController(battleSimulation,
        BattleSimulationUI, movementController, aiContext, players);

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
