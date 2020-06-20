using Controller.Exts;
using Model.NBattleSimulation;
using UniRx;
using View.Presenters;
using View.UIs;

namespace Controller.NDebug {
  public class BattleSimulationDebugController {
    public BattleSimulationDebugController(BattleSimulation simulation, BattleSimulationUI ui, 
        AiContext context, PlayerContext playerContext, PlayerPresenter[] playerPresenters,
        RealtimeBattleSimulationController realtimeBattleSimulationController, 
        BattleSimulationPresenter simulationPresenter) {
      this.simulation = simulation;
      this.ui = ui;
      this.context = context;
      this.playerContext = playerContext;
      this.playerPresenters = playerPresenters;
      this.realtimeBattleSimulationController = realtimeBattleSimulationController;
      this.simulationPresenter = simulationPresenter;

      ui.OStart.OnValueChangedAsObservable().Where(b => b)
        .Subscribe(StartBattle).AddTo(ui.OStart);
      ui.OPause.OnValueChangedAsObservable()
        .Subscribe(SetPaused).AddTo(ui.OStart);
      ui.SSpeed.OnValueChangedAsObservable()
        .Subscribe(SetSpeed).AddTo(ui.SSpeed);
      ui.BExecuteNextDecision.Sub(ExecuteNextCommand);
      ui.BExecuteAllDecisions.Sub(ExecuteAllCommands);
      ui.BExecuteInRealtime.Sub(PlayBattleInRealtime);
    }
    
    void StartBattle() {
      realtimeBattleSimulationController.StopBattle();
      simulation.PrepareBattle(playerContext);
      simulationPresenter.Reset(playerPresenters);
      ui.SetEnabled(!simulation.IsBattleOver);
    }

    void ExecuteNextCommand() {
      simulation.ExecuteNextCommand();
      simulationPresenter.SimulationTick(context.CurrentTime.Float);

      if (!simulation.IsBattleOver) return;
      
      ui.Disable();
    }

    void ExecuteAllCommands() {
      simulation.ExecuteAllCommands();
      ui.Disable();
    }
    
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
    readonly PlayerPresenter[] playerPresenters;
    readonly RealtimeBattleSimulationController realtimeBattleSimulationController;
  }
}