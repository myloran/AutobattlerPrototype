using System.Collections.Generic;
using Model.NUnit;
using Shared;
using UnityEngine;

namespace Model.NBattleSimulation {
  public class Player {
    public readonly Dictionary<Coord, Unit> BoardUnits = new Dictionary<Coord, Unit>(10);
    public readonly Dictionary<Coord, Unit> BenchUnits = new Dictionary<Coord, Unit>(10);
    
    public Player(UnitFactory unitFactory) {
      this.unitFactory = unitFactory;
    }
    
    public void AddBenchUnit(string name, Coord coord, int playerId) => 
      BenchUnits[coord] = unitFactory.Create(name, coord, playerId);

    public void RemoveBenchUnit(Coord coord) => BenchUnits.Remove(coord);
    
    public void AddBoardUnit(string name, Coord coord, int playerId) => 
      BoardUnits[coord] = unitFactory.Create(name, coord, playerId);

    public void RemoveBoardUnit(Coord coord) => BoardUnits.Remove(coord);
    
    // public void AddUnit(string name, Coord coord, int playerId) => 
    //   BenchUnits[coord] = unitFactory.Create(name, playerId);
    //
    // public void RemoveUnit(Coord coord) => BenchUnits.Remove(coord);

    public void MoveUnit(Coord from, Coord to) {
      var fromDict = from.Y < 0 ? BenchUnits : BoardUnits;
      var toDict = to.Y < 0 ? BenchUnits : BoardUnits;
        
      if (!fromDict.ContainsKey(from)) {
        Debug.LogError($"Dict does not have unit at coord: {from}");
        return;
      }
      var fromUnit = fromDict[from];
      
      if (toDict.ContainsKey(to)) {
        fromDict[from] = toDict[to];
        fromDict[from].Movement.StartingCoord = from;
        toDict[to] = fromUnit;
        toDict[to].Movement.StartingCoord = to;
      }
      else {
        fromDict.Remove(from);
        toDict[to] = fromUnit;
        toDict[to].Movement.StartingCoord = to;
      }
    }

    readonly UnitFactory unitFactory;
  }
}