using System.Collections.Generic;
using Shared;
using Shared.Abstraction;
using Shared.Poco;
using UnityEngine;
using View.NUnit;

namespace View.Presenters {
  public class PlayerPresenter {
    public readonly Dictionary<Coord, UnitView> BenchUnits = new Dictionary<Coord, UnitView>();
    public readonly Dictionary<Coord, UnitView> BoardUnits = new Dictionary<Coord, UnitView>();
    
    public PlayerPresenter(UnitViewFactory factory, UnitViewCoordChangedHandler handler) {
      this.factory = factory;
      unitMoveStrategy = new UnitMoveStrategy<UnitView>(BenchUnits, BoardUnits, handler);
    }
    
    public void MoveUnit(Coord from, Coord to) => unitMoveStrategy.MoveUnit(from, to);
    
    public void InstantiateToBench(string name, Coord coord, EPlayer player) => 
      BenchUnits[coord] = factory.Create(name, coord, player);

    public void DestroyFromBench(Coord coord) {
      Object.Destroy(BenchUnits[coord].gameObject);
      BenchUnits.Remove(coord);
    } 
    
    public void InstantiateToBoard(string name, Coord coord, EPlayer player) => 
      BoardUnits[coord] = factory.Create(name, coord, player);
    
    
    public void DestroyAll() {
      foreach (var unit in BenchUnits.Values) Object.Destroy(unit.gameObject);
      foreach (var unit in BoardUnits.Values) Object.Destroy(unit.gameObject);
      BenchUnits.Clear();
      BoardUnits.Clear();
    }

    readonly UnitViewFactory factory;
    readonly UnitMoveStrategy<UnitView> unitMoveStrategy;
  }
}