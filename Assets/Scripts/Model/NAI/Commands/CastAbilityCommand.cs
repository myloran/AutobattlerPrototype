using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.Commands {
  public class CastAbilityCommand : BaseCommand {
    public CastAbilityCommand(IUnit unit, AiContext context) {
      this.unit = unit;
      this.context = context;
    }

    public override void Execute() {
      if (!unit.TargetExists) return;
      
      var target = unit.Target;
      if (!target.IsAlive) return;

      unit.CastAbility(context);
    }

    readonly IUnit unit;
    readonly AiContext context;
  }
}