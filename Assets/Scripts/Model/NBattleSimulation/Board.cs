using System.Collections.Generic;
using Model.NUnit;
using Shared;
using static Shared.Const;

namespace Model.NBattleSimulation {
  public class Board {
    public bool ContainsUnitAt(Coord coord) => units.ContainsKey(coord); 
    public Unit GetUnitAt(Coord coord) => units[coord];
    public IEnumerable<Unit> GetUnits() => units.Values;

    public void AddUnit(Coord coord, Unit unit) {
      units[coord] = unit;

      if (unit.Stats.Player == EPlayer.First)
        player1Units[coord] = unit;
      else
        player2Units[coord] = unit;
    }

    public void RemoveUnitAt(Coord coord) {
      units.Remove(coord);
      
      if (player1Units.ContainsKey(coord))
        player1Units.Remove(coord);
      else 
        player2Units.Remove(coord);
    }

    public void Reset(Player player1, Player player2) {
      units.Clear();
      player1Units.Clear();
      player2Units.Clear();
      
      foreach (var unit in player1.BoardUnits) {
        units[unit.Key] = unit.Value;
        player1Units[unit.Key] = unit.Value;
      }
      
      foreach (var unit in player2.BoardUnits) {
        units[unit.Key] = unit.Value;
        player2Units[unit.Key] = unit.Value;
      }
    }
    
    readonly Dictionary<Coord, Unit> units = new Dictionary<Coord, Unit>(MaxUnitsOnBoard);
    readonly Dictionary<Coord, Unit> player1Units = new Dictionary<Coord, Unit>(MaxUnitsOnBench);
    readonly Dictionary<Coord, Unit> player2Units = new Dictionary<Coord, Unit>(MaxUnitsOnBench);

    public IEnumerable<Unit> GetPlayerUnits(EPlayer player) => 
      player == EPlayer.First ? player1Units.Values : player2Units.Values;

    public bool HasUnits(EPlayer player) =>
      player == EPlayer.First ? player1Units.Count == 0 : player2Units.Count == 0;
  }
}