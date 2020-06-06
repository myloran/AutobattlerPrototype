using System.Collections;
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
using Model;
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

    IEnumerator Start() {
      MainLog.DefaultInit();
      var units = new UnitInfoLoader().Load();
      var saveDataLoader = new SaveInfoLoader();
      var saves = saveDataLoader.Load();
      
      var eventBus = new EventBus();
      var unitFactory = new UnitFactory(units, new DecisionFactory(eventBus));
      
      var players = new[] {
        new Player(new UnitDict(unitFactory), new UnitDict(unitFactory)), 
        new Player(new UnitDict(unitFactory), new UnitDict(unitFactory))
      };
      
      BattleSetupUI.Init(units.Keys.ToList());
      BattleSaveUI.Init(saves.Keys.ToList());
      
      var tilePresenter = new TilePresenter(TileStartPoints, new TileViewFactory(TileView));
      var unitViewFactory = new UnitViewFactory(units, UnitView, tilePresenter);

      var boardPresenter = new BoardPresenter(
        new UnitViewDict(unitViewFactory), new UnitViewDict(unitViewFactory),
        new UnitViewDict(unitViewFactory), tilePresenter);
      
      var playerPresenters = new[] {
        new PlayerPresenter(new UnitViewDict(unitViewFactory), 
          new UnitViewDict(unitViewFactory), tilePresenter), 
        new PlayerPresenter(new UnitViewDict(unitViewFactory), 
          new UnitViewDict(unitViewFactory), tilePresenter)
      };
                  
      var unitTooltipController = new UnitTooltipController(UnitTooltipUI);
      var board = new Board();
      var aiContext = new AiContext(board, 
        new FibonacciHeap<ICommand, TimePoint>(float.MinValue));

      var raycastController = new RaycastController(Camera.main, 
        LayerMask.GetMask("Terrain", "GlobalCollider"), 
        LayerMask.GetMask("Unit"), 
        unitTooltipController);
      
      var unitDragController2 = new UnitDragController(tilePresenter, BattleSetupUI,
        players, playerPresenters, unitTooltipController, 
        new BattleStateController(BattleSimulationUI), raycastController);
      
      UpdateInput.Init(new UpdateController(
        new TargetDebugController(board, tilePresenter), 
        new DebugUIController(BattleSetupUI, BattleSaveUI, BattleSimulationUI), 
        raycastController));
      
      var movementController = new MovementController(boardPresenter, tilePresenter);
      var attackController = new AttackController(boardPresenter, UnitTooltipUI);
      eventBus.Register<StartMoveEvent>(movementController);
      eventBus.Register<EndMoveEvent>(movementController);
      eventBus.Register<ApplyDamageEvent>(attackController);
      eventBus.Register<DeathEvent>(attackController);

      var battleSimulationController = new BattleSimulationController(
        new BattleSimulation(aiContext), BattleSimulationUI, movementController, 
        aiContext, players, boardPresenter, playerPresenters);

      var battleSetupController = new BattleSetupController(players, playerPresenters, 
        BattleSetupUI);
      var battleSaveController = new BattleSaveController(players, playerPresenters, 
        BattleSaveUI, saveDataLoader, saves);

      yield return null;
      
      unitDragController2.Init();
    }
  }
}
