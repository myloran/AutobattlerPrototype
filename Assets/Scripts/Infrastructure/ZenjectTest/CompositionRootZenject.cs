using System;
using System.Collections.Generic;
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
using Controller.TestCases;
using Controller.UnitDrag;
using Controller.Update;
using Infrastructure.OkwyLoggingUnity;
using Model;
using Model.NAbility;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NSynergy;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared.Abstraction;
using Shared.Addons.OkwyLogging;
using Shared.Primitives;
using Shared.Shared.Client.Events;
using SharedClient.Abstraction;
using UniRx;
using UnityEngine;
using View.NTile;
using View.NUnit;
using View.Presenters;
using View.UIs;
using View.UIToolkit;
using View.Views;
using Zenject;
using Logger = Shared.Addons.OkwyLogging.Logger;

namespace Infrastructure.ZenjectTest {
  public class CompositionRootZenject : MonoInstaller {
    #region Monobehaviours
    #region Controller

    public InitializationFlow InitializationFlow;
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
    
    public override void InstallBindings() {
      #region Infrastructure
      
      #region Config
      
      OkwyDefaultLog.DefaultInit();
      Container.Bind(typeof(InfoLoader<UnitInfo>), typeof(IInfoGetter<UnitInfo>)).To<InfoLoader<UnitInfo>>().AsSingle();
      Container.Bind(typeof(InfoLoader<AbilityInfo>), typeof(IInfoGetter<AbilityInfo>)).To<InfoLoader<AbilityInfo>>().AsSingle();
      Container.Bind(typeof(InfoLoader<SynergyInfo>), typeof(IInfoGetter<SynergyInfo>)).To<InfoLoader<SynergyInfo>>().AsSingle();
      Container.Bind(typeof(InfoLoader<EffectInfo>), typeof(IInfoGetter<EffectInfo>)).To<InfoLoader<EffectInfo>>().AsSingle();

      var saveDataLoader = new SaveInfoLoader(); //rework similar to info loader
      Container.Bind<SaveInfoLoader>().FromInstance(saveDataLoader).AsSingle();
      var battleSaveGetter = new BattleSaveGetter { Saves = saveDataLoader.Load() }; //remove later
      Container.Bind<BattleSaveGetter>().FromInstance(battleSaveGetter).AsSingle();
      
      var decisionTreeLoader = new DecisionTreeDataLoader();
      Container.Bind<DecisionTreeDataLoader>().FromInstance(decisionTreeLoader).AsSingle();
      var decisionTreeComponent = decisionTreeLoader.Load();
      Container.Bind<DecisionTreeComponent>().FromInstance(decisionTreeComponent).AsSingle();
      
      #endregion
      
      Container.BindInterfacesTo<InitializationFlow>().FromInstance(InitializationFlow).AsSingle();
      Container.Bind<MonoBehaviourCallBackController>().FromInstance(MonoBehaviourCallBackController).AsSingle();
      Container.Bind<TickController>().AsSingle();
      Container.Bind<InputController>().AsSingle();

      //custom bind to inject stuff into eventBus or rework it a little bit 
      //TODO: implement event bus that won't allocate(no delegates)
      var eventBus = new EventBus { //TODO: stop using eventbus Ievent interface to remove reference on that library from model
        Log = m => log.Info($"{m}"), IsLogOn = () => DebugController.Info.IsDebugOn //TODO: remove that lmao
      };
      Container.Bind(typeof(IEventBus),typeof(EventBus)).FromInstance(eventBus).AsSingle();

      #endregion
      #region View
      
      Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
      Container.Bind<TileView>().FromInstance(TileViewPrefab).AsSingle();
      Container.Bind<TileStartPoints>().FromInstance(TileStartPoints).AsSingle();
      Container.Bind<UnitViewInfoHolder>().FromInstance(UnitViewInfoHolder).AsSingle();
      Container.Bind<TileViewFactory>().AsSingle();
      Container.Bind<TilePresenter>().AsSingle();
      Container.Bind<CoordFinder>().AsSingle();
      Container.Bind<UnitViewFactory>().AsSingle();
      Container.Bind<UnitViewCoordChangedHandler>().AsSingle();
      Container.Bind<BoardPresenter>().AsSingle();
      Container.Bind<PlayerPresenter>().AsTransient(); //need to have 2 separate instances, as transient should work
      Container.Bind<PlayerPresenterContext>().AsSingle();
      Container.Bind<CommandDebugUI>().AsSingle();

      CommandDebugWindowUI.gameObject.SetActive(true);
      // DebugWindowUI.gameObject.SetActive(true);
      
      #endregion
      #region Model
      
      Container.Bind<DecisionTreeLookup>().AsSingle();
      Container.Bind<DebugInfo>().FromInstance(DebugController.Info).AsSingle();
      // Container.Bind<LoggingDecorator>().AsSingle();  //need to pass custom func
      Action makeDecisionLog = () => {};
      Func<IDecisionTreeNode, IDecisionTreeNode> logDecisionTree = d => new LoggingDecorator(d, DebugController.Info, makeDecisionLog);
      Container.Bind<DecisionTreeCreatorVisitor>().AsSingle().WithArguments(logDecisionTree); //need to pass custom func
      Container.Bind<SystemRandomEmbedded>().AsSingle().WithArguments(0); //need to pass int
      Container.Bind<EffectFactory>().AsSingle();
      Container.Bind<AbilityFactory>().AsSingle(); //need to pass info
      Container.Bind(typeof(IDecisionTreeFactory), typeof(DecisionFactory)).To<DecisionFactory>().AsSingle();
      Container.Bind<UnitFactory>().AsSingle();
      Container.Bind<Player>().AsTransient(); //as transient should work
      Container.Bind<PlayerContext>().AsSingle();
      Container.Bind<Board>().AsSingle();
      Container.Bind<AiHeap>().AsSingle();
      Container.Bind<AiContext>().AsSingle();
      
      // var decisionTreeLookup = new DecisionTreeLookup();
      //
      // var decisionTreeCreatorVisitor = new DecisionTreeCreatorVisitor(eventBus, 
      //   d => new LoggingDecorator(d, DebugController.Info, makeDecisionLog), decisionTreeLookup);
      //
      // var systemRandomEmbedded = new SystemRandomEmbedded(0);
      // var effectFactory = new EffectFactory(eventBus);
      // var abilityFactory = new AbilityFactory(abilitiesInfo, effectFactory, eventBus);
      // var decisionTreeFactory = new DecisionFactory(decisionTreeCreatorVisitor, decisionTreeComponent);
      // var unitFactory = new UnitFactory(unitsInfo, decisionTreeFactory, abilitiesInfo, abilityFactory, systemRandomEmbedded);
      //
      // //TODO: replace board/bench dictionaries with array?               
      // var playerContext = new PlayerContext(new Player(unitFactory), new Player(unitFactory));
      // var board = new Board();
      // var aiHeap = new AiHeap();
      // var aiContext = new AiContext(board, aiHeap);
      
      #endregion
      #region Shared
      
      Container.Bind<BattleSetupUI>().FromInstance(BattleSetupUI).AsSingle();
      Container.Bind<PlayerSharedContext>().AsSingle(); //to pass ui need to bind it first
      
      // var playerSharedContext = new PlayerSharedContext(playerContext, playerPresenterContext, BattleSetupUI);
      
      #endregion
      #region Unit selection
      
      Container.Bind<UnitTooltipUI>().FromInstance(UnitTooltipUI).AsSingle();
      Container.Bind<RaycastController>().AsSingle()
        .WithArguments(LayerMask.GetMask("Terrain", "GlobalCollider"), LayerMask.GetMask("Unit")); //custom ints for mask, why not mask instead?
      Container.Bind<UnitSelectionController>().AsSingle();
      Container.Bind<UnitTooltipController>().AsSingle();
      
      // var raycastController = new RaycastController(mainCamera, 
      //   LayerMask.GetMask("Terrain", "GlobalCollider"), LayerMask.GetMask("Unit"));
      //
      // var unitSelectionController = new UnitSelectionController(inputController, 
      //   raycastController, coordFinder);
      //
      // var unitTooltipController = new UnitTooltipController(UnitTooltipUI, 
      //   unitSelectionController);
      
      #endregion
      #region Unit drag
      
      Container.Bind<BattleSimulationUI>().FromInstance(BattleSimulationUI).AsSingle();
      Container.Bind<BattleStateController>().AsSingle(); //to pass ui need to bind it first
      Container.Bind<CoordFinderBySelectedPlayer>().AsSingle(); //to pass ui need to bind it first
      Container.Bind<IPredicate<UnitSelectedEvent>>().To<CanStartDrag>().AsSingle(); //to pass ui need to bind it first, not sure if that binding should work or not
      Container.Bind<UnitDragController>().AsSingle();
      Container.Bind<TileHighlighterController>().AsSingle();
      Container.Bind<UnitMoveController>().AsSingle();
      //
      //
      // var battleStateController = new BattleStateController(BattleSimulationUI);
      // var unitDragController = new UnitDragController(raycastController, 
      //   new CoordFinderBySelectedPlayer(coordFinder, BattleSetupUI), 
      //   inputController, unitSelectionController, 
      //   new CanStartDrag(battleStateController, BattleSetupUI));
      //
      // var tileHighlightController = new TileHighlighterController(tileSpawner, 
      //   unitDragController);
      //
      // var unitMoveController = new UnitMoveController(playerSharedContext, unitDragController);
      
      #endregion
      #region Battle simulation
      
      Container.Bind(typeof(ISimulationTick), typeof(IReset), typeof(IEventHandler<StartMoveEvent>), 
        typeof(IEventHandler<FinishMoveEvent>), typeof(IEventHandler<PauseMoveEvent>), 
        typeof(IEventHandler<ContinueMoveEvent>), typeof(IEventHandler<RotateEvent>))
        .To<MovementController>().AsSingle();
      Container.Bind(typeof(AttackController), typeof(IEventHandler<UpdateHealthEvent>), 
        typeof(IEventHandler<DeathEvent>), typeof(IEventHandler<UpdateManaEvent>)).To<AttackController>().AsSingle();
      Container.Bind(typeof(AnimationController), typeof(IEventHandler<StartMoveEvent>), 
        typeof(IEventHandler<IdleEvent>), typeof(IEventHandler<StartAttackEvent>), 
        typeof(IEventHandler<StartCastEvent>), typeof(IEventHandler<UpdateStunDurationEvent>), 
        typeof(IEventHandler<ContinueMoveEvent>)).To<AnimationController>().AsSingle();
      Container.Bind(typeof(ISimulationTick), typeof(IReset), typeof(IEventHandler<UpdateSilenceDurationEvent>))
        .To<SilenceController>().AsSingle();
      Container.Bind(typeof(ISimulationTick), typeof(IReset), typeof(IEventHandler<UpdateStunDurationEvent>))
        .To<StunController>().AsSingle();
      Container.Bind(typeof(ISimulationTick), typeof(IReset), typeof(IEventHandler<SpawnProjectileEvent>))
        .To<ProjectileController>().AsSingle();
      // Container.Bind<ISimulationTick>()
        // .To(typeof(MovementController), typeof(SilenceController), typeof(StunController), typeof(ProjectileController))
        // .AsTransient();
      Container.Bind<CompositeSimulationTick>().AsSingle();
      Container.Bind<CompositeReset>().AsSingle();
      Container.Bind<BattleSimulationPresenter>().AsSingle();
      Container.Bind<SynergyEffectApplier>().AsSingle();
      Container.Bind<BattleSimulation>().AsSingle();
      Container.Bind(typeof(RealtimeBattleSimulationController), typeof(ITick)).To<RealtimeBattleSimulationController>().AsSingle();
      //
      //
      // // new SystemRandomTests().ExecuteTests();
      // var movementController = new MovementController(boardPresenter, coordFinder);
      // var attackController = new AttackController(boardPresenter, unitTooltipController);
      // var animationController = new AnimationController(boardPresenter);
      // var silenceController = new SilenceController(boardPresenter);
      // var stunController = new StunController(boardPresenter);
      // var projectileController = new ProjectileController(boardPresenter, coordFinder);
      //
      // var compositeSimulationTick = new CompositeSimulationTick(movementController, silenceController, 
      //   stunController, projectileController);
      // var compositeReset = new CompositeReset(movementController, silenceController, stunController,
      //   projectileController);
      //
      // var battleSimulationPresenter = new BattleSimulationPresenter(coordFinder, 
      //   boardPresenter, compositeSimulationTick, compositeReset);
      //
      // var synergyEffectApplier = new SynergyEffectApplier(board, aiContext, synergiesInfo, effectsInfo, effectFactory);
      // var battleSimulation = new BattleSimulation(aiContext, board, aiHeap, systemRandomEmbedded, synergyEffectApplier);
      //
      // var realtimeBattleSimulationController = new RealtimeBattleSimulationController(
      //   compositeSimulationTick, battleSimulation);
      
      #endregion
      #region Debug
      
      Container.Bind<CommandDebugWindowUI>().FromInstance(CommandDebugWindowUI).AsSingle();
      Container.Bind<EventRow>().AsSingle().WithArguments(CommandDebugWindowUI.EventTemplate); //to pass ui need to bind it first
      Container.Bind<CommandButtonStyler>().AsSingle();
      Container.Bind<CommandRow>().AsSingle(); //to pass ui need to bind it first
      Container.Bind<CommandsDebugController>().AsSingle(); //to pass ui need to bind it first
      //need to bind into a list
      Container.Bind(typeof(IBattleTest)).To<SilenceTest>().AsSingle();
      Container.Bind(typeof(IBattleTest)).To<WithinRadiusTest>().AsSingle();
      Container.Bind(typeof(IBattleTest)).To<TauntTest>().AsSingle();
      Container.Bind(typeof(IBattleTest)).To<PeriodicStunTest>().AsSingle();
      Container.Bind(typeof(IBattleTest)).To<SecondStunDuringMovementShouldApplyDifferenceOnly>().AsSingle();
      Container.Bind(typeof(IBattleTest)).To<StunTest>().AsSingle();
      Container.Bind(typeof(IBattleTest)).To<NestedAbilityPeriodTest>().AsSingle();

      Container.Bind<BattleSaveUI>().FromInstance(BattleSaveUI).AsSingle();
      Container.Bind<ModelUI>().FromInstance(ModelUI).AsSingle();
      Container.Bind<BattleTestController>().AsSingle(); //to pass ui need to bind it first
      Container.Bind<BattleSimulationDebugController>().AsSingle(); //to pass ui need to bind it first
      Container.Bind<BattleSaveController>().AsSingle(); //to pass ui need to bind it first
      Container.Bind<BattleSetupController>().AsSingle(); //to pass ui need to bind it first
      Container.Bind(typeof(UnitModelDebugController), typeof(ITick)).To<UnitModelDebugController>().AsSingle(); //to pass ui need to bind it first
      Container.Bind(typeof(TakenCoordDebugController), typeof(ITick)).To<TakenCoordDebugController>().AsSingle(); //to pass ui need to bind it first
      Container.Bind(typeof(TargetDebugController), typeof(ITick)).To<TargetDebugController>().AsSingle(); //to pass ui need to bind it first
      Container.Bind(typeof(UIDebugController), typeof(ITick)).To<UIDebugController>().AsSingle(); //to pass ui need to bind it first

      Container.Bind<DebugController>().FromInstance(DebugController).AsSingle();

      //
      // var commandEvents = new EventRow(battleSimulation, eventBus, CommandDebugWindowUI.EventTemplate);
      // var commandButtonStyler = new CommandButtonStyler(boardPresenter);
      // var commandsHandler = new CommandRow(commandEvents, commandButtonStyler, CommandDebugWindowUI, aiContext, 
      //   battleSimulation);
      // var commandsDebugController = new CommandsDebugController(aiHeap, commandsHandler, CommandDebugWindowUI);
      //
      // var battleTests = new List<IBattleTest> {
      //   new SilenceTest(playerSharedContext),
      //   new WithinRadiusTest(playerSharedContext),
      //   new TauntTest(playerSharedContext),
      //   new PeriodicStunTest(playerSharedContext),
      //   new SecondStunDuringMovementShouldApplyDifferenceOnly(playerSharedContext),
      //   new StunTest(playerSharedContext),
      //   new NestedAbilityPeriodTest(playerSharedContext),
      // };
      // var battleTestController = new BattleTestController(BattleSimulationUI, battleTests);
      //
      // var battleSimulationDebugController = new BattleSimulationDebugController(battleSimulation, BattleSimulationUI, 
      //   aiContext, playerContext, playerPresenterContext, realtimeBattleSimulationController, battleSimulationPresenter,
      //   commandDebugUI, battleTestController);
      //
      // var battleSaveController = new BattleSaveController(playerSharedContext, 
      //   BattleSaveUI, saveDataLoader, saves,
      //   battleSimulationDebugController);
      //
      // var battleSetupController = new BattleSetupController(playerContext, 
      //   playerPresenterContext, BattleSetupUI);
      //
      // var unitModelDebugController = new UnitModelDebugController(playerContext, board, ModelUI, 
      //   DebugController.Info, unitSelectionController, aiHeap, coordFinder);
      //
      // var takenCoordDebugController = new TakenCoordDebugController(board, DebugController,
      //   tileSpawner);
      //
      // var targetDebugController = new TargetDebugController(
      //   board, coordFinder, DebugController.Info);
      //
      // var uiDebugController = new UIDebugController(
      //   BattleSetupUI, BattleSaveUI, BattleSimulationUI,
      //   unitModelDebugController, CommandDebugWindowUI);

      #endregion
    }
    
    static readonly Logger log = MainLog.GetLogger(nameof(CompositionRootZenject));
  }
}