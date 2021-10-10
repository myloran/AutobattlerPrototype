using System.Collections.Generic;
using PlasticFloor.EventBus;
using Shared.Abstraction;
using Shared.Shared.Client.Events;
using SharedClient.Abstraction;
using View.NUnit;
using View.Presenters;

namespace Controller.NBattleSimulation {
  public class SilenceController : IEventHandler<UpdateSilenceDurationEvent>, ISimulationTick, IReset {
    public SilenceController(BoardPresenter board) {
      this.board = board;
    }
    
    public void HandleEvent(UpdateSilenceDurationEvent e) {
      var unit = board.TryGetUnit(e.Coord);
      if (unit == null) return;
      
      unitSilenceDurations[unit] = e.Duration.Float;
      unit.ShowSilenceCross();
    }

    public void SimulationTick(float time) {
      foreach (var pair in unitSilenceDurations) {
        if (pair.Value < time) 
          unitSilencesMarkedForRemove.Add(pair.Key);
      }

      foreach (var unit in unitSilencesMarkedForRemove) {
        unitSilenceDurations.Remove(unit);
        unit.HideSilenceCross();
      }
      unitSilencesMarkedForRemove.Clear();
    }
    
    public void Reset() => unitSilenceDurations.Clear();

    readonly Dictionary<UnitView, float> unitSilenceDurations = new Dictionary<UnitView, float>();
    readonly List<UnitView> unitSilencesMarkedForRemove = new List<UnitView>();
    readonly BoardPresenter board;
  }
}