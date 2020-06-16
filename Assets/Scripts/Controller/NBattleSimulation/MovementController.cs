using System.Collections.Generic;
using PlasticFloor.EventBus;
using Shared;
using Shared.Abstraction;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using UnityEngine;
using View;
using View.Exts;
using View.Presenters;
using View.Views;

namespace Controller.NBattleSimulation {
  public class MovementController : IEventHandler<StartMoveEvent>, 
      IEventHandler<EndMoveEvent>, ISimulationTick {
    public MovementController(IBoard<UnitView, PlayerPresenter> board, TilePresenter tilePresenter) {
      this.board = board;
      this.tilePresenter = tilePresenter;
    }
    
    public void HandleEvent(StartMoveEvent e) {
      var unit = board[e.From];
      var from = tilePresenter.PositionAt(e.From).WithY(unit.Height);
      var to = tilePresenter.PositionAt(e.To).WithY(unit.Height);
      routines[e.From] = new MoveRoutine(unit.transform, from, to, e.StartingTime, e.Duration);
    }

    public void HandleEvent(EndMoveEvent e) {
      routines.Remove(e.From);
      board.MoveUnit(e.From, e.To);
    }

    public void SimulationTick(float time) {
      foreach (var routine in routines.Values) {
        routine.SimulationTick(time);
      }
    }

    readonly TilePresenter tilePresenter;
    readonly Dictionary<Coord, MoveRoutine> routines = new Dictionary<Coord, MoveRoutine>();
    readonly IBoard<UnitView, PlayerPresenter> board;
  }
}