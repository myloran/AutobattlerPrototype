using Model.NBattleSimulation;
using View;

namespace Controller {
  public class BattleSimulationController {
    public BattleSimulationController(BattleSimulation simulation, BattleSimulationUI ui) {
      this.simulation = simulation;
      ui.BPrepareBattle.Sub(StartBattle);
      ui.BExecuteNextDecision.Sub(ExecuteNextDecision);
    }
    
    void StartBattle() => simulation.PrepareBattle();

    void ExecuteNextDecision() {
      simulation.ExecuteNextDecision();
      // simulation.Command.Accept(visitor);
    }
    
    readonly BattleSimulation simulation;
  }
}