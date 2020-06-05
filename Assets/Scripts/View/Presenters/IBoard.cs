using System.Collections.Generic;
using Shared;

namespace View.Presenters {
  public interface IBoard<TUnit, in TPlayer> 
      where TPlayer : IPlayer<TUnit> where TUnit : IUnit {
    bool ContainsUnitAt(Coord coord);
    TUnit GetUnitAt(Coord coord);
    IEnumerable<TUnit> Units { get; }
    void AddUnitAt(Coord coord, TUnit unit);
    void RemoveUnitAt(Coord coord);
    void MoveUnitOnBoard(Coord from, Coord to);
    void Reset(TPlayer player1, TPlayer player2);
  }
}