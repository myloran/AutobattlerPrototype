using System;
using Controller.Exts;
using Controller.NBattleSimulation;
using Model.NBattleSimulation;
using UniRx;
using UnityEngine.UIElements;
using View.Presenters;
using View.UIs;

namespace Controller.NDebug {
  public class BattleSimulationDebugController : IDisposable {
    public BattleSimulationDebugController(BattleSimulation simulation, BattleSimulationUI ui,
      AiContext context, PlayerContext playerContext,
      PlayerPresenterContext playerPresenterContext,
      RealtimeBattleSimulationController realtimeBattleSimulationController,
      BattleSimulationPresenter simulationPresenter, CommandDebugUI commandDebugUI) {
      this.simulation = simulation;
      this.ui = ui;
      this.context = context;
      this.playerContext = playerContext;
      this.playerPresenterContext = playerPresenterContext;
      this.realtimeBattleSimulationController = realtimeBattleSimulationController;
      this.simulationPresenter = simulationPresenter;
      this.commandDebugUI = commandDebugUI;
    }

    public void SubToUI(CompositeDisposable disposable) {
      BattleControlSubs();
      BattleSimulationSubs();
    //extract to Timeline
      commandDebugUI.BGenerateTimeline.clicked += GenerateTimeline;
      commandDebugUI.STimeline.RegisterCallback<ChangeEvent<int>>(OnTimelineChange);
      disposable.Add(this);
    }

    public void Dispose() {
      commandDebugUI.STimeline.UnregisterCallback<ChangeEvent<int>>(OnTimelineChange);
      commandDebugUI.BGenerateTimeline.clicked -= GenerateTimeline;
    }
    
    void GenerateTimeline() {
      ResetBattle();
      commandCount = 0;

      while (!simulation.IsBattleOver) {
        ExecuteNextCommand();
        commandCount++;
      }

      commandDebugUI.STimeline.highValue = commandCount;
    }

    void OnTimelineChange(ChangeEvent<int> evt) {
      ResetBattle();

      var commandsLeft = evt.newValue;
      while (commandsLeft-- > 0 && !simulation.IsBattleOver) {
        ExecuteNextCommand();
        commandCount++;
      }
    }
    //extract to Timeline

    void BattleSimulationSubs() {
      ui.BExecuteNextDecision.Sub(ExecuteNextCommand);
      ui.BExecuteAllDecisions.Sub(ExecuteAllCommands);
      ui.BExecuteInRealtime.Sub(PlayBattleInRealtime);
    }

    void BattleControlSubs() {
      ui.OStart.OnValueChangedAsObservable().Subscribe(ResetBattle).AddTo(ui.OStart);
      ui.OPause.OnValueChangedAsObservable().Subscribe(SetPaused).AddTo(ui.OPause);
      ui.SSpeed.OnValueChangedAsObservable().Subscribe(SetSpeed).AddTo(ui.SSpeed);
    }

    public void ResetBattle() {
      realtimeBattleSimulationController.StopBattle();
      simulation.PrepareBattle(playerContext);
      simulationPresenter.Reset(playerPresenterContext);
      ui.SetEnabled(!simulation.IsBattleOver);
    }
                       
    void ExecuteNextCommand() {
      //TODO: log command here and pass debug controller to be able disable logging
      simulation.ExecuteNextCommand();
      simulationPresenter.SimulationTick(context.CurrentTime.Float);
      if (!simulation.IsBattleOver) return;
      
      ui.Disable();
    }

    void ExecuteAllCommands() {
      simulation.ExecuteAllCommands();
      ui.Disable();
    }
    
    //TODO: extract realtime simulation
    void SetPaused(bool isPaused) => realtimeBattleSimulationController.SetPaused(isPaused);
    void SetSpeed(float speed) => realtimeBattleSimulationController.SetSpeed(speed);
    
    void PlayBattleInRealtime() {
      realtimeBattleSimulationController.StartBattle();
      ui.Disable();
    }

    int commandCount;
    readonly BattleSimulation simulation;
    readonly BattleSimulationUI2 ui2;
    readonly BattleSimulationPresenter simulationPresenter;
    readonly CommandDebugUI commandDebugUI;
    readonly BattleSimulationUI ui;
    readonly AiContext context;
    readonly PlayerContext playerContext;
    readonly PlayerPresenterContext playerPresenterContext;
    readonly RealtimeBattleSimulationController realtimeBattleSimulationController;
  }
}