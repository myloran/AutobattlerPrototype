using Shared.OkwyLogging;

namespace Shared.Abstraction {
  public abstract class BasePlayer<TUnit> : IPlayer<TUnit> {
    public EPlayer Player { get; }
    public IUnitDict<TUnit> BoardUnits { get; private set; }
    public IUnitDict<TUnit> BenchUnits { get; private set; }

    protected BasePlayer(EPlayer player, IUnitDict<TUnit> boardUnitDict, IUnitDict<TUnit> benchUnitDict) {
      BoardUnits = boardUnitDict;
      BenchUnits = benchUnitDict;
      Player = player;
    }

    public void MoveUnit(Coord from, Coord to) {
      var fromDict = from.Y < 0 ? BenchUnits : BoardUnits;
      var toDict = to.Y < 0 ? BenchUnits : BoardUnits;

      if (!fromDict.Has(from)) {
        log.Error($"Dict does not have unit at coord: {from}");
        return;
      }

      var unit = fromDict[from];
      var hasUnitAtDestination = toDict.Has(to);

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

    protected virtual void OnChangeCoord(Coord coord, TUnit unit) { }

    static readonly Logger log = MainLog.GetLogger(nameof(BasePlayer<TUnit>));
  }
}