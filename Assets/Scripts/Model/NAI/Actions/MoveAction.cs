using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared;
using Shared.Addons.OkwyLogging;
using Shared.Poco;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAI.Actions {
  public class MoveAction : BaseAction {
    public override EDecision Type { get; } = EDecision.MoveAction;
    public IDecisionTreeNode FindNearestTarget;
    
    public MoveAction(IUnit unit, IEventBus bus) : base(unit, bus) { }

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      if (mover.Move(Unit, context, out var info)) {
        var (coord, time) = info;
        new StartMoveCommand(context, Unit, coord, time, Bus).Execute(); 
        context.InsertCommand(time, new FinishMoveCommand(context, Unit, coord, Bus));
        context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));
        return this;
      }

      if (context.IsSurrounded(Unit.Coord) && !Unit.IsWaiting) { //TODO: guard against cyclic make decision in general way and remove IsWaiting
        Unit.IsWaiting = true;
        var decisionCommand = new WaitForAlliesToMoveCommand(Unit, context);
        context.InsertCommand(Zero, decisionCommand);
        return this;
      }
      Unit.IsWaiting = false;
                              
      //TODO: remove find nearest target
      if (context.IsCyclicDecision) {
        log.Error($"Cyclic reference {nameof(MoveAction)}");
        return this;
      }
      Unit.ClearTarget();
      context.IsCyclicDecision = true;
      return FindNearestTarget.MakeDecision(context);
    }

    static readonly Mover mover = new Mover();
    static readonly Logger log = MainLog.GetLogger(nameof(MoveAction));
  }
}