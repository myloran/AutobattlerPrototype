using System.Collections.Generic;
using Model.NAI.Commands;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using UnityEngine.UIElements;
using View.UIToolkit;

namespace Controller.NDebug.CommandsDebug {
  public class CommandRow {
    public CommandRow(EventRow eventRow, CommandButtonStyler styler,
      CommandsDebugUI ui) {
      this.ui = ui;
      this.eventRow = eventRow;
      this.styler = styler;
    }
    
    public void OnReset() {
      eventRow.OnReset();
      commandStuff.Clear();
      unitCommands.Clear();
    }
    
    public TemplateContainer InstantiateCommandRow(F32 time, ICommand command) {
      var template = ui.CommandRowTemplate.Instantiate();
      var timeLabel = template.Q<Label>("Time");
      timeLabel.text = $"{time.Float:F}";
      var commandRow = template.Q<VisualElement>("Commands");
      var button = InstantiateCommandButton(command);
      commandRow.Add(button);
      return template;
    }

    public TemplateContainer InstantiateCommandButton(ICommand command) {
      var template = ui.CommandTemplate.Instantiate();
      var button = ConfigureButton(template, command);
      if (command is BaseCommand baseCommand) {
        commandStuff[button] = new CommandButtonStuff(baseCommand.Unit, command, template);
        
        if (unitCommands.TryGetValue(baseCommand.Unit, out var commands))
          commands.Add(button);
        else
          unitCommands[baseCommand.Unit] = new List<Button> {button};
      }
      return template;
    }

    Button ConfigureButton(TemplateContainer template, ICommand command) {
      var button = template.Q<Button>();
      var commandName = command.GetType().Name;
      button.text = styler.SimplifyCommandName(commandName);
      button.RegisterCallback<MouseEnterEvent>(OnEnter);
      button.RegisterCallback<MouseLeaveEvent>(OnLeave);
      button.RegisterCallback<ClickEvent>(OnClick);
      return button;
    }

    void OnClick(ClickEvent evt) {
      if (evt.target is Button button && commandStuff.TryGetValue(button, out var stuff))
        eventRow.Toggle(stuff);
    }

    void OnEnter(MouseEnterEvent evt) {
      if (evt.target is Button button && commandStuff.TryGetValue(button, out var stuff)) {
        var buttons = unitCommands.TryGetValue(stuff.Unit, out var bs) ? bs : new List<Button>();
        styler.Highlight(buttons, stuff.Unit);
      }
    }

    void OnLeave(MouseLeaveEvent evt) {
      if (evt.target is Button button && commandStuff.TryGetValue(button, out var stuff)) {
        var buttons = unitCommands.TryGetValue(stuff.Unit, out var bs) ? bs : new List<Button>();
        styler.Unhighlight(buttons, stuff.Unit);
      }
    }

    readonly Dictionary<IUnit, List<Button>> unitCommands = new Dictionary<IUnit, List<Button>>();
    readonly Dictionary<Button, CommandButtonStuff> commandStuff = new Dictionary<Button, CommandButtonStuff>();
    readonly CommandsDebugUI ui;
    readonly EventRow eventRow;
    readonly CommandButtonStyler styler;
  }
}