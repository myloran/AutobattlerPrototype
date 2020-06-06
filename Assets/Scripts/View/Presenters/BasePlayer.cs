using Shared;

namespace View.Presenters {
  public abstract class BasePlayer<TUnit> : IPlayer<TUnit> {
     public IUnitDict<TUnit> BoardUnits { get; private set; }
     public IUnitDict<TUnit> BenchUnits { get; private set; }

     public void Init(IUnitDict<TUnit> boardUnitDict, IUnitDict<TUnit> benchUnitDict) {
      BoardUnits = boardUnitDict;
      BenchUnits = benchUnitDict;
    }

     public void MoveUnit(Coord from, Coord to) {
      var fromDict = from.Y < 0 ? BenchUnits : BoardUnits;
      var toDict = to.Y < 0 ? BenchUnits : BoardUnits;
        
      if (!fromDict.Contains(from)) {
        log.Error($"Dict does not have unit at coord: {from}");
        return;
      }
      var unit = fromDict[from];
      var hasUnitAtDestination = toDict.Contains(to);

      if (hasUnitAtDestination)
        SwapUnits(from, to, fromDict, toDict, unit);
      else
        MoveUnit(from, to, fromDict, toDict, unit);
     }

     void MoveUnit(Coord from, Coord to, IUnitDict<TUnit> fromDict, 
       IUnitDict<TUnit> toDict, TUnit fromUnit) {
       fromDict.Remove(from);
       OnChangeCoord(to, fromUnit);
       toDict[to] = fromUnit;
     }

     void SwapUnits(Coord from, Coord to, IUnitDict<TUnit> fromDict, 
       IUnitDict<TUnit> toDict, TUnit fromUnit) {
       OnChangeCoord(to, fromUnit);
       OnChangeCoord(from, toDict[to]);
       fromDict[from] = toDict[to];
       toDict[to] = fromUnit;
     }

     protected abstract void OnChangeCoord(Coord coord, TUnit unit);

     static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(BasePlayer<TUnit>));
  }
}