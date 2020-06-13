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
      player1Units = player1.BoardUnits;
      player2Units = player2.BoardUnits;
      
      units.Clear();
      foreach (var (coord, unit) in player1Units) units[coord] = unit;
      foreach (var (coord, unit) in player2Units) units[coord] = unit;
    }
    
    //TODO: ResetBack unit coord and coord in unit dict

    public IEnumerable<TUnit> GetPlayerUnits(EPlayer player) => 
      player == EPlayer.First ? player1Units.Values : player2Units.Values;

    public bool HasUnits(EPlayer player) =>
      player == EPlayer.First ? player1Units.Count == 0 : player2Units.Count == 0;
    
    protected abstract void OnChangeCoord(Coord coord, TUnit unit);

    IUnitDict<TUnit> units;

    IUnitDict<TUnit> player1Units,
      player2Units;
  }
}