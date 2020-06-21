
namespace Model.NAI.Commands {
  public interface ICommand {
    ECommand Type { get; }
    void Execute();
  }
}