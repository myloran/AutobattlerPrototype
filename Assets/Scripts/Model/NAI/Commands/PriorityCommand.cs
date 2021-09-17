using System.Collections.Generic;
using System.Text;

namespace Model.NAI.Commands {
  public class PriorityCommand : ICommand {
    public readonly LinkedList<ICommand> Commands = new LinkedList<ICommand>(); //TODO: Compare performance to 2 lists
    public ECommand Type { get; } = ECommand.Other;
    
    public PriorityCommand(ICommand command) => AddChild(command);

    public void Execute() {
      foreach (var command in Commands) {
        command.Execute();
        //TODO: add debug check if command is the one we want to stop and exit immediately after that, so that we can see game state in our point of interest
      }
    }

    public void AddChild(ICommand command) {
      if (command.Type == ECommand.MakeDecision)
        Commands.AddLast(command);
      else
        Commands.AddFirst(command);
    }

    public override string ToString() {
      var text = new StringBuilder();

      foreach (var command in Commands) 
        text.Append(command.GetType().Name).Append(", ");

      return text.ToString();
    }
  }
}