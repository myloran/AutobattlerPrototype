using System.Collections.Generic;
using Model.NAI.Commands;
using Model.NBattleSimulation;
using Shared.Addons.Examples.FixMath;
using Shared.Exts;
using UnityEngine.UIElements;
using View.UIToolkit;

namespace Controller.NDebug.CommandsDebug {
  public class CommandsDebugController {
    public CommandsDebugController(AiHeap aiHeap, CommandRow commandRow, CommandsDebugUI commandsDebugUI) {
      this.aiHeap = aiHeap;
      this.commandRow = commandRow;
      ui = commandsDebugUI;
    }

    public void Init() {
      InitUI();
      aiHeap.OnInsert += OnInsert;
      aiHeap.OnReset += OnReset;
    }
    
    void InitUI() {
      var root = ui.Document.rootVisualElement;
      commandsContainer = root.Q<VisualElement>("CommandsBody");
      ui.Document.rootVisualElement.visible = false;
    }

    void OnReset() {
      commandRow.OnReset();
      sortedCommands.Clear();
      commandRows.Clear();
      commandsContainer.Clear();
    }

    void OnInsert(F32 time, ICommand command) {
      if (sortedCommands.TryGetValue(time, out var priorityCommand)) {
        InstantiateCommandButton(time, command, priorityCommand);
      }
      else {
        sortedCommands.Add(time, new PriorityCommand(command));
        InstantiateCommandRow(time, command);
      }
    }

    void InstantiateCommandButton(F32 time, ICommand command, PriorityCommand priorityCommand) {
      priorityCommand.AddChild(command);
      var row = commandRows[time];
      var body = row.Q<VisualElement>("Commands");
      var template = commandRow.InstantiateCommandButton(command);
      body.Add(template);
    }
    
    void InstantiateCommandRow(F32 time, ICommand command) {
      if (commandsContainer.childCount == 0) {
        InstantiateCommandRowAt(time, command, 0);
        return;
      }

      if (commandRows.TryGetValue(GetPreviousTime(time), out var pc)) {
        var index = commandsContainer.IndexOf(pc) + 1;
        InstantiateCommandRowAt(time, command, index);
      }
      else {
        InstantiateCommandRowAt(time, command, 0);
      }
    }

    void InstantiateCommandRowAt(F32 time, ICommand command, int index) {
      var row = commandRow.InstantiateCommandRow(time, command);
      commandsContainer.Insert(index, row);
      commandRows[time] = row;
    }

    F32 GetPreviousTime(F32 time) { //Use left node instead
      var previousTime = F32.MinValue;
      foreach (var (time2, _) in sortedCommands) {
        if (time == time2) break;
        previousTime = time2;
      }

      return previousTime;
    }

    readonly AiHeap aiHeap;
    readonly CommandsDebugUI ui;
    readonly Dictionary<F32, TemplateContainer> commandRows = new Dictionary<F32, TemplateContainer>();
    readonly SortedDictionary<F32, PriorityCommand> sortedCommands = new SortedDictionary<F32, PriorityCommand>();
    readonly CommandRow commandRow;
    VisualElement commandsContainer;
  }
}