using Model.NAI.Visitors;

namespace Model.NAI.UnitCommands {
  public interface ICommand {
    void Execute();
    void Accept(ICommandVisitor visitor);
  }
}