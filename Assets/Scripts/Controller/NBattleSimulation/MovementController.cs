using System;
using System.Collections.Generic;
using PlasticFloor.EventBus;
using Shared;
using Shared.Abstraction;
using Shared.Primitives;
using Shared.Shared.Client;
using Shared.Shared.Client.Abstraction;
using Shared.Shared.Client.Events;
using UnityEngine;
using View;
using View.Exts;
using View.NTile;
using View.NUnit;
using View.NUnit.States;
using View.Presenters;
using View.Views;

namespace Controller.NBattleSimulation {
  public class MovementController : IEventHandler<StartMoveEvent>, 
      IEventHandler<FinishMoveEvent>, IEventHandler<RotateEvent>, 
      ISimulationTick, IReset {
    public MovementController(BoardPresenter board, CoordFinder finder) {
      this.board = board;
      this.finder = finder;
    }
    
    public void HandleEvent(StartMoveEvent e) {
      var unit = board.GetUnit(e.From);
      var from = finder.PositionAt(e.From).WithY(unit.Height);
      var to = finder.PositionAt(e.To).WithY(unit.Height);
      routines[e.From] = new MoveRoutine(unit.transform, from, to, e.StartingTime.Float, e.Duration.Float);
      unit.transform.rotation = (e.To - e.From).ToQuaternion(); 
    }

    public void HandleEvent(FinishMoveEvent e) {
      routines.Remove(e.From);
      board.MoveUnit(e.From, e.To);
    }
    
    public void HandleEvent(RotateEvent e) => 
      board.GetUnit(e.From).transform.rotation = (e.To - e.From).ToQuaternion();

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