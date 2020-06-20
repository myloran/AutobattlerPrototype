using System.Collections.Generic;
using Shared;
using Shared.Abstraction;
using Shared.Poco;
using UnityEngine;
using View.NTile;
using View.NUnit;

namespace View.Presenters {
  public class PlayerPresenter {
    public readonly Dictionary<Coord, UnitView> BenchUnits = new Dictionary<Coord, UnitView>();
    public readonly Dictionary<Coord, UnitView> BoardUnits = new Dictionary<Coord, UnitView>();
    
    public PlayerPresenter(TilePresenter tilePresenter, UnitViewFactory factory) {
      this.factory = factory;
      unitMoveStrategy = new UnitMoveStrategy<UnitView>(BoardUnits, BenchUnits,
        new UnitViewCoordChangedHandler(tilePresenter));
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
      BenchUnits.Clear();
      BoardUnits.Clear();
    }

    readonly UnitViewFactory factory;
    readonly UnitMoveStrategy<UnitView> unitMoveStrategy;
  }
}