using Model.NAI.Visitors;
using Model.NBattleSimulation;
using Model.NDecisionTree;
using Model.NUnit;

namespace Model.NAI.UnitCommands {
  public class MakeDecisionCommand : ICommand {
    public IDecisionTreeNode Decision { get; private set; }
    
    public MakeDecisionCommand(CAi ai, AiContext context) {
      this.ai = ai;
      this.context = context;
    }

    public void Execute() => Decision = ai.MakeDecision(context);
    public void Accept(ICommandVisitor visitor) => visitor.VisitMakeDecisionCommand(this);

    readonly CAi ai;
    readonly AiContext context;
  }
}