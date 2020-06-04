using System.Collections.Generic;
using System.Text;

namespace Model.NBattleSimulation.Commands {
  public class CompositeCommand : ICommand {
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
      if (command is MakeDecisionCommand)
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

    readonly LinkedList<ICommand> commands = new LinkedList<ICommand>();
  }
}