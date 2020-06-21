using System.Collections.Generic;
using FibonacciHeap;
using Model.NAI.Commands;
using Shared.Addons.Examples.FixMath;
using Shared.Addons.OkwyLogging;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NBattleSimulation {
  public class AiHeap {
    public F32 CurrentTime { get; set; }
    
    public void InsertCommand(F32 time, ICommand command) {
      var nextTime = CurrentTime + time;

      if (nodes.ContainsKey(nextTime)) {
        var existingNode = nodes[nextTime];
        var existingCommand = existingNode.Data;
        existingNode.Data = new CompositeCommand(existingCommand, command); //TODO: replace with once composite command instead of nested ones
        return;
      }

      var node = new FibonacciHeapNode<ICommand, F32>(command, nextTime);
      aiHeap.Insert(node);
      nodes[nextTime] = node;
    }

    public (bool, ICommand) RemoveMin() {
      var node = aiHeap.RemoveMin();
      if (node == null) {
        log.Info("The battle is over");
        return (true, default);
      }
      var time = node.Key;
      var command = node.Data;
      CurrentTime = time;
      nodes.Remove(CurrentTime);
      return (false, command);
    }
    
    public bool HasEventInHeap => aiHeap.Min() != null;
    public F32 NextEventTime => aiHeap.Min().Key;
    
    public void Reset() {
      aiHeap.Clear();
      nodes.Clear();
    }
    
    readonly FibonacciHeap<ICommand, F32> aiHeap = 
      new FibonacciHeap<ICommand, F32>(MinValue); //TODO: use composite command instead of ICommand
    
    readonly Dictionary<F32, FibonacciHeapNode<ICommand, F32>> nodes = 
      new Dictionary<F32, FibonacciHeapNode<ICommand, F32>>(); //TODO: fix allocations inside heap
    static readonly Logger log = MainLog.GetLogger(nameof(AiContext));
  }
}