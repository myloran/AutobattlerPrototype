using System.Collections.Generic;

namespace Shared.Abstraction {
  public interface IBoard<TUnit, in TPlayer> 
      where TPlayer : IPlayer<TUnit> where TUnit : IUnit {
    TUnit this[Coord coord] { get; }
    bool ContainsUnitAt(Coord coord);
    IEnumerable<TUnit> Values { get; }
    void AddUnit(Coord coord, TUnit unit);
    void RemoveUnit(Coord coord);
    void MoveUnit(Coord from, Coord to);
    void Reset(TPlayer player1, TPlayer player2);
  }
}