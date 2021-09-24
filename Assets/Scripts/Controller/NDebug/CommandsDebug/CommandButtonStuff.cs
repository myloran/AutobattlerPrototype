using Model.NAI.Commands;
using Model.NUnit.Abstraction;
using UnityEngine.UIElements;

namespace Controller.NDebug.CommandsDebug {
  public class CommandButtonStuff {
    public readonly IUnit Unit;
    public readonly ICommand Command;
    public readonly TemplateContainer Template;
    public bool IsOn;

    public CommandButtonStuff(IUnit unit, ICommand command, TemplateContainer template) {
      Unit = unit;
      Command = command;
      Template = template;
    }
  }
}