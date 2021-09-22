using System.Collections.Generic;
using Model.NAI.Commands;
using Model.NBattleSimulation;
using Shared.Addons.Examples.FixMath;

namespace Controller.NDebug {
  public class AiHeapDebug : IAiHeap {
    public AiHeapDebug(IAiHeap aiHeap) {
      this.aiHeap = aiHeap;
    }

    public F32 CurrentTime {
      get => aiHeap.CurrentTime;
      set => aiHeap.CurrentTime = value;
    }

    public bool HasEventInHeap => aiHeap.HasEventInHeap;
    public F32 NextEventTime => aiHeap.NextEventTime;

    public void InsertCommand(F32 time, ICommand command) {
      aiHeap.InsertCommand(time, command);
      tree[time] = command;
    }

    public (bool IsEmpty, PriorityCommand PriorityCommand) RemoveMin() {
      var result = aiHeap.RemoveMin();
      tree.Remove(CurrentTime);
      return result;
    }

    public void Reset() => aiHeap.Reset();

    readonly IAiHeap aiHeap;
    readonly SortedDictionary<F32, ICommand> tree = new SortedDictionary<F32, ICommand>();
  }
}