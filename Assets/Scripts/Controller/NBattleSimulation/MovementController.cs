using System.Collections.Generic;
using PlasticFloor.EventBus;
using Shared;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using View;

namespace Controller.NBattleSimulation {
  public class MovementController : IEventHandler<StartMoveEvent>, 
      IEventHandler<EndMoveEvent>, ISimulationTick {
    public MovementController(BoardView board) {
      this.board = board;
    }
    
    public void HandleEvent(StartMoveEvent e) {
      routines[e.From] = board.MoveRoutine(e.From, e.To, e.StartingTime, e.Duration);
    }

    public void HandleEvent(EndMoveEvent e) {
      routines.Remove(e.From);
      board.Move(e.From, e.To);
    }

    public void SimulationTick(float time) {
      foreach (var routine in routines.Values) {
        routine.SimulationTick(time);
      }
    }

    readonly Dictionary<Coord, MoveRoutine> routines = new Dictionary<Coord, MoveRoutine>();
    readonly BoardView board;
  }
}