using System.Linq;
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
using Model.NSynergy;
using PlasticFloor.EventBus;
using Shared.Abstraction;
using Shared.Primitives;
using Shared.Shared.Client.Events;
using UniRx;
using UnityEngine;
using View.NTile;
using View.UIs;
using View.UIToolkit;
using Zenject;

namespace Infrastructure.ZenjectTest {
  public class InitializationFlow : MonoBehaviour, IInitializable {
    [Inject]
    void Construct(InfoLoader<UnitInfo> unitInfoLoader, InfoLoader<AbilityInfo> abilityInfoLoader, 
        InfoLoader<SynergyInfo> synergyInfoLoader, InfoLoader<EffectInfo> effectInfoLoader, 
        TickController tickController, InputController inputController, ITick[] ticks,
        CommandDebugUI commandDebugUI, CommandDebugWindowUI commandDebugWindowUI, BattleSetupUI battleSetupUI,
        BattleSaveUI battleSaveUI, IInfoGetter<UnitInfo> unitInfoGetter, BattleSaveGetter battleSaveGetter,
        TilePresenter tilePresenter, SynergyEffectApplier synergyEffectApplier, 
        DecisionTreeCreatorVisitor decisionTreeCreatorVisitor, EventBus eventBus, 
        IEventHandler<StartMoveEvent>[] startMoveEventHandlers, IEventHandler<FinishMoveEvent> finishMoveEventHandler,
        IEventHandler<PauseMoveEvent> pauseMoveEventHandler, 
        IEventHandler<ContinueMoveEvent>[] continueMoveEventHandlers, IEventHandler<RotateEvent> rotateEventHandler,
        IEventHandler<UpdateHealthEvent> updateHealthEventHandler, IEventHandler<DeathEvent> deathEventHandler,
        IEventHandler<UpdateManaEvent> updateManaEventHandler, IEventHandler<IdleEvent> idleEventHandler,
        IEventHandler<StartAttackEvent> startAttackEventHandler, IEventHandler<StartCastEvent> startCastEventHandler,
        IEventHandler<UpdateSilenceDurationEvent> updateSilenceDurationEventHandler,
        IEventHandler<UpdateStunDurationEvent>[] updateStunDurationEventHandlers, 
        IEventHandler<SpawnProjectileEvent> spawnProjectileEventHandler, 
        UnitSelectionController unitSelectionController, BattleStateController battleStateController, 
        UnitDragController unitDragController, TileHighlighterController tileHighlightController,
        UnitMoveController unitMoveController, UnitTooltipController unitTooltipController,
        BattleSaveController battleSaveController, BattleSetupController battleSetupController,
        UnitModelDebugController unitModelDebugController, BattleTestController battleTestController,
        BattleSimulationDebugController battleSimulationDebugController, EventRow eventRow,
        CommandRow commandRow, CommandsDebugController commandsDebugController, DebugController debugController, 
        UnitTooltipUI unitTooltipUI, MonoBehaviourCallBackController monoBehaviourCallBackController) {
      this.ticks = ticks;
      this.monoBehaviourCallBackController = monoBehaviourCallBackController;
      this.debugController = debugController;
      this.unitTooltipUI = unitTooltipUI;
      this.commandsDebugController = commandsDebugController;
      this.commandRow = commandRow;
      this.eventRow = eventRow;
      this.battleSimulationDebugController = battleSimulationDebugController;
      this.battleTestController = battleTestController;
      this.unitModelDebugController = unitModelDebugController;
      this.battleSetupController = battleSetupController;
      this.battleSaveController = battleSaveController;
      this.unitTooltipController = unitTooltipController;
      this.unitMoveController = unitMoveController;
      this.tileHighlightController = tileHighlightController;
      this.unitDragController = unitDragController;
      this.battleStateController = battleStateController;
      this.unitSelectionController = unitSelectionController;
      this.spawnProjectileEventHandler = spawnProjectileEventHandler;
      this.updateStunDurationEventHandlers = updateStunDurationEventHandlers;
      this.updateSilenceDurationEventHandler = updateSilenceDurationEventHandler;
      this.startCastEventHandler = startCastEventHandler;
      this.startAttackEventHandler = startAttackEventHandler;
      this.idleEventHandler = idleEventHandler;
      this.updateManaEventHandler = updateManaEventHandler;
      this.deathEventHandler = deathEventHandler;
      this.updateHealthEventHandler = updateHealthEventHandler;
      this.rotateEventHandler = rotateEventHandler;
      this.continueMoveEventHandlers = continueMoveEventHandlers;
      this.pauseMoveEventHandler = pauseMoveEventHandler;
      this.finishMoveEventHandler = finishMoveEventHandler;
      this.startMoveEventHandlers = startMoveEventHandlers;
      this.eventBus = eventBus;
      this.decisionTreeCreatorVisitor = decisionTreeCreatorVisitor;
      this.synergyEffectApplier = synergyEffectApplier;
      this.tilePresenter = tilePresenter;
      this.battleSaveGetter = battleSaveGetter;
      this.unitInfoGetter = unitInfoGetter;
      this.battleSaveUI = battleSaveUI;
      this.battleSetupUI = battleSetupUI;
      this.commandDebugWindowUI = commandDebugWindowUI;
      this.commandDebugUI = commandDebugUI;
      this.inputController = inputController;
      this.tickController = tickController;
      this.effectInfoLoader = effectInfoLoader;
      this.synergyInfoLoader = synergyInfoLoader;
      this.abilityInfoLoader = abilityInfoLoader;
      this.unitInfoLoader = unitInfoLoader;
    }
    
