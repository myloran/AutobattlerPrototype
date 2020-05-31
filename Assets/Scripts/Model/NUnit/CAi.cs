using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NUnit {
  public class CAi {
    public IDecisionTreeNode Decision;

    public CAi(CAttack attack, CHealth health) {
      this.attack = attack;
      this.health = health;
    }

    public IDecisionTreeNode MakeDecision(AiContext context) => Decision.MakeDecision(context);

    public bool CanExecuteDecision => health.IsAlive;

    public TimePoint NextDecisionTime => attack.AttackTime;
    
    readonly CAttack attack;
    readonly CHealth health;
  }
}