using System.Linq;
using Controller;
using Controller.Save;
using Model.NBattleSimulation;
using Model.NUnit;
using Shared;
using UnityEngine;
using View;
using View.UI;
using FibonacciHeap;
using Model.NBattleSimulation.Commands;
using PlasticFloor.EventBus;
using Shared.Events;

namespace Infrastructure {
  public class CompositionRoot : MonoBehaviour {
    public BattleSetupUI BattleSetupUI;
    public BattleSaveUI BattleSaveUI;
    public BattleSimulationUI BattleSimulationUI;
    public BoardView BoardView;
    public BenchView BenchView1, BenchView2;
    public UnitView UnitView;
    public TileView TileView;

    void Start() {
      var unitDataLoader = new UnitInfoLoader();
      var units = unitDataLoader.Load();
      var saveDataLoader = new SaveInfoLoader();
      var saves = saveDataLoader.Load();
      
      var eventBus = new EventBus();
      var movementController = new MovementController();
      eventBus.Register(movementController);
               
      var decisionFactory = new DecisionFactory(eventBus);
      var unitFactory = new UnitFactory(units, decisionFactory);
      var players = new[] {new Player(unitFactory), new Player(unitFactory)};
      var closestTileFinder = new ClosestTileFinder(BoardView, BenchView1, BenchView2);
      var unitViewFactory = new UnitViewFactory(units, UnitView);
      
      var unitViewFactoryDecorator = new UnitViewFactoryDecorator(closestTileFinder, 
        BattleSetupUI, players, unitViewFactory);
      
      var tileViewFactory = new TileViewFactory(TileView);
      BattleSetupUI.Init(units.Keys.ToList());
      BattleSaveUI.Init(saves.Keys.ToList());
      BoardView.Init(tileViewFactory, unitViewFactoryDecorator);
      BenchView1.Init(unitViewFactoryDecorator, tileViewFactory, EPlayer.First);
      BenchView2.Init(unitViewFactoryDecorator, tileViewFactory, EPlayer.Second);
      
      var benches = new[] {BenchView1, BenchView2};
      var battleSetupController = new BattleSetupController(players, benches, BattleSetupUI);
      var battleSaveController = new BattleSaveController(players, benches, BoardView, 
        BattleSaveUI, saveDataLoader, saves);

      var board = new Board();
      var aiHeap = new FibonacciHeap<ICommand, TimePoint>(float.MinValue);
      var aiContext = new AiContext(players, board, aiHeap);
      var battleSimulation = new BattleSimulation(aiContext);

      var battleSimulationController = new BattleSimulationController(battleSimulation,
        BattleSimulationUI);
    }
  }
}
