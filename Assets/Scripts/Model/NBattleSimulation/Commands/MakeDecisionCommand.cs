using Model.NAI.NDecisionTree;
using Model.NUnit;

namespace Model.NBattleSimulation.Commands {
  public class MakeDecisionCommand : BaseCommand {
    public IDecisionTreeNode Decision { get; private set; }
    
    public MakeDecisionCommand(CAi ai, AiContext context, float time) {
      this.ai = ai;
      this.context = context;
      ai.NextDecisionTime = time;
    }

    public override void Execute() => Decision = ai.MakeDecision(context);

    readonly CAi ai;
    readonly AiContext context;
  }
}