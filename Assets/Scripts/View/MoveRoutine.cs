using Shared.Shared.Client;
using UnityEngine;
using View.Exts;

namespace View {
  public class MoveRoutine : ISimulationTick {
    public MoveRoutine(UnitView fromUnit, TileView fromTile, TileView toTile, 
        float startTime, float duration) {
      this.fromUnit = fromUnit;
      this.duration = duration;
      this.startTime = startTime;
      endTime = startTime + duration;
      var height = fromUnit.Height;
      fromPosition = fromTile.transform.position.WithY(height);
      toPosition = toTile.transform.position.WithY(height);
    }
    
    public void SimulationTick(float time) {
      var timeClamped = Mathf.Clamp(time, startTime, endTime);
      var durationPassed = timeClamped - startTime;
      var t = durationPassed / duration;
      fromUnit.transform.position = Vector3.Lerp(fromPosition, toPosition, t);
    }
    
    readonly UnitView fromUnit;
    readonly Vector3 fromPosition, toPosition;
    readonly float duration, startTime, endTime;
  }
}