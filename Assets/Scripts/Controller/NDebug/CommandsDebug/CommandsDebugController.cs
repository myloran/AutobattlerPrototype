using System.Collections.Generic;
using Model.NAI.Commands;
using Model.NBattleSimulation;
using Shared.Addons.Examples.FixMath;
using Shared.Exts;
using UnityEngine.UIElements;
using View.UIToolkit;

namespace Controller.NDebug {
  public class CommandsDebugController {
    public CommandsDebugController(AiHeap aiHeap, CommandsDebugUI ui) {
      this.aiHeap = aiHeap;
      this.ui = ui;
    }

    public void Init() {
      ShowUI();
      aiHeap.OnInsert += OnInsert;
      aiHeap.OnReset += OnReset;
    }

    void OnReset() {
      commands.Clear();
      commandRows.Clear();
      commandsContainer.Clear();
    }

    void OnInsert(F32 time, ICommand command) {
      if (commands.TryGetValue(time, out var priorityCommand)) {
        priorityCommand.AddChild(command);
        var commandRow = commandRows[time];
        var body = commandRow.Q<VisualElement>("Commands");
        var template = CreateCommandButton(command);
        body.Add(template);
      }
      else {
        commands.Add(time, new PriorityCommand(command));
        
        if (commandsContainer.childCount == 0) {
          var commandRow = CreateCommandRow(time, command);
          commandsContainer.Add(commandRow);
          commandRows[time] = commandRow;
          return;
        }

        //Use left node instead
        var previousTime = F32.MinValue;
        foreach (var (time2, _) in commands) {
          if (time == time2) break;
          previousTime = time2;
        }

        if (commandRows.TryGetValue(previousTime, out var pc)) {
          var index = commandsContainer.IndexOf(pc) + 1;
          var commandRow = CreateCommandRow(time, command);
          commandsContainer.Insert(index, commandRow);
          commandRows[time] = commandRow;
        }
        else {
          var commandRow = CreateCommandRow(time, command);
          commandsContainer.Insert(0, commandRow);
          commandRows[time] = commandRow;
        }
      }
    }

    void ShowUI() {
      var root = ui.Document.rootVisualElement;
      commandsContainer = root.Q<VisualElement>("CommandsBody");
    }

    TemplateContainer CreateCommandRow(F32 time, ICommand command) {
      var element = ui.CommandElementTemplate.Instantiate();
      var timeElement = element.Q<Label>("Time");
      timeElement.text = $"{time.Float:F}";
      var body = element.Q<VisualElement>("Commands");
      var template = CreateCommandButton(command);
      body.Add(template);
      return element;
    }

    TemplateContainer CreateCommandButton(ICommand command) {
      var template = ui.CommandTemplate.Instantiate();
      var button = template.Q<Button>();
      button.text = command.GetType().Name;
      return template;
    }
    
    readonly AiHeap aiHeap;
    readonly CommandsDebugUI ui;
    readonly Dictionary<F32, TemplateContainer> commandRows = new Dictionary<F32, TemplateContainer>();
    readonly SortedDictionary<F32, PriorityCommand> commands = new SortedDictionary<F32, PriorityCommand>();
    VisualElement commandsContainer;
  }
}