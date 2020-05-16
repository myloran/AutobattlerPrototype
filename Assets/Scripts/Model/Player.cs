using System.Collections.Generic;

namespace Model {
  public class Player {
    public bool AddUnit(Unit unit) {
      if (BenchUnits.Count >= 10) return false;
      
      BenchUnits.Add(unit);
      return true;
    }
    
    List<Unit> BoardUnits = new List<Unit>(10);
    List<Unit> BenchUnits = new List<Unit>(10);
  }
}