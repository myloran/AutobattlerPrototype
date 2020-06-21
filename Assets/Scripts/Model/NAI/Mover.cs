using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Poco;

namespace Model.NAI {
  public class Mover {
    public bool Move(IUnit unit, AiContext context, out MoveInfo moveInfo) {
      this.context = context;
      this.unit = unit;
      targetCoord = unit.Target.Coord;
      moveInfo = Mover.moveInfo;
      
      var direction = (targetCoord - unit.Coord).Normalized;
      if (Move(direction)) return true;

      var (direction1, direction2) = direction.GetClosestDirections();
      if (SmartMove(direction1, direction2)) return true;

      var direction3 = direction1.GetClosestDirection(direction);
      var direction4 = direction2.GetClosestDirection(direction);
      if (SmartMove(direction3, direction4)) return true;
      
      var direction5 = direction3.GetClosestDirection(direction1);
      var direction6 = direction4.GetClosestDirection(direction2);
      if (SmartMove(direction5, direction6)) return true;

      return false;
    }

    bool SmartMove(Coord direction1, Coord direction2) {
      var coord1 = unit.Coord + direction1;
      var coord2 = unit.Coord + direction2;
      var canMove1 = context.IsTileEmpty(coord1);
      var canMove2 = context.IsTileEmpty(coord2);

      if (canMove1 && canMove2) return SmartMove2();
      if (canMove1) return PureMove(coord1, direction1);
      if (canMove2) return PureMove(coord2, direction2);

      return false;
      
      //Checks which move will be closer to the target after simulating second move
      bool SmartMove2() {
        var (couldMove1, nextCoord1) = SimulateNextMove(coord1);
        var (couldMove2, nextCoord2) = SimulateNextMove(coord2);
        var distance1 = CoordExt.SqrDistance(targetCoord, nextCoord1);
        var distance2 = CoordExt.SqrDistance(targetCoord, nextCoord2);

        if (distance1 == distance2) return SmartMove3();

        return distance1 < distance2
          ? PureMove(coord1, direction1)
          : PureMove(coord2, direction2);

        //Selects move which didn't move in simulation and has the same distance from target as the other one
        bool SmartMove3() {
          if (couldMove1) return PureMove(coord2, direction2);
          if (couldMove2) return PureMove(coord1, direction1);

          return PureMove(coord1, direction1);
        }
      }
    }

    bool PureMove(Coord coord, Coord direction) {
      var time = unit.TimeToMove(direction.IsDiagonal);
      moveInfo.Update(coord, time);
      return true;
    }

    (bool, Coord) SimulateNextMove(Coord coord) {
      (bool, Coord) res = (false, coord);
      
      var direction = (unit.Target.Coord - coord).Normalized;
      if (SimulateMove(direction, ref res)) return res;

      var (direction1, direction2) = direction.GetClosestDirections();
      if (SimulateMove(direction1, ref res)) return res;
      if (SimulateMove(direction2, ref res)) return res;

      return res;
      
      bool SimulateMove(Coord moveDirection, ref (bool, Coord) result) {
        var pos = coord + moveDirection;
        if (!context.IsTileEmpty(pos)) return false;
        
        result = (true, pos);
        return true;
      }
    }

    bool Move(Coord direction) {
      var newCoord = unit.Coord + direction;
      if (!context.IsTileEmpty(newCoord)) return false;

      return PureMove(newCoord, direction);
    }

    IUnit unit;
    AiContext context;
    Coord targetCoord;
    static readonly MoveInfo moveInfo = new MoveInfo();
  }
}