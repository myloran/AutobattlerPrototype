using Controller.Update;
using FixMath;
using Model.NBattleSimulation;
using Shared.Shared.Client;
using UnityEngine;
using static FixMath.F32;

namespace Controller {
  public class RealtimeBattleSimulationController : ITick {
    public RealtimeBattleSimulationController(ISimulationTick viewSimulation, 
        BattleSimulation simulation, EventHolder eventHolder) {
      this.viewSimulation = viewSimulation;
      this.simulation = simulation;
      this.eventHolder = eventHolder;
    }
    
    public void StartBattle() {
      ExecuteAllDecisions();
      currentTime = Zero;
      isStarted = true;
    }

    public void Tick() {
      if (!isStarted || isPaused) return;

      currentTime += ToF32(Time.deltaTime/* * speed*/);
      
      while (eventHolder.HasEventInHeap && eventHolder.NextEventTime < currentTime) {
        eventHolder.RaiseFromHeap();
      }
      
      viewSimulation.SimulationTick(currentTime.Float);
    }
    
    void ExecuteAllDecisions() {
      eventHolder.NeedExecuteImmediately = false;
      
      while (!simulation.IsBattleOver) {
        simulation.ExecuteNextCommand();
      }
      
      eventHolder.NeedExecuteImmediately = true;
    }

    public void SetPaused(bool isPaused) {
      this.isPaused = isPaused;
      Time.timeScale = isPaused ? 0 : speed;
    }

    public void SetSpeed(float speed) {
      this.speed = speed;
      Time.timeScale = speed;
    }

    readonly ISimulationTick viewSimulation;
    readonly BattleSimulation simulation;
    readonly EventHolder eventHolder;
    F32 currentTime;
    bool isPaused, isStarted;
    float speed;
  }
}