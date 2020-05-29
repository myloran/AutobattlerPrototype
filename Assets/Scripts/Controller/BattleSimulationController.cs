using Model.NAI.Visitors;
using Model.NBattleSimulation;
using View;

namespace Controller {
  public class BattleSimulationController {
    public BattleSimulationController(BattleSimulation simulation, ICommandVisitor visitor,
        BattleSimulationUI ui) {
      this.simulation = simulation;
      this.visitor = visitor;
      ui.BPrepareBattle.Sub(StartBattle);
      ui.BExecuteNextDecision.Sub(ExecuteNextDecision);
    }
    
    void StartBattle() => simulation.PrepareBattle();

    void ExecuteNextDecision() {
      simulation.ExecuteNextDecision();
      simulation.Command.Accept(visitor);
    }
    
    readonly BattleSimulation simulation;
    readonly ICommandVisitor visitor;
  }
}