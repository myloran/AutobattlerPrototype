using System;
using System.Collections.Generic;
using FibonacciHeap;
using Model.NAI.Commands;
using Shared.Addons.Examples.FixMath;
using Shared.Addons.OkwyLogging;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NBattleSimulation {
  public class AiHeap : IAiHeap {
    public readonly SortedDictionary<F32, PriorityCommand> DebugTree = new SortedDictionary<F32, PriorityCommand>();
    public Action<F32, ICommand> OnInsert = (v, c) => {};
    public Action OnReset = () => {};

    public F32 CurrentTime { get; set; }
    
    //execute event on state change, so that command window can subscribe to changes
    public void InsertCommand(F32 time, ICommand command) {
      var nextTime = CurrentTime + time;

      if (nodes.ContainsKey(nextTime)) {
        var priorityCommand = nodes[nextTime].Data;
        priorityCommand.AddChild(command);
        OnInsert(nextTime, command);
        return;
      }

      var pCommand = new PriorityCommand(command);
      var node = new FibonacciHeapNode<PriorityCommand, F32>(pCommand, nextTime);
      aiHeap.Insert(node);
      DebugTree[nextTime] = pCommand;
      nodes[nextTime] = node;
      OnInsert(nextTime, command);
    }

    public (bool IsEmpty, ICommand Command) RemoveMin() {
      var node = aiHeap.RemoveMin();
      if (node == null) {
        log.Info("The battle is over");
        return (true, default);
      }
      var time = node.Key;
      var command = node.Data;
      CurrentTime = time;
      DebugTree.Remove(time);
      nodes.Remove(time);
      return (false, command);
    }
    
    public bool HasEventInHeap => aiHeap.Min() != null;
    public F32 NextEventTime => aiHeap.Min().Key;
    
    public void Reset() {
      aiHeap.Clear();
      nodes.Clear();
      OnReset();
    }
    
    readonly FibonacciHeap<PriorityCommand, F32> aiHeap = 
      new FibonacciHeap<PriorityCommand, F32>(MinValue); //TODO: Pool commands
    
    //TODO: optimize capacity
    readonly Dictionary<F32, FibonacciHeapNode<PriorityCommand, F32>> nodes = 
      new Dictionary<F32, FibonacciHeapNode<PriorityCommand, F32>>(100); //TODO: fix allocations inside heap
    static readonly Logger log = MainLog.GetLogger(nameof(AiContext));
  }
}