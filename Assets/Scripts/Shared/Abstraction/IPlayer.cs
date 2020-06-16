namespace Shared.Abstraction {
  public interface IPlayer<TUnit> {
    EPlayer Player { get; }
    IUnitDict<TUnit> BoardUnits { get; }
    IUnitDict<TUnit> BenchUnits { get; }
    void MoveUnit(Coord from, Coord to);
  }
}