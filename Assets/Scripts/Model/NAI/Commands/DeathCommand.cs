using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Primitives;

namespace Model.NAI.Commands {
  public class DeathCommand : BaseCommand {
    public DeathCommand(IUnit target, AiContext context) : base(target) {
      this.target = target;
      this.context = context;
    }
    
    public override void Execute() {
      //cancel actions of dead unit?
      //end move immediately if it had start move
      //cancel attack
      
      //notify units which had dead unit as target
      //so that they can remove it and start looking for a new target
      //so that they don't wait for target to end move
      //so that they don't wait to attack target
      context.RemoveUnit(target.Coord);
      
      if (target.TakenCoord != Coord.Invalid)
        context.RemoveUnit(target.TakenCoord);
      
      context.CheckBattleIsOver();
    }

    readonly IUnit target;
    readonly AiContext context;
  }
}