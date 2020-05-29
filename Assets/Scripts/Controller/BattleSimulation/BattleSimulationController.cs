using Model.NBattleSimulation;
using Shared.Shared.Client;
using View;

namespace Controller.BattleSimulation {
  public class BattleSimulationController {
    public BattleSimulationController(Model.NBattleSimulation.BattleSimulation simulation, BattleSimulationUI ui,
        ITick viewSimulation, AiContext context) {
      this.simulation = simulation;
      this.viewSimulation = viewSimulation;
      this.context = context;
      ui.BPrepareBattle.Sub(StartBattle);
      ui.BExecuteNextDecision.Sub(ExecuteNextDecision);
    }
    
    void StartBattle() => simulation.PrepareBattle();

    void ExecuteNextDecision() {
      viewSimulation.Update(context.CurrentTime);
      simulation.ExecuteNextDecision();
    }
    
    readonly Model.NBattleSimulation.BattleSimulation simulation;
    readonly ITick viewSimulation;
    readonly AiContext context;
  }
}