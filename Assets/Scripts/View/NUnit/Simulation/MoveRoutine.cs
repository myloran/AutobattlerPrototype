using System;
using SharedClient.Abstraction;
using UnityEngine;

namespace View {
  public class MoveRoutine : ISimulationTick {
    public readonly Transform Obj;
    public float EndTime { get; private set; }

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
      
      var timeClamped = Mathf.Clamp(time, startTime, EndTime);
      var durationPassed = timeClamped - startTime;
      var t = durationPassed / duration;
      if (from == to) return;
      
      Obj.transform.position = Vector3.Lerp(from, to, t);
    }

    public void Pause() => isPaused = true;
    
    public void Unpause(float startTime, float duration) {
      this.startTime = startTime;
      this.duration = duration;
      isPaused = false;
      from = Obj.transform.position;
      EndTime = startTime + duration;
    }

    Vector3 from;
    readonly Vector3 to;
    float duration;
    float startTime;
    bool isPaused;
  }
}