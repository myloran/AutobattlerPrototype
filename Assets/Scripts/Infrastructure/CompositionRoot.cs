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
using View.Presenters;

namespace Infrastructure {
  public class CompositionRoot : MonoBehaviour {
    public UpdateInput UpdateInput;
    public BattleSetupUI BattleSetupUI;
    public BattleSaveUI BattleSaveUI;
    public BattleSimulationUI BattleSimulationUI;
    public UnitTooltipUI UnitTooltipUI;
    public TileStartPoints TileStartPoints;
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
      
      var tileViewFactory = new TileViewFactory(TileView);
      
      BattleSetupUI.Init(units.Keys.ToList());
      BattleSaveUI.Init(saves.Keys.ToList());
      
      var tilePresenter = new TilePresenter(TileStartPoints, tileViewFactory);
      var playerPresenter1 = new PlayerPresenter();
      var playerPresenter2 = new PlayerPresenter();
      var playerPresenters = new[] {playerPresenter1, playerPresenter2};
                  
      var unitTooltipController = new UnitTooltipController(UnitTooltipUI);
      var battleStateController = new BattleStateController(BattleSimulationUI);
      var unitViewFactory = new UnitViewFactory(units, UnitView, tilePresenter);
      var unitViewFactoryDecorator = new UnitViewFactoryDecorator(tilePresenter, 
        BattleSetupUI, players, playerPresenters, unitTooltipController, unitViewFactory, battleStateController);
      
      var board1UnitDict = new UnitViewDict(unitViewFactoryDecorator);
      var bench1UnitDict = new UnitViewDict(unitViewFactoryDecorator);
      var board2UnitDict = new UnitViewDict(unitViewFactoryDecorator);
      var bench2UnitDict = new UnitViewDict(unitViewFactoryDecorator);
      var player1Units = new UnitViewDict(unitViewFactoryDecorator);
      var player2Units = new UnitViewDict(unitViewFactoryDecorator);
      var allUnitsDict = new UnitViewDict(unitViewFactoryDecorator);
      var boardPresenter = new BoardPresenter(allUnitsDict, player1Units, player2Units,
          tilePresenter);
      playerPresenter1.Init(tilePresenter, board1UnitDict, bench1UnitDict);
      playerPresenter2.Init(tilePresenter, board2UnitDict, bench2UnitDict);

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
      var unitDebugController = new TargetDebugController(board, tilePresenter);
      var updateController = new UpdateController(unitDebugController, debugUIController, 
        raycastController);
      UpdateInput.Init(updateController);
      
      var movementController = new MovementController(boardPresenter, tilePresenter);
      var attackController = new AttackController(boardPresenter, UnitTooltipUI);
      eventBus.Register<StartMoveEvent>(movementController);
      eventBus.Register<EndMoveEvent>(movementController);
      eventBus.Register<ApplyDamageEvent>(attackController);
      eventBus.Register<DeathEvent>(attackController);

      var battleSimulationController = new BattleSimulationController(battleSimulation,
        BattleSimulationUI, movementController, aiContext, players, boardPresenter,
        playerPresenters);

      var battleSetupController = new BattleSetupController(players, playerPresenters, 
        BattleSetupUI);
      var battleSaveController = new BattleSaveController(players, playerPresenters, 
        BattleSaveUI, saveDataLoader, saves);
    }
  }
}
