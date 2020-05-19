using System.Collections.Generic;
using Shared;
using UnityEngine;

namespace Model {
  public class Player {
    public void AddUnit(Unit unit, Coord coord) {
      benchUnits[coord] = unit;
    }
    
    public void RemoveUnit(Coord coord) {
      benchUnits.Remove(coord);
    }

    public void MoveUnit(Coord from, Coord to) {
      var fromDict = from.Y < 0 ? benchUnits : boardUnits;
      var toDict = to.Y < 0 ? benchUnits : boardUnits;
        
      if (!fromDict.ContainsKey(from)) {
        Debug.LogError($"Dict does not have unit at coord: {from}");
        return;
      }
      var fromUnit = fromDict[from];
      
      if (toDict.ContainsKey(to)) {
        fromDict[from] = toDict[to];  
        toDict[to] = fromUnit;
        //swap unit coords as well
      }
      else {
        fromDict.Remove(from);
        toDict.Add(to, fromUnit);
        //add coord to unit itself
      }
    }

    readonly Dictionary<Coord, Unit> boardUnits = new Dictionary<Coord, Unit>(10);
    readonly Dictionary<Coord, Unit> benchUnits = new Dictionary<Coord, Unit>(10);
  }
}