using Model.NAI.UnitCommands;
using Model.NAI.Visitors;

namespace Controller.Visitors {
  public class DisplayToViewCommandVisitor : ICommandVisitor {
    public DisplayToViewCommandVisitor(IActionVisitor visitor) => this.visitor = visitor;

    public void VisitMakeDecisionCommand(MakeDecisionCommand command) => command.Decision.Accept(visitor);

    readonly IActionVisitor visitor;
  }
}