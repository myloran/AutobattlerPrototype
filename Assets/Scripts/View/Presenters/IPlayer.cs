using Shared;

namespace View.Presenters {
  public interface IPlayer<TUnit> {
    IUnitDict<TUnit> BoardUnits { get; }
    IUnitDict<TUnit> BenchUnits { get; }
    void MoveUnit(Coord from, Coord to);
  }
}