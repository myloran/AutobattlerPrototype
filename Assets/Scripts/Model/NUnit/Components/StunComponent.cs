using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Const;

namespace Model.NUnit.Components {
  public class StunComponent : IStun {
    public F32 StunEndTime { get; private set; }
    
    public StunComponent(IAttack attack, IAbility ability, AiComponent ai, MovementComponent movement) {
      this.attack = attack;
      this.ability = ability;
      this.ai = ai;
      this.movement = movement;
    }
  
    public void ApplyStun(F32 currentTime, F32 duration) {
      var stunEndTime = Max(StunEndTime, currentTime + duration);
      if (stunEndTime != StunEndTime) {
        StunEndTime = stunEndTime;
        attack.EndAttack(); //TODO: do it only if stun end time is more then before
        ability.EndCasting(); //TODO: do it only if stun end time is more then before
        movement.TryPauseMovement(currentTime);
        ai.UpdateNextDecisionExecutionTime(duration);
      }
    }

    public void Reset() => StunEndTime = -MaxBattleDuration;

    public override string ToString() => $"{nameof(StunEndTime)}: {StunEndTime}";

    readonly IAttack attack;
    readonly IAbility ability;
    readonly AiComponent ai;
    readonly MovementComponent movement;
  }
}