    public void Initialize() {
      unitInfoLoader.Load("Units");
      abilityInfoLoader.Load("Abilities");
      synergyInfoLoader.Load("Synergies");
      effectInfoLoader.Load("Effects");
      
      #region Infrastructure
      
      tickController.InitObservable(ticks); //TODO: register implicitly?
      inputController.InitObservables();
      
      #endregion
      #region View
      
      commandDebugUI.FillReferences(commandDebugWindowUI.Document);
      
      battleSetupUI.SetDropdownOptions(unitInfoGetter.Infos.Keys.ToList());
      battleSaveUI.SubToUI(battleSaveGetter.Saves.Keys.ToList());
      tilePresenter.SpawnTiles();
      #endregion
      
      #region Model
      
      synergyEffectApplier.Init();
      decisionTreeCreatorVisitor.Init();
      eventBus.Register<StartMoveEvent>(startMoveEventHandlers); //TODO: register implicitly?
      eventBus.Register<FinishMoveEvent>(finishMoveEventHandler);
      eventBus.Register<RotateEvent>(rotateEventHandler);
      eventBus.Register<PauseMoveEvent>(pauseMoveEventHandler);
      eventBus.Register<ContinueMoveEvent>(continueMoveEventHandlers);
      eventBus.Register<UpdateHealthEvent>(updateHealthEventHandler);
      eventBus.Register<UpdateManaEvent>(updateManaEventHandler);
      eventBus.Register<DeathEvent>(deathEventHandler);
      eventBus.Register<IdleEvent>(idleEventHandler);
      eventBus.Register<StartAttackEvent>(startAttackEventHandler);
      eventBus.Register<StartCastEvent>(startCastEventHandler);
      eventBus.Register<UpdateSilenceDurationEvent>(updateSilenceDurationEventHandler);
      eventBus.Register<UpdateStunDurationEvent>(updateStunDurationEventHandlers);
      eventBus.Register<SpawnProjectileEvent>(spawnProjectileEventHandler);
      
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
      battleTestController.SubToUI();
      battleSimulationDebugController.SubToUI(disposable);
      eventRow.Init(disposable);
      commandRow.Init(disposable);
      commandsDebugController.Init();
      debugController.Init(unitTooltipUI);
      
      #endregion
      
      #endregion
      
      monoBehaviourCallBackController.StartUpdating(tickController);
    }
    
    InfoLoader<UnitInfo> unitInfoLoader;
    InfoLoader<AbilityInfo> abilityInfoLoader;
    InfoLoader<SynergyInfo> synergyInfoLoader;
    InfoLoader<EffectInfo> effectInfoLoader;
    TickController tickController;
    InputController inputController;
    CommandDebugUI commandDebugUI;
    CommandDebugWindowUI commandDebugWindowUI;
    BattleSetupUI battleSetupUI;
    BattleSaveUI battleSaveUI;
    IInfoGetter<UnitInfo> unitInfoGetter;
    BattleSaveGetter battleSaveGetter;
    TilePresenter tilePresenter;
    SynergyEffectApplier synergyEffectApplier;
    DecisionTreeCreatorVisitor decisionTreeCreatorVisitor;
    EventBus eventBus;
    IEventHandler<StartMoveEvent>[] startMoveEventHandlers;
    IEventHandler<FinishMoveEvent> finishMoveEventHandler;
    IEventHandler<PauseMoveEvent> pauseMoveEventHandler;
    IEventHandler<ContinueMoveEvent>[] continueMoveEventHandlers;
    IEventHandler<RotateEvent> rotateEventHandler;
    IEventHandler<UpdateHealthEvent> updateHealthEventHandler;
    IEventHandler<DeathEvent> deathEventHandler;
    IEventHandler<UpdateManaEvent> updateManaEventHandler;
    IEventHandler<IdleEvent> idleEventHandler;
    IEventHandler<StartAttackEvent> startAttackEventHandler;
    IEventHandler<StartCastEvent> startCastEventHandler;
    IEventHandler<UpdateSilenceDurationEvent> updateSilenceDurationEventHandler;
    IEventHandler<UpdateStunDurationEvent>[] updateStunDurationEventHandlers;
    IEventHandler<SpawnProjectileEvent> spawnProjectileEventHandler;
    readonly CompositeDisposable disposable = new CompositeDisposable();
    UnitSelectionController unitSelectionController;
    BattleStateController battleStateController;
    UnitDragController unitDragController;
    TileHighlighterController tileHighlightController;
    UnitMoveController unitMoveController;
    UnitTooltipController unitTooltipController;
    BattleSaveController battleSaveController;
    BattleSetupController battleSetupController;
    UnitModelDebugController unitModelDebugController;
    BattleTestController battleTestController;
    BattleSimulationDebugController battleSimulationDebugController;
    EventRow eventRow;
    CommandRow commandRow;
    CommandsDebugController commandsDebugController;
    UnitTooltipUI unitTooltipUI;
    DebugController debugController;
    MonoBehaviourCallBackController monoBehaviourCallBackController;
    ITick[] ticks;
  }
}