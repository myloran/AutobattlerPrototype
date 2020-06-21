using System;
using System.Collections.Generic;
using PlasticFloor.EventBus;
using Shared;
using Shared.Abstraction;
using Shared.Poco;
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
      IEventHandler<IdleEvent>, ISimulationTick {
    public MovementController(BoardPresenter boardPresenter, CoordFinder coordFinder) {
      this.boardPresenter = boardPresenter;
      this.coordFinder = coordFinder;
    }
    
    public void HandleEvent(StartMoveEvent e) {
      var unit = boardPresenter.GetUnit(e.From);
      var from = coordFinder.PositionAt(e.From).WithY(unit.Height);
      var to = coordFinder.PositionAt(e.To).WithY(unit.Height);
      routines[e.From] = new MoveRoutine(unit.transform, from, to, e.StartingTime.Float, e.Duration.Float);
      unit.transform.rotation = (e.To - e.From).ToQuaternion(); 
      unit.ChangeStateTo(EState.Walking); //TODO: make specific animation event and handle them in animation controller?
    }

    public void HandleEvent(FinishMoveEvent e) {
      routines.Remove(e.From);
      boardPresenter.MoveUnit(e.From, e.To);
    }
    
    public void HandleEvent(RotateEvent e) => 
      boardPresenter.GetUnit(e.From).transform.rotation = (e.To - e.From).ToQuaternion();

    public void SimulationTick(float time) {
      foreach (var routine in routines.Values) {
        routine.SimulationTick(time);
      }
    }
    
    public void HandleEvent(IdleEvent e) => 
      boardPresenter.GetUnit(e.Coord).ChangeStateTo(EState.Idle);

    readonly CoordFinder coordFinder;
    readonly Dictionary<Coord, MoveRoutine> routines = new Dictionary<Coord, MoveRoutine>();
    readonly BoardPresenter boardPresenter;
  }
}