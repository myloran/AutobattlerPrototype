using System.Collections.Generic;
using System.Text;

namespace Model.NAI.Commands {
  public class CompositeCommand : ICommand { //PriorityCommand
    public bool IsComposite { get; } = true;
    
    public CompositeCommand(params ICommand[] commands) {
      foreach (var command in commands) {
        AddChild(command);
      }
    }

    public void Execute() {
      foreach (var command in commands) {
        command.Execute();;
      }
    }

    public void AddChild(ICommand command) {
      if (command is MakeDecisionCommand) //TODO: expose enum type of command, remove isCOmposite, addChild
        commands.AddLast(command);
      else
        commands.AddFirst(command);
    }

    public override string ToString() {
      var text = new StringBuilder();

      foreach (var command in commands) {
        if (command.IsComposite)
          text.Append(command).Append(", ");
        else
          text.Append(command.GetType().Name).Append(", ");
      }

      return text.ToString();
    }

    readonly LinkedList<ICommand> commands = new LinkedList<ICommand>(); //TODO: Compare performance to 2 lists
  }
}