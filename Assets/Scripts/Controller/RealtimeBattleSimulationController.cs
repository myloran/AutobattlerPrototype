using Controller.Update;
using Model.NBattleSimulation;
using Shared.Shared.Client;
using UnityEngine;

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
      startTime = Time.realtimeSinceStartup;
      isBattleStarted = true;
    }

    public void Tick() {
      var currentTime = Time.realtimeSinceStartup - startTime;

      while (eventHolder.HasEventInHeap && eventHolder.NextEventTime < currentTime) {
        eventHolder.RaiseFromHeap();
      }
      
      viewSimulation.SimulationTick(currentTime);
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
    float startTime;
    bool isBattleStarted;
  }
}