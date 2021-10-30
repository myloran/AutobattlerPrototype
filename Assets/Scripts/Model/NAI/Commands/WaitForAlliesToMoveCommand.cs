using System.Collections.Generic;
using System.Linq;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAI.Commands {
  public class WaitForAlliesToMoveCommand : BaseCommand {
    public WaitForAlliesToMoveCommand(IUnit unit, AiContext context) : base(unit) {
      this.unit = unit;
      this.context = context;
    }

    public override void Execute() {
      var allyUnits = context.GetSurroundUnits(unit.Coord);
      var enemyUnits = context.EnemyUnits(Unit.Player.Opposite());
      var units = allyUnits.Concat(enemyUnits).ToList();
      MakeDecisionOnMoveFinished(units);
    }

    void MakeDecisionOnMoveFinished(List<IUnit> units) {
      var isHandled = false;
      foreach (var u in units) u.OnMoveFinished += OnMoveFinished;
      
      void OnMoveFinished() {
        if (isHandled) return;
        
        isHandled = true;
        foreach (var u in units) u.OnMoveFinished -= OnMoveFinished;
        context.InsertCommand(Zero, new MakeDecisionCommand(unit, context, Zero));
      }
    }

    readonly IUnit unit;
    readonly AiContext context;
  }
}