using System.Collections.Generic;
using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation {
  public class Player : BasePlayer<Unit> {
    public Player(IUnitDict<Unit> boardUnitDict, IUnitDict<Unit> benchUnitDict) : 
      base(boardUnitDict, benchUnitDict) { }
    
    // public void AddBenchUnit(string name, Coord coord, int playerId) => 
    //   BenchUnits[coord] = unitFactory.Create(name, coord, playerId);
    //
    // public void RemoveBenchUnit(Coord coord) => BenchUnits.Remove(coord);
    //
    // public void AddBoardUnit(string name, Coord coord, int playerId) => 
    //   BoardUnits[coord] = unitFactory.Create(name, coord, playerId);
    //
    // public void RemoveBoardUnit(Coord coord) => BoardUnits.Remove(coord);

    protected override void OnChangeCoord(Coord coord, Unit unit) => 
      unit.Movement.StartingCoord = coord;
  }
}