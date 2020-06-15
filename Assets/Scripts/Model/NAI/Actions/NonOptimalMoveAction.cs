// using Model.NAI.NDecisionTree;
// using Model.NBattleSimulation;
// using Model.NBattleSimulation.Commands;
// using Model.NUnit;
// using PlasticFloor.EventBus;
// using Shared;
//
// namespace Model.NAI.Actions {
//   public class NonOptimalMoveAction : BaseAction {
//     public NonOptimalMoveAction(Unit unit, IEventBus bus) : base(unit, bus) { }
//     
//     public override IDecisionTreeNode MakeDecision(AiContext context) {
//       var (direction1, direction2) = direction.GetClosestDirectionsToMove();
//       var newCoord1 = movement.Coord + direction1.Normalized;
//       if (context.IsTileEmpty(newCoord1)) {
//         Move(context, movement, direction1.IsDiagonal, newCoord1, ai, target);
//         return this;
//       }
//       
//       var newCoord2 = movement.Coord + direction2.Normalized;
//       if (context.IsTileEmpty(newCoord2)) {
//         Move(context, movement, direction2.IsDiagonal, newCoord2, ai, target);
//         return this;
//       }
//
//       var direction3 = direction1.GetClosestDirectionToMove(direction);
//       var newCoord3 = movement.Coord + direction3.Normalized;
//       if (context.IsTileEmpty(newCoord3)) {
//         Move(context, movement, direction3.IsDiagonal, newCoord3, ai, target);
//         return this;
//       }
//       
//       var direction4 = direction2.GetClosestDirectionToMove(direction);
//       var newCoord4 = movement.Coord + direction4.Normalized;
//       if (context.IsTileEmpty(newCoord4)) {
//         Move(context, movement, newCoord4.IsDiagonal, newCoord4, ai, target);
//         return this;
//       }
//     }
//     
//     void Move(AiContext context, CMovement movement, bool isDiagonalMove, Coord newCoord, 
//       CAi ai, CTarget target) {
//       var time = movement.TimeToMove(isDiagonalMove);
//       new StartMoveCommand(context.Board, movement, newCoord, context.CurrentTime,time, Bus)
//         .Execute();
//       var moveCommand = new EndMoveCommand(context.Board, movement, target, newCoord, Bus);
//       context.InsertCommand(moveCommand, time);
//       var decisionCommand = new MakeDecisionCommand(ai, context, time);
//       context.InsertCommand(decisionCommand, time);
//     }
//
//     CMovement movement;
//     CAi ai;
//     CTarget target;
//   }
// }