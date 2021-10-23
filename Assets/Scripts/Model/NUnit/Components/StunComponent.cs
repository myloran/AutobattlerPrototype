using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Const;

namespace Model.NUnit.Components {
  public class StunComponent : IStun {
    public F32 StunEndTime { get; private set; }
    
    public StunComponent(IAttack attack, IAbility ability, AiComponent ai) {
      this.attack = attack;
      this.ability = ability;
      this.ai = ai;
    }
  
    public void ApplyStun(F32 endTime) {
      var stunEndTime = Max(StunEndTime, endTime);
      if (stunEndTime != StunEndTime) {
        StunEndTime = stunEndTime;
        attack.EndAttack(); //TODO: do it only if stun end time is more then before
        ability.EndCasting(); //TODO: do it only if stun end time is more then before
        ai.UpdateNextDecisionExecutionTime(StunEndTime);
        //split move action to start move and finish move
        //add decision HasMoveStarted
          //if true => WasInterrupted
            //if true => delay it
            //if false => finish
          //if false => continue with HasTarget check
        //Display stunned effect
      }
    }

    public void Reset() => StunEndTime = -MaxBattleDuration;

    public override string ToString() => $"{nameof(StunEndTime)}: {StunEndTime}";

    readonly IAttack attack;
    readonly IAbility ability;
    readonly AiComponent ai;
  }
}