
namespace Model.NAI.Commands {
  public interface ICommand {
    bool IsComposite { get; }
    void Execute();
    void AddChild(ICommand command);
  }
}