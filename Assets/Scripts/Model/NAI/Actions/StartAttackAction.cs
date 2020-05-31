using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;

namespace Model.NAI.Actions {
  public class StartAttackAction : IDecisionTreeNode {
    public StartAttackAction(CAttack attack, CAi ai) {
      this.attack = attack;
      this.ai = ai;
    }
    
    public IDecisionTreeNode MakeDecision(AiContext context) {
      attack.StartAttack(context.CurrentTime);
      var decisionCommand = new MakeDecisionCommand(ai, context);
      context.AiHeap[context.CurrentTime + attack.AnimationSpeed] = decisionCommand;
      //if health == 0 execute unit death command
      return this;
    }
    
    readonly CAttack attack;
    readonly CAi ai;
  }
}