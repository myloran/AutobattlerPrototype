using System.Collections.Generic;

namespace Shared.Abstraction {
  public abstract class BaseBoard<TUnit, TPlayer> : IBoard<TUnit, TPlayer> 
      where TPlayer : IPlayer<TUnit> where TUnit : IUnit {
    protected BaseBoard(IUnitDict<TUnit> units, IUnitDict<TUnit> player1Units, 
        IUnitDict<TUnit> player2Units) {
      this.Units = units;
      this.player1Units = player1Units;
      this.player2Units = player2Units;
    }

    public TUnit this[Coord coord] => Units[coord];

    public bool ContainsUnitAt(Coord coord) => Units.Has(coord); 
    public IEnumerable<TUnit> Values => Units.Values;

    public void AddUnit(Coord coord, TUnit unit) => Units[coord] = unit;
    public void RemoveUnit(Coord coord) => Units.Remove(coord);

    public void MoveUnit(Coord from, Coord to) {
      var unit = Units[from];
      OnChangeCoord(to, unit);
      AddUnit(to, unit);
      RemoveUnit(from);
    }
    
    public void Reset(TPlayer player1, TPlayer player2) {
      player1Units = player1.BoardUnits;
      player2Units = player2.BoardUnits;
      
      Units.Units.Clear();
      foreach (var (coord, unit) in player1Units) Units[coord] = unit;
      foreach (var (coord, unit) in player2Units) Units[coord] = unit;
      // foreach (var unit in Units.Values) OnReset(unit);
    }
    //TODO: ResetBack unit coord and coord in unit dict

    public IEnumerable<TUnit> GetPlayerUnits(EPlayer player) => 
      player == EPlayer.First ? player1Units.Values : player2Units.Values;

    public bool HasUnits(EPlayer player) =>
      player == EPlayer.First ? player1Units.Count == 0 : player2Units.Count == 0;
    
    protected abstract void OnChangeCoord(Coord coord, TUnit unit);

    protected readonly IUnitDict<TUnit> Units;

    IUnitDict<TUnit> player1Units,
      player2Units;
  }
}