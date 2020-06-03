using System.Collections.Generic;
using System.Text;

namespace Model.NBattleSimulation.Commands {
  public class CompositeCommand : ICommand {
    public CompositeCommand(params ICommand[] commands) {
      this.commands = new LinkedList<ICommand>();
      
      foreach (var command in commands) {
        if (command is MakeDecisionCommand)
          this.commands.AddLast(command);
        else
          this.commands.AddFirst(command);
      }
    }
    
    public void Execute() {
      foreach (var command in commands) {
        command.Execute();;
      }
    }

    public override string ToString() {
      var text = new StringBuilder();

      foreach (var command in commands) {
        text.Append(command.GetType().Name).Append(", ");
      }

      return text.ToString();
    }

    readonly LinkedList<ICommand> commands;  
  }
}