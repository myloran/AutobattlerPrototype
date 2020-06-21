using Model.NAI.Commands;
using Shared.Addons.Examples.FixMath;
using Shared.Addons.OkwyLogging;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NBattleSimulation {
  public class BattleSimulation {
    public bool IsBattleOver { get; private set; }

    public BattleSimulation(AiContext context, Board board, AiHeap heap) {
      this.context = context;
      this.board = board;
      this.heap = heap;
    }

    public void PrepareBattle(PlayerContext playerContext) {
      board.SetContext(playerContext);
      
      foreach (var unit in board.Values) {
        unit.Reset();
      }
      
      heap.Reset();
      context.Reset();
      
      IsBattleOver = context.IsBattleOver;
      if (IsBattleOver) return;
            
      foreach (var unit in board.Values) {
        context.InsertCommand(Zero, new MakeDecisionCommand(unit, context, Zero));
      }
    }

    public void ExecuteNextCommand() {
      var (isEmpty, command) = context.RemoveMin();
      IsBattleOver = isEmpty || context.IsBattleOver;
      if (IsBattleOver) return;
      
      log.Info($"{context.CurrentTime}: {command}");
      command.Execute();
    }

    public void ExecuteAllCommands() {
      while (!IsBattleOver) {
        ExecuteNextCommand();
      }
    }

    public void ExecuteCommandsTill(F32 time) {
      while (!IsBattleOver && heap.NextEventTime < time) {
        ExecuteNextCommand();
      }
    }

    readonly AiContext context;
    readonly AiHeap heap;
    readonly Board board;
    static readonly Logger log = MainLog.GetLogger(nameof(BattleSimulation));
  }
}