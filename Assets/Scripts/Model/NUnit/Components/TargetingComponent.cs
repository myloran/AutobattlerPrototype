using System.Collections.Generic;
using System.Linq;
using Model.NUnit.Abstraction;
using Newtonsoft.Json;
using Shared.Primitives;

namespace Model.NUnit.Components {
  public class TargetingComponent : ITargeting {
    [JsonIgnore] public IUnit Target { get; private set; }
    public Coord TargetCoord => Target?.Coord ?? Coord.Invalid; //to test determinism
    [JsonIgnore] public IEnumerable<IUnit> ArrivingTargets { get; set; }
    public IEnumerable<Coord> ArrivingTargetCoords => ArrivingTargets?.Select(t => t.Coord); //to test determinism

    public bool TargetExists => Target != null;
    
    public void Reset() {
      Target = null;
      ArrivingTargets = null;
    }

    public void ClearTarget() {
      if (!TargetExists) return;
      
      Target.UnsubFromDeath(this);
      Target = null;
    }

    public void ChangeTargetTo(IUnit unit) {
      ClearTarget();
      Target = unit;
      Target.SubToDeath(this);
    }
    
    //TODO: log target without it's target?
    public override string ToString() => TargetExists ? $"Target coord: {Target.Coord}" : "";
  }
}