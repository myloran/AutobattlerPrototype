using Model.NUnit.Abstraction;

namespace Model.NUnit {
  public class CTarget : ITarget {
    public Unit Target { get; private set; }

    public CTarget(CMovement movement) => this.movement = movement;

    public bool TargetExists => Target != null;
    public void Reset() => Target = null;
    
    public void ClearTarget() {
      if (!TargetExists) return;
      
      Target.UnsubFromDeath(this);
      Target = null;
    }

    public void ChangeTargetTo(Unit unit) {
      ClearTarget();
      Target = unit;
      Target.SubToDeath(this);
    }

    public static implicit operator Unit(CTarget target) => target.Target;

    public override string ToString() => TargetExists ? $"Target coord: {Target.Movement.Coord}" : "";

    readonly CMovement movement;
  }
}