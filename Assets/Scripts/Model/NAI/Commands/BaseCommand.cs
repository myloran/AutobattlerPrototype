namespace Model.NAI.Commands {
  public abstract class BaseCommand : ICommand {
    public virtual ECommand Type { get; } = ECommand.Other;
    
    public abstract void Execute();
  }
}