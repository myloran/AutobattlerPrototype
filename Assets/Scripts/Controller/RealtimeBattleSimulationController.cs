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
      startTime = ToF32(Time.realtimeSinceStartup);
      isBattleStarted = true;
    }

    public void Tick() {
      var currentTime = ToF32(Time.realtimeSinceStartup) - startTime;

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

    readonly ISimulationTick viewSimulation;
    readonly BattleSimulation simulation;
    readonly EventHolder eventHolder;
    F32 startTime;
    bool isBattleStarted;
  }
}