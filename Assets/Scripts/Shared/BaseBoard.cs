using System.Collections.Generic;

namespace Shared {
  public abstract class BaseBoard<TUnit, TPlayer> : IBoard<TUnit, TPlayer> 
      where TPlayer : IPlayer<TUnit> where TUnit : IUnit {
    protected BaseBoard(IUnitDict<TUnit> units, IUnitDict<TUnit> player1Units, 
        IUnitDict<TUnit> player2Units) {
      this.units = units;
      this.player1Units = player1Units;
      this.player2Units = player2Units;
    }
    
    public bool ContainsUnitAt(Coord coord) => units.Has(coord); 
    public TUnit GetUnitAt(Coord coord) => units[coord];
    public IEnumerable<TUnit> Units => units.Values;

    public void AddUnit(Coord coord, TUnit unit) {
      units[coord] = unit;

      if (unit.Player == EPlayer.First)
        player1Units[coord] = unit;
      else
        player2Units[coord] = unit;
    }

    public void RemoveUnit(Coord coord) {
      units.Remove(coord);
      
      if (player1Units.Has(coord))
        player1Units.Remove(coord);
      else 
        player2Units.Remove(coord);
    }

    public void MoveUnit(Coord from, Coord to) {
      var unit = GetUnitAt(from);
      OnChangeCoord(to, unit);
      AddUnit(to, unit);
      RemoveUnit(from);
    }
    
    public void Reset(TPlayer player1, TPlayer player2) {
      ClearUnits();
      
      foreach (var (coord, unit) in player1.BoardUnits) {
        units[coord] = unit;
        player1Units[coord] = unit;
      }
      
      foreach (var (coord, unit) in player2.BoardUnits) {
        units[coord] = unit;
        player2Units[coord] = unit;
      }
    }

    void ClearUnits() {
      units.Clear();
      player1Units.Clear();
      player2Units.Clear();
    }
    
    readonly IUnitDict<TUnit> units;
    readonly IUnitDict<TUnit> player1Units;
    readonly IUnitDict<TUnit> player2Units;

    public IEnumerable<TUnit> GetPlayerUnits(EPlayer player) => 
      player == EPlayer.First ? player1Units.Values : player2Units.Values;

    public bool HasUnits(EPlayer player) =>
      player == EPlayer.First ? player1Units.Count == 0 : player2Units.Count == 0;
    
    protected abstract void OnChangeCoord(Coord coord, TUnit unit);
  }
}