using Model.NBattleSimulation.Commands;

namespace Model.NBattleSimulation {
  public class BattleSimulation {
    public ICommand Command { get; private set; }
    
    public BattleSimulation(AiContext context) {
      this.context = context;
    }
    
    public void PrepareBattle() {
      context.Board.Reset(context.Player1, context.Player2);
      
      foreach (var unit in context.Board.Units.Values) {
        unit.Reset();
        // unit.Ai.MakeDecision(context);
        var decisionCommand = new MakeDecisionCommand(unit.Ai, context);
        context.AiHeap[0] = decisionCommand;
      }
    }

    public void ExecuteNextDecision() {
      var node = context.AiHeap.RemoveMin();
      context.CurrentTime = node.Key;
      Command = node.Data;
      Command.Execute();
    }

    public bool HasNextDecision => context.AiHeap.IsEmpty();
    public TimePoint NextDecisionTime => context.AiHeap.Min().Key;

    public bool IsFinished;

    readonly AiContext context;
  }
}