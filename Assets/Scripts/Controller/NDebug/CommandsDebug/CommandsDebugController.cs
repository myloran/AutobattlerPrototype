using System;
using System.Collections.Generic;
using Model.NAI.Commands;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Exts;
using UnityEngine;
using UnityEngine.UIElements;
using View.NUnit;
using View.Presenters;
using View.UIToolkit;

namespace Controller.NDebug {
  public class CommandsDebugController {
    class UnitDebugStuff {
      public readonly IUnit Unit;
      public readonly ICommand Command;
      public readonly TemplateContainer Template;

      public bool IsOn;

      public UnitDebugStuff(IUnit unit, ICommand command, TemplateContainer template) {
        Unit = unit;
        Command = command;
        Template = template;
      }
    }
    
    public CommandsDebugController(AiHeap aiHeap, BoardPresenter boardPresenter, BattleSimulation battleSimulation,
        CommandsDebugUI ui, EventBus eventBus) {
      this.aiHeap = aiHeap;
      this.boardPresenter = boardPresenter;
      this.battleSimulation = battleSimulation;
      this.ui = ui;
      this.eventBus = eventBus;
    }

    public void Init() {
      InitUI();
      eventBus.Log += Log;
      aiHeap.OnInsert += OnInsert;
      aiHeap.OnReset += OnReset;
    }

    void Log(string obj) {
      if (commandEvents.TryGetValue(battleSimulation.LastCommandBeingExecuted, out var events))
        events.Add(obj);
      else
        commandEvents.Add(battleSimulation.LastCommandBeingExecuted, new List<string> {obj});
    }

    void OnReset() {
      unitStuff.Clear();
      sortedCommands.Clear();
      commandRows.Clear();
      commandsContainer.Clear();
      commandEvents.Clear();
    }

    void OnInsert(F32 time, ICommand command) {
      if (sortedCommands.TryGetValue(time, out var priorityCommand)) {
        priorityCommand.AddChild(command);
        var commandRow = commandRows[time];
        var body = commandRow.Q<VisualElement>("Commands");
        var template = CreateCommandButton(command);
        body.Add(template);
      }
      else {
        sortedCommands.Add(time, new PriorityCommand(command));
        
        if (commandsContainer.childCount == 0) {
          var commandRow = CreateCommandRow(time, command);
          commandsContainer.Add(commandRow);
          commandRows[time] = commandRow;
          return;
        }

        //Use left node instead
        var previousTime = F32.MinValue;
        foreach (var (time2, _) in sortedCommands) {
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

    void InitUI() {
      var root = ui.Document.rootVisualElement;
      commandsContainer = root.Q<VisualElement>("CommandsBody");
      ui.Document.rootVisualElement.visible = false;
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
      var commandName = command.GetType().Name;
      button.text = SimplifyCommandName(commandName);
      if (command is BaseCommand baseCommand) {
        unitStuff[button] = new UnitDebugStuff(baseCommand.Unit, command, template);
        
        if (unitCommands.TryGetValue(baseCommand.Unit, out var commands))
          commands.Add(button);
        else
          unitCommands[baseCommand.Unit] = new List<Button> {button};
      }
      button.RegisterCallback<MouseEnterEvent>(OnEnter);
      button.RegisterCallback<MouseLeaveEvent>(OnLeave);
      button.RegisterCallback<ClickEvent>(OnClick);
      return template;
    }

    void OnClick(ClickEvent evt) {
      if (evt.target is Button button && unitStuff.TryGetValue(button, out var unitDebugStuff) 
                                      && commandEvents.TryGetValue(unitDebugStuff.Command, out var events)) {
        unitDebugStuff.IsOn = !unitDebugStuff.IsOn;
        
        if (unitDebugStuff.IsOn)
          CreateEventRow(unitDebugStuff.Template, events);
        else
          HideEventRow(unitDebugStuff.Template, events);
      }
    }

    void HideEventRow(TemplateContainer commandTemplate, List<string> events) {
      if (events.Count == 0) return;

      var groupBox = commandTemplate.Q<GroupBox>();
      SetBorderWidthTo(groupBox, 0);

      for (int i = groupBox.childCount - 1; i >= 0; i--) {
        groupBox.RemoveAt(i);
      }
    }

    void CreateEventRow(TemplateContainer commandTemplate, List<string> events) {
      if (events.Count == 0) return;

      var groupBox = commandTemplate.Q<GroupBox>();
      SetBorderWidthTo(groupBox, 1);

      foreach (var evnt in events) {
        var template = ui.EventTemplate.Instantiate();
        var button = template.Q<Button>();
        button.text = evnt;
        groupBox.Add(template);
      }
    }

    static void SetBorderWidthTo(GroupBox groupBox, int value) {
      groupBox.style.borderBottomWidth = value;
      groupBox.style.borderTopWidth = value;
      groupBox.style.borderLeftWidth = value;
      groupBox.style.borderRightWidth = value;
    }

    void OnEnter(MouseEnterEvent evt) {
      if (evt.target is Button button && unitStuff.TryGetValue(button, out var stuff) 
                                      && boardPresenter.TryGetUnit(stuff.Unit.Coord, out var unitView)) {
        unitView.Highlight();
        HighlightButton(button);
        HighlightUnitButtons(stuff.Unit, HighlightButton);
      }
    }

    void HighlightButton(Button button) {
      defaultButtonBackgroundColor = button.style.backgroundColor; 
      button.style.backgroundColor = new StyleColor(Color.green);
    }

    void UnhighlightButton(Button button) => button.style.backgroundColor = defaultButtonBackgroundColor;

    void HighlightUnitButtons(IUnit unit, Action<Button> highlight) {
      if (!unitCommands.TryGetValue(unit, out var buttons)) return;
      
      foreach (var button in buttons) highlight(button);
    }

    void OnLeave(MouseLeaveEvent evt) {
      if (evt.target is Button button && unitStuff.TryGetValue(button, out var stuff)
                                      && boardPresenter.TryGetUnit(stuff.Unit.Coord, out var unitView)) {
        unitView.Unhighlight();
        UnhighlightButton(button);
        HighlightUnitButtons(stuff.Unit, UnhighlightButton);
      }
    }

    string SimplifyCommandName(string name) {
      var index = name.LastIndexOf('C');
      return index > 0 
        ? name.Substring(0, index) 
        : name;
    }
    
    readonly AiHeap aiHeap;
    readonly BoardPresenter boardPresenter;
    readonly BattleSimulation battleSimulation;
    readonly CommandsDebugUI ui;
    readonly EventBus eventBus;
    readonly Dictionary<Button, UnitDebugStuff> unitStuff = new Dictionary<Button, UnitDebugStuff>();
    readonly Dictionary<F32, TemplateContainer> commandRows = new Dictionary<F32, TemplateContainer>();
    readonly SortedDictionary<F32, PriorityCommand> sortedCommands = new SortedDictionary<F32, PriorityCommand>();
    readonly Dictionary<ICommand, List<string>> commandEvents = new Dictionary<ICommand, List<string>>();
    readonly Dictionary<IUnit, List<Button>> unitCommands = new Dictionary<IUnit, List<Button>>();
    VisualElement commandsContainer;
    StyleColor defaultButtonBackgroundColor;
  }
}