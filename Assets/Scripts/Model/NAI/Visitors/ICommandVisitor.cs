using Model.NAI.UnitCommands;

namespace Model.NAI.Visitors {
  public interface ICommandVisitor {
    void VisitMakeDecisionCommand(MakeDecisionCommand command);
  }
}