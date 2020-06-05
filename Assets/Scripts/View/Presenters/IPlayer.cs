using Shared;

namespace View.Presenters {
  public interface IPlayer<TUnit> {
    IUnitDict<TUnit> BoardUnits { get; }
    IUnitDict<TUnit> BenchUnits { get; }
    void MoveUnitOnBoard(Coord from, Coord to);
    void MoveUnit(Coord from, Coord to);
  }
}