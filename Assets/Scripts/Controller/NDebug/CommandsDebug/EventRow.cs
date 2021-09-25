using System;
using System.Collections.Generic;
using Model.NAI.Commands;
using Model.NBattleSimulation;
using PlasticFloor.EventBus;
using UniRx;
using UnityEngine.UIElements;

namespace Controller.NDebug.CommandsDebug {
  public class EventRow : IDisposable {
    public EventRow(BattleSimulation battleSimulation, EventBus eventBus, VisualTreeAsset eventTemplate) {
      this.battleSimulation = battleSimulation;
      this.eventBus = eventBus;
      this.eventTemplate = eventTemplate;
    }
    
    public void Init(CompositeDisposable disposable) {
      eventBus.Log += Log;
      disposable.Add(this);
    }

    public void OnReset() => commandEvents.Clear();
    public void Dispose() => eventBus.Log -= Log;

    void Log(string obj) {
      if (commandEvents.TryGetValue(battleSimulation.LastCommandBeingExecuted, out var events))
        events.Add(obj);
      else
        commandEvents.Add(battleSimulation.LastCommandBeingExecuted, new List<string> {obj});
    }

    public void Toggle(CommandButtonStuff stuff) {
      if (!commandEvents.TryGetValue(stuff.Command, out var events)) return;

      stuff.IsOn = !stuff.IsOn;
        
      if (stuff.IsOn)
        CreateEventRow(stuff.Template, events);
      else
        HideEventRow(stuff.Template, events);
    }
    
    void CreateEventRow(TemplateContainer commandTemplate, List<string> events) {
      if (events.Count == 0) return;

      var groupBox = commandTemplate.Q<GroupBox>();
      SetBorderWidthTo(groupBox, 1);

      foreach (var evnt in events) {
        var template = eventTemplate.Instantiate();
        var button = template.Q<Button>();
        button.text = evnt;
        groupBox.Add(template);
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

    void SetBorderWidthTo(GroupBox groupBox, int value) {
      groupBox.style.borderBottomWidth = value;
      groupBox.style.borderTopWidth = value;
      groupBox.style.borderLeftWidth = value;
      groupBox.style.borderRightWidth = value;
    }
    
    readonly Dictionary<ICommand, List<string>> commandEvents = new Dictionary<ICommand, List<string>>();
    readonly BattleSimulation battleSimulation;
    readonly BattleSimulation simulation;
    readonly EventBus eventBus;
    readonly VisualTreeAsset eventTemplate;
  }
}