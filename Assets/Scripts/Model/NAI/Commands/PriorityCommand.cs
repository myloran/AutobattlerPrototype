using System.Collections.Generic;
using System.Text;

namespace Model.NAI.Commands {
  public class PriorityCommand : ICommand {
    public ECommand Type { get; } = ECommand.Other;
    
    public PriorityCommand(ICommand command) => AddChild(command);

    public void Execute() {
      foreach (var command in commands) {
        command.Execute();;
      }
    }

    public void AddChild(ICommand command) {
      if (command.Type == ECommand.MakeDecision)
        commands.AddLast(command);
      else
        commands.AddFirst(command);
    }

    public override string ToString() {
      var text = new StringBuilder();

      foreach (var command in commands) 
        text.Append(command.GetType().Name).Append(", ");

      return text.ToString();
    }

    readonly LinkedList<ICommand> commands = new LinkedList<ICommand>(); //TODO: Compare performance to 2 lists
  }
}