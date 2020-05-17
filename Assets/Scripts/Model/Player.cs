using System.Collections.Generic;

namespace Model {
  public class Player {
    public bool AddUnit(Unit unit) {
      if (BenchUnits.Count >= 10) return false;
      
      BenchUnits.Add(unit);
      return true;
    }
    
    public bool RemoveUnit() {
      if (BenchUnits.Count <= 0) return false;
      
      BenchUnits.RemoveAt(BenchUnits.Count - 1);
      return true;
    }
    
    List<Unit> BoardUnits = new List<Unit>(10);
    List<Unit> BenchUnits = new List<Unit>(10);
  }
}