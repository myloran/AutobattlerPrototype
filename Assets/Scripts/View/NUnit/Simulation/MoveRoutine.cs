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
      if (isPaused) return;
      
      var adjustedTime = time - pauseTimeLeft;
      var timeClamped = Mathf.Clamp(adjustedTime, startTime, EndTime);
      var durationPassed = timeClamped - startTime;
      var t = durationPassed / duration;
      Obj.transform.position = Vector3.Lerp(from, to, t);
    }
    
    public void Pause(float durationLeft) {
      isPaused = true;
      pauseTimeLeft += durationLeft;
    }
    
    public void Unpause() => isPaused = false;

    readonly Vector3 from;
    readonly Vector3 to;
    readonly float duration, startTime;
    float pauseTimeLeft;
    bool isPaused;
  }
}