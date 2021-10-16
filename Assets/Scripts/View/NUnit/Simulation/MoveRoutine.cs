using SharedClient.Abstraction;
using UnityEngine;

namespace View {
  public class MoveRoutine : ISimulationTick {
    public readonly Transform Obj;
    public readonly float EndTime;

    public MoveRoutine(Transform obj, Vector3 from, Vector3 to, 
        float startTime, float duration) {
      this.Obj = obj;
      this.from = from;
      this.to = to;
      this.duration = duration;
      this.startTime = startTime;
      EndTime = startTime + duration;
    }
    
    public void SimulationTick(float time) {
      var timeClamped = Mathf.Clamp(time, startTime, EndTime);
      var durationPassed = timeClamped - startTime;
      var t = durationPassed / duration;
      Obj.transform.position = Vector3.Lerp(from, to, t);
    }

    readonly Vector3 from;
    readonly Vector3 to;
    readonly float duration, startTime;
  }
}