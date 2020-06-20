using System.Collections.Generic;
using Model.NUnit;
using Shared;
using Shared.Abstraction;
                                          
namespace Model.NBattleSimulation {
  public class Player {
    public Player(UnitFactory unitFactory) {
      this.unitFactory = unitFactory;
      unitMoveStrategy = new UnitMoveStrategy<Unit>(
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

    public void DestroyAll() {
      BenchUnits.Clear();
      BoardUnits.Clear();
    }

    readonly UnitMoveStrategy<Unit> unitMoveStrategy;
    readonly UnitFactory unitFactory;
    public readonly Dictionary<Coord, Unit> BoardUnits = new Dictionary<Coord, Unit>();
    public readonly Dictionary<Coord, Unit> BenchUnits = new Dictionary<Coord, Unit>();
  }
}