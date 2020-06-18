namespace Model.NUnit {
  public class CTarget {
    public Unit Unit;

    public CTarget(CMovement movement) => this.movement = movement;

    public bool Exists => Unit != null;
    public void Reset() => Unit = null;
    
    public void Clear() {
      if (!Exists) return;
      
      Unit.Health.UnsubFromDeath(this);
      Unit = null;
    }

    public void ChangeTo(Unit unit) {
      Clear();
      Unit = unit;
      Unit.Health.SubToDeath(this);
    }

    public static implicit operator Unit(CTarget target) => target.Unit;

    public override string ToString() => Exists ? $"Target coord: {Unit.Movement.Coord}" : "";

    readonly CMovement movement;
  }
}