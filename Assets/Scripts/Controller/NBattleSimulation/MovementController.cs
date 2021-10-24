using System.Collections.Generic;
using PlasticFloor.EventBus;
using Shared.Abstraction;
using Shared.Primitives;
using Shared.Shared.Client.Events;
using SharedClient.Abstraction;
using View;
using View.Exts;
using View.NTile;
using View.Presenters;

namespace Controller.NBattleSimulation {
  public class MovementController : IEventHandler<StartMoveEvent>, IEventHandler<FinishMoveEvent>, 
    IEventHandler<PauseMoveEvent>, IEventHandler<ContinueMoveEvent>, IEventHandler<RotateEvent>, ISimulationTick, IReset {
    public MovementController(BoardPresenter board, CoordFinder finder) {
      this.board = board;
      this.finder = finder;
    }
    
    public void HandleEvent(StartMoveEvent e) {
      var unit = board.TryGetUnit(e.From);
      if (unit == null) return;
      
      var from = finder.PositionAt(e.From).WithY(unit.Height);
      var to = finder.PositionAt(e.To).WithY(unit.Height);
      routines[e.From] = new MoveRoutine(unit.transform, from, to, e.StartingTime.Float, e.Duration.Float);
      unit.transform.rotation = (e.To - e.From).ToQuaternion(); 
    }

    public void HandleEvent(FinishMoveEvent e) {
      routines.Remove(e.From);
      board.MoveUnit(e.From, e.To);
    }
    
    public void HandleEvent(PauseMoveEvent e) => routines[e.Coord].Pause(e.PauseDuration.Float);
    public void HandleEvent(ContinueMoveEvent e) => routines[e.Coord].Unpause(); 

    public void HandleEvent(RotateEvent e) {
      var unit = board.TryGetUnit(e.From);
      if (unit == null) return;
      
      unit.transform.rotation = (e.To - e.From).ToQuaternion();
    }

    public void SimulationTick(float time) {
      foreach (var routine in routines.Values) 
        routine.SimulationTick(time);
    }
    
    public void Reset() => routines.Clear();

    readonly CoordFinder finder;
    readonly Dictionary<Coord, MoveRoutine> routines = new Dictionary<Coord, MoveRoutine>();
    readonly BoardPresenter board;
  }
}