namespace Model.NUnit {
  public class CTarget {
    public Unit Unit;

    public CTarget(CMovement movement) {
      this.movement = movement;
    }
    
    public bool Exists => Unit != null;
    public void OnDeath() => Unit = null;
    
    public void Clear() {
      Unit.Health.UnsubFromDeath(this);
      Unit = null;
    }

    public override string ToString() => Exists ? $"Target coord: {Unit.Movement.Coord}" : "";

    readonly CMovement movement;
  }
}