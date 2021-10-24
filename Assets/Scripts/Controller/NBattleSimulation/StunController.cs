using System.Collections.Generic;
using PlasticFloor.EventBus;
using Shared.Abstraction;
using Shared.Shared.Client.Events;
using SharedClient.Abstraction;
using View.NUnit;
using View.Presenters;

namespace Controller.NBattleSimulation {
  public class StunController : IEventHandler<UpdateStunDurationEvent>, ISimulationTick, IReset {
    public StunController(BoardPresenter board) {
      this.board = board;
    }
    
    public void HandleEvent(UpdateStunDurationEvent e) {
      var unit = board.TryGetUnit(e.Coord);
      if (unit == null) return;
      
      unitDurations[unit] = e.Duration.Float;
      unit.ShowSilenceCross();
    }

    public void SimulationTick(float time) {
      foreach (var pair in unitDurations) {
        if (pair.Value < time) 
          unitStunsMarkedForRemove.Add(pair.Key);
      }

      foreach (var unit in unitStunsMarkedForRemove) {
        unitDurations.Remove(unit);
        unit.HideSilenceCross();
      }
      unitStunsMarkedForRemove.Clear();
    }
    
    public void Reset() => unitDurations.Clear();

    readonly Dictionary<UnitView, float> unitDurations = new Dictionary<UnitView, float>();
    readonly List<UnitView> unitStunsMarkedForRemove = new List<UnitView>();
    readonly BoardPresenter board;
  }
}