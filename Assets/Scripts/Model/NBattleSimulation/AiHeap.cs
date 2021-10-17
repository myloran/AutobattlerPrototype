using System;
using System.Collections.Generic;
using System.Linq;
using FibonacciHeap;
using Model.NAI.Commands;
using Newtonsoft.Json;
using Shared.Addons.Examples.FixMath;
using Shared.Addons.OkwyLogging;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NBattleSimulation {
  public class AiHeap : IAiHeap {
    [JsonIgnore] public Action<F32, ICommand> OnInsert = (v, c) => {};
    [JsonIgnore] public Action OnReset = () => {};

    [JsonIgnore] public F32 CurrentTime { get; set; }
    
    //execute event on state change, so that command window can subscribe to changes
    public void InsertCommand(F32 time, ICommand command) {
      var nextTime = CurrentTime + time;

      if (nodes.ContainsKey(nextTime)) {
        var priorityCommand = nodes[nextTime];
        priorityCommand.AddChild(command);
        OnInsert(nextTime, command);
        return;
      }

      var pCommand = new PriorityCommand(command);
      var node = new FibonacciHeapNode<PriorityCommand, F32>(pCommand, nextTime);
      aiHeap.Insert(node);
      nodes[nextTime] = node.Data;
      OnInsert(nextTime, command);
    }

    public (bool IsEmpty, PriorityCommand PriorityCommand) RemoveMin() {
      var node = aiHeap.RemoveMin();
      if (node == null) {
        log.Info("The battle is over");
        return (true, default);
      }
      var time = node.Key;
      var command = node.Data;
      CheckForRecursiveBehaviour(time);
      CurrentTime = time;
      nodes.Remove(time);
      return (false, command);
    }

    void CheckForRecursiveBehaviour(F32 time) {
      if (time != CurrentTime) counter = 0;
      if (counter++ == 100) throw new Exception($"Recursive behaviour found"); //TODO: wait one more time, record last decision, extract that logic from here 
    }

    [JsonIgnore] public bool HasEventInHeap => aiHeap.Min() != null;
    [JsonIgnore] public F32 NextEventTime => aiHeap.Min().Key;
    
    public void Reset() {
      aiHeap.Clear();
      nodes.Clear();
      OnReset();
    }
    
    readonly FibonacciHeap<PriorityCommand, F32> aiHeap = 
      new FibonacciHeap<PriorityCommand, F32>(MinValue); //TODO: Pool commands
    
    //TODO: optimize capacity
    [JsonProperty] readonly Dictionary<F32, PriorityCommand> nodes = 
      new Dictionary<F32, PriorityCommand>(100); //TODO: fix allocations inside heap
    static readonly Logger log = MainLog.GetLogger(nameof(AiContext));
    int counter;
  }
}