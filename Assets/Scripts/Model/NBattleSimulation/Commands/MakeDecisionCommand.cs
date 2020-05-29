using Model.NDecisionTree;
using Model.NUnit;

namespace Model.NBattleSimulation.Commands {
  public class MakeDecisionCommand : ICommand {
    public IDecisionTreeNode Decision { get; private set; }
    
    public MakeDecisionCommand(CAi ai, AiContext context) {
      this.ai = ai;
      this.context = context;
    }

    public void Execute() => Decision = ai.MakeDecision(context);

    readonly CAi ai;
    readonly AiContext context;
  }
}