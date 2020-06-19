namespace Model.NUnit.Abstraction {
  public interface ITarget {
    Unit Target { get; }
    bool TargetExists { get; }
    void ClearTarget();
    void ChangeTargetTo(Unit unit);
  }
}