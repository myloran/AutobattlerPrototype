using System.Collections.Generic;
using Model.NUnit;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NBattleSimulation {
  public class Player {
    public Player(UnitFactory unitFactory) {
      this.unitFactory = unitFactory;
      unitMoveStrategy = new UnitMover(this, BoardUnits, BenchUnits); 
    }
    
    public void MoveUnit(Coord from, Coord to) => unitMoveStrategy.MoveUnit(from, to);

    public void InstantiateToBench(string name, Coord coord, EPlayer player) {
      var unit = unitFactory.Create(name, coord, player);
      BenchUnits.Add(unit);
    }

    public IUnit InstantiateToBoard(string name, Coord coord, EPlayer player) {
      var unit = unitFactory.Create(name, coord, player);
      BoardUnits.Add(unit);
      return unit;
    }

    public (bool, Coord) InstantiateToBenchStart(string name, EPlayer player) {
      for (int x = 0; x < 10; x++) {
        bool isCoordTaken = false;
        foreach (var benchUnit in BenchUnits) {
          if (benchUnit.StartingCoord.X != x) continue;
          
          isCoordTaken = true;
          break;
        }
        if (isCoordTaken) continue;
        
        var y = player.BenchId();
        var coord = new Coord(x, y);
        var unit = unitFactory.Create(name, coord, player);
        BenchUnits.Add(unit);
        return (true, coord);
      }
      return (false, default);
    }
    
    public (bool, Coord) DestroyFromBenchEnd(EPlayer player) {
      for (int x = 9; x >= 0; x--) {
        bool isCoordTaken = false;
        foreach (var benchUnit in BenchUnits) {
          if (benchUnit.StartingCoord.X != x) continue;
          
          isCoordTaken = true;
          break;
        }
        if (!isCoordTaken) continue;
        
        var y = player.BenchId();
        var coord = new Coord(x, y);
        var unit = GetUnitFromBench(coord).Unit;
        BenchUnits.Remove(unit);
        return (true, coord);
      }
      
      return (false, default);
    }

    public (bool HasUnit, IUnit Unit) GetUnit(Coord coord) {
      var unit = GetUnitFromBoard(coord);
      if (unit.HasUnit) return unit;
      
      return GetUnitFromBench(coord);
    }

    (bool HasUnit, IUnit Unit) GetUnitFromBoard(Coord coord) {
      foreach (var unit in BoardUnits)
        if (unit.StartingCoord == coord)
          return (true, unit);
      return (false, default);
    }

    (bool HasUnit, IUnit Unit) GetUnitFromBench(Coord coord) {
      foreach (var unit in BenchUnits)
        if (unit.StartingCoord == coord)
          return (true, unit);
      return (false, default);
    }

    public void DestroyAll() {
      BenchUnits.Clear();
      BoardUnits.Clear();
    }

    readonly UnitMover unitMoveStrategy;
    readonly UnitFactory unitFactory;
    public readonly List<IUnit> BoardUnits = new List<IUnit>();
    public readonly List<IUnit> BenchUnits = new List<IUnit>();
  }
}