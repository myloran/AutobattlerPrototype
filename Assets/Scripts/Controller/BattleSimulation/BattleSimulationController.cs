using Model.NBattleSimulation;
using Shared.Shared.Client;
using UniRx;
using View;

namespace Controller.BattleSimulation {
  public class BattleSimulationController {
    public bool IsBattleStarted;

    public BattleSimulationController(Model.NBattleSimulation.BattleSimulation simulation, BattleSimulationUI ui,
        ISimulationTick viewSimulation, AiContext context) {
      this.simulation = simulation;
      this.viewSimulation = viewSimulation;
      this.context = context;
      ui.BPrepareBattle.Sub(StartBattle);
      ui.OStartBattle.onValueChanged.AsObservable().Subscribe(StartB).AddTo(ui.OStartBattle);
      ui.BExecuteNextDecision.Sub(ExecuteNextDecision);
    }

    void StartB(bool isOn) {
      simulation.PrepareBattle();
      IsBattleStarted = isOn;
    }

    void StartBattle() => simulation.PrepareBattle();

    void ExecuteNextDecision() {
      simulation.ExecuteNextDecision();
      // viewSimulation.Update(context.CurrentTime);
    }
    
    readonly Model.NBattleSimulation.BattleSimulation simulation;
    readonly ISimulationTick viewSimulation;
    readonly AiContext context;
  }
}