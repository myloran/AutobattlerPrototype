using System.Collections.Generic;
using Model.NUnit;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Abstraction;
using Shared.Poco;

namespace Model.NBattleSimulation {
  public class Player {
    public Player(UnitFactory unitFactory) {
      this.unitFactory = unitFactory;
      unitMoveStrategy = new UnitMoveStrategy<IUnit>(
        BenchUnits, BoardUnits, new UnitCoordChangedHandler());
    }
    
    public void MoveUnit(Coord from, Coord to) => unitMoveStrategy.MoveUnit(from, to);

    public void InstantiateToBench(string name, Coord coord, EPlayer player) => 
      BenchUnits[coord] = unitFactory.Create(name, coord, player);

    public void InstantiateToBoard(string name, Coord coord, EPlayer player) => 
      BoardUnits[coord] = unitFactory.Create(name, coord, player);

    public (bool, Coord) InstantiateToBenchStart(string name, EPlayer player) {
      for (int x = 0; x < 10; x++) {
        var y = player.BenchId();
        var coord = new Coord(x, y);
        if (BenchUnits.ContainsKey(coord)) {
          continue;
        }

        BenchUnits[coord] = unitFactory.Create(name, coord, player);
        return (true, new Coord(x, y));
      }
      return (false, default);
    }
    
    public (bool,Coord) DestroyFromBenchEnd(EPlayer player) {
      for (int x = 9; x >= 0; x--) {
        var y = player.BenchId();
        var coord = new Coord(x, y);
        if (!BenchUnits.ContainsKey(coord)) continue;
        
        BenchUnits.Remove(coord);
        return (true, coord);
      }
      return (false, default);
    }

    public (bool, IUnit) GetUnit(Coord coord) {
      if (BoardUnits.TryGetValue(coord, out var unit1)) return (true, unit1);
      if (BenchUnits.TryGetValue(coord, out var unit2)) return (true, unit2);
      return (false, default);
    }

    public void DestroyAll() {
      BenchUnits.Clear();
      BoardUnits.Clear();
    }

    readonly UnitMoveStrategy<IUnit> unitMoveStrategy;
    readonly UnitFactory unitFactory;
    public readonly Dictionary<Coord, IUnit> BoardUnits = new Dictionary<Coord, IUnit>();
    public readonly Dictionary<Coord, IUnit> BenchUnits = new Dictionary<Coord, IUnit>();
  }
}