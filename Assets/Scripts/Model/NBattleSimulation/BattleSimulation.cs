using FixMath;
using Model.NBattleSimulation.Commands;
using Shared.OkwyLogging;
using static FixMath.F32;

namespace Model.NBattleSimulation {
  public class BattleSimulation { //TODO: Think if it's controller
    public bool IsBattleOver { get; private set; }
    public F32 CurrentTime { get; private set; }

    public BattleSimulation(AiContext context, Board board) {
      this.context = context;
      this.board = board;
    }

    public void PrepareBattle(PlayerContext playerContext) {
      board.Reset(playerContext);
      context.Reset();
      IsBattleOver = context.IsPlayerDead;
      if (IsBattleOver) return;
            
      foreach (var unit in board.Values) {
        unit.Reset();
        context.InsertCommand(Zero, new MakeDecisionCommand(unit, context, Zero));
      }
    }

    public void ExecuteNextCommand() {
      var (isBattleOver, command) = context.RemoveMin();
      CurrentTime = context.CurrentTime;
      IsBattleOver = isBattleOver;
      if (IsBattleOver) return;
      
      log.Info($"{context.CurrentTime}: {command}");
      command.Execute();
    }

    readonly AiContext context;
    readonly Board board;
    static readonly Logger log = MainLog.GetLogger(nameof(BattleSimulation));
  }
}