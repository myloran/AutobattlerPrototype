using System.Collections.Generic;
using System.Linq;
using Model.NUnit.Abstraction;
using Newtonsoft.Json;
using Shared;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Model.NUnit.Components {
  public class TargetingComponent : ITargeting {
    [JsonIgnore] public IUnit Target { get; private set; }
    public Coord TargetCoord => Target?.Coord ?? Coord.Invalid; //to test determinism
    [JsonIgnore] public IEnumerable<IUnit> ArrivingTargets { get; set; }
    public IEnumerable<Coord> ArrivingTargetCoords => ArrivingTargets?.Select(t => t.Coord); //to test determinism
    public F32 TauntEndTime { get; private set; }

    public bool TargetExists => Target != null;

    public bool IsTaunted(F32 currentTime) => TauntEndTime > currentTime;
    
    public void Reset() {
      Target = null;
      ArrivingTargets = null;
      TauntEndTime = -Const.MaxBattleDuration;
    }

    public void ClearTarget() {
      if (!TargetExists) return;
      
      Target.UnsubFromDeath(this);
      Target = null;
    }

    public void Taunt(IUnit unit, F32 tauntEndTime) {
      TauntEndTime = tauntEndTime;
      ChangeTargetTo(unit);
    }

    public void ChangeTargetTo(IUnit unit) {
      ClearTarget();
      Target = unit;
      Target.SubToDeath(this);
    }
    // public override string ToString() => TargetExists ? $"Target coord: {Target.Coord}" : "" + $"{nameof(TauntEndTime)}: {TauntEndTime}";

    public override string ToString() => $"{nameof(TargetCoord)}: {TargetCoord}, {nameof(TauntEndTime)}: {TauntEndTime}, {nameof(TargetExists)}: {TargetExists}, {nameof(ArrivingTargetCoords)}: {string.Join(",", ArrivingTargetCoords ?? Enumerable.Empty<Coord>())}";
  }
}