using System.Collections.Generic;
using Shared;
using Shared.Abstraction;
using UnityEngine;
using View.Factories;
using View.NUnit;

namespace View.Presenters {
  public class PlayerPresenter {
    public PlayerPresenter(TilePresenter tilePresenter, UnitViewFactory factory) {
      this.factory = factory;
      unitMoveStrategy = new UnitMoveStrategy<UnitView>(BoardUnits, benchUnits,
        new UnitViewCoordChangedHandler(tilePresenter));
    }
    
    public void InstantiateToBench(string name, Coord coord, EPlayer player) => 
      benchUnits[coord] = factory.Create(name, coord, player);

    public void DestroyOnBench(Coord coord) {
      Object.Destroy(benchUnits[coord].gameObject);
      benchUnits.Remove(coord);
    } 
    
    public void InstantiateToBoard(string name, Coord coord, EPlayer player) => 
      BoardUnits[coord] = factory.Create(name, coord, player);
    
    public void MoveUnit(Coord from, Coord to) => unitMoveStrategy.MoveUnit(from, to);
    
    public void DestroyAll() {
      benchUnits.Clear();
      BoardUnits.Clear();
    }

    readonly UnitViewFactory factory;
    readonly UnitMoveStrategy<UnitView> unitMoveStrategy;
    readonly Dictionary<Coord, UnitView> benchUnits = new Dictionary<Coord, UnitView>();
    public readonly Dictionary<Coord, UnitView> BoardUnits = new Dictionary<Coord, UnitView>();
  }
}