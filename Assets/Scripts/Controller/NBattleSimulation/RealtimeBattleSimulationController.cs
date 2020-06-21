using Controller.Update;
using Model.NBattleSimulation;
using Shared.Addons.Examples.FixMath;
using Shared.Shared.Client.Abstraction;
using UnityEngine;
using static Shared.Addons.Examples.FixMath.F32;

namespace Controller.NBattleSimulation {
  public class RealtimeBattleSimulationController : ITick {
    public RealtimeBattleSimulationController(ISimulationTick viewSimulation, 
        BattleSimulation simulation) {
      this.viewSimulation = viewSimulation;
      this.simulation = simulation;
    }
    
    public void StartBattle() {
      if (simulation.IsBattleOver) return;
      currentTime = Zero;
      isStarted = true;
    }

    public void StopBattle() => isStarted = false;

    public void Tick() {
      if (!isStarted || isPaused) return;

      currentTime += ToF32(Time.deltaTime);
      simulation.ExecuteCommandsTill(currentTime);
      viewSimulation.SimulationTick(currentTime.Float);
    }

    public void SetPaused(bool isPaused) {
      this.isPaused = isPaused;
      Time.timeScale = isPaused ? 0 : speed;
    }

    public void SetSpeed(float speed) {
      this.speed = speed;
      if (isPaused) return;
      
      Time.timeScale = speed;
    }

    readonly ISimulationTick viewSimulation;
    readonly BattleSimulation simulation;
    F32 currentTime;
    bool isPaused, isStarted;
    float speed;
  }
}