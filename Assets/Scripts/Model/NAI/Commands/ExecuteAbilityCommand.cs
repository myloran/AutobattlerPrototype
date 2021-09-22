using Model.NAbility;
using Model.NBattleSimulation;

namespace Model.NAI.Commands {
  public class ExecuteAbilityCommand : BaseCommand {
    public ExecuteAbilityCommand(Ability ability, AiContext context) : base(ability.Unit) {
      this.ability = ability;
      this.context = context;
    }
    
    public override void Execute() => ability.Execute(context);

    readonly AiContext context;
    readonly Ability ability;
  }
}