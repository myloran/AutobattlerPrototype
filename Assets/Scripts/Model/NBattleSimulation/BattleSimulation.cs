using Model.NBattleSimulation.Commands;
using Shared.OkwyLogging;
using static FixMath.F32;

namespace Model.NBattleSimulation {
  public class BattleSimulation { //TODO: Think if it's controller
    public bool IsBattleOver { get; private set; }

    public BattleSimulation(AiContext context, Board board) {
      this.context = context;
      this.board = board;
    }

    public void PrepareBattle(BoardContext boardContext) {
      board.SetContext(boardContext);
      
      foreach (var unit in board.Values) {
        unit.Reset();
      }
      
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

    readonly AiContext context;
    readonly Board board;
    static readonly Logger log = MainLog.GetLogger(nameof(BattleSimulation));
  }
}