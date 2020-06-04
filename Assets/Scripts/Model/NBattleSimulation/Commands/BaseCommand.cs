namespace Model.NBattleSimulation.Commands {
  public abstract class BaseCommand : ICommand {
    public bool IsComposite { get; } = false;

    public abstract void Execute();

    public void AddChild(ICommand command) { }
  }
}