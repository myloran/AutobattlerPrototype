using Model.NUnit.Abstraction;

namespace Model.NAI.Commands {
  public abstract class BaseCommand : ICommand {
    protected BaseCommand(IUnit unit) {
      Unit = unit;
    }
    
    public virtual ECommand Type { get; } = ECommand.Other;
    public IUnit Unit { get; }
    
    public abstract void Execute();
  }
}