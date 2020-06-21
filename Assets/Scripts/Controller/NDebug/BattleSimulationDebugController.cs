using Controller.Exts;
using Controller.NBattleSimulation;
using Model.NBattleSimulation;
using UniRx;
using View.Presenters;
using View.UIs;

namespace Controller.NDebug {
  public class BattleSimulationDebugController {
    public BattleSimulationDebugController(BattleSimulation simulation, BattleSimulationUI ui, 
        AiContext context, PlayerContext playerContext, 
        PlayerPresenterContext playerPresenterContext,
        RealtimeBattleSimulationController realtimeBattleSimulationController, 
        BattleSimulationPresenter simulationPresenter) {
      this.simulation = simulation;
      this.ui = ui;
      this.context = context;
      this.playerContext = playerContext;
      this.playerPresenterContext = playerPresenterContext;
      this.realtimeBattleSimulationController = realtimeBattleSimulationController;
      this.simulationPresenter = simulationPresenter;

      ui.OStart.OnValueChangedAsObservable().Where(b => b)
        .Subscribe(StartBattle).AddTo(ui.OStart);
      ui.OPause.OnValueChangedAsObservable()
        .Subscribe(SetPaused).AddTo(ui.OPause);
      ui.SSpeed.OnValueChangedAsObservable()
        .Subscribe(SetSpeed).AddTo(ui.SSpeed);
      ui.BExecuteNextDecision.Sub(ExecuteNextCommand);
      ui.BExecuteAllDecisions.Sub(ExecuteAllCommands);
      ui.BExecuteInRealtime.Sub(PlayBattleInRealtime);
    }
    
    void StartBattle() {
      realtimeBattleSimulationController.StopBattle();
      simulation.PrepareBattle(playerContext);
      simulationPresenter.Reset(playerPresenterContext);
      ui.SetEnabled(!simulation.IsBattleOver);
    }

    void ExecuteNextCommand() {
      //TODO: log command here
      //TODO: pass debug controller to be able disable logging
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

    readonly BattleSimulation simulation;
    readonly BattleSimulationPresenter simulationPresenter;
    readonly BattleSimulationUI ui;
    readonly AiContext context;
    readonly PlayerContext playerContext;
    readonly PlayerPresenterContext playerPresenterContext;
    readonly RealtimeBattleSimulationController realtimeBattleSimulationController;
  }
}