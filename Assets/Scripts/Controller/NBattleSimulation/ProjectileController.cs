using System.Collections.Generic;
using PlasticFloor.EventBus;
using Shared.Abstraction;
using Shared.Primitives;
using Shared.Shared.Client.Events;
using SharedClient.Abstraction;
using UnityEngine;
using View;
using View.Exts;
using View.NTile;
using View.Presenters;

namespace Controller.NBattleSimulation {
  public class ProjectileController : IEventHandler<SpawnProjectileEvent>,  
    ISimulationTick, IReset {
    public ProjectileController(BoardPresenter board, CoordFinder finder) {
      this.board = board;
      this.finder = finder;
    }
    
    public void HandleEvent(SpawnProjectileEvent e) {
      var unit = board.TryGetUnit(e.From);
      if (unit == null || unit.Projectile == null) return;

      const float unitHalfHeight = 0.8f;
      var adjustment = new Vector3(-0.421236f, -0.337472f, -0.39f);
      var from = finder.PositionAt(e.From).WithY(unit.Height).WithY(unitHalfHeight) - adjustment;
      var to = finder.PositionAt(e.To).WithY(unit.Height).WithY(unitHalfHeight);
      var rotation = Quaternion.LookRotation(to - from);
      var projectile = Object.Instantiate(unit.Projectile, from, rotation);
      var routine = new MoveRoutine(projectile.transform, from, to, e.StartingTime.Float, e.Duration.Float);
      routines.Add(routine);
    }

    public void SimulationTick(float time) {
      foreach (var routine in routines) {
        routine.SimulationTick(time);
        
        if (routine.EndTime < time) 
          routinesMarkedForRemove.Add(routine);
      }

      foreach (var routine in routinesMarkedForRemove) {
        routines.Remove(routine);
        Object.Destroy(routine.Obj.gameObject);
      }
      
      routinesMarkedForRemove.Clear();
    }
    
    public void Reset() => routines.Clear();

    readonly CoordFinder finder;
    readonly List<MoveRoutine> routines = new List<MoveRoutine>(),
      routinesMarkedForRemove = new List<MoveRoutine>();
    readonly BoardPresenter board;
  }
}