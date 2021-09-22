using Model.NAI.Commands;
using Shared.Addons.Examples.FixMath;

namespace Model.NBattleSimulation {
  public interface IAiHeap {
    F32 CurrentTime { get; set; }
    bool HasEventInHeap { get; }
    F32 NextEventTime { get; }
    
    void InsertCommand(F32 time, ICommand command);
    (bool IsEmpty, PriorityCommand PriorityCommand) RemoveMin();
    void Reset();
  }
}