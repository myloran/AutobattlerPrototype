using Shared.OkwyLogging;

namespace Model.NBattleSimulation {
  public class BattleSimulation {
    public bool IsBattleOver { get; private set; }

    public BattleSimulation(AiContext context) {
      this.context = context;
    }

    public void PrepareBattle(Player player1, Player player2) {
      context.Reset(player1, player2);
      IsBattleOver = context.IsPlayerDead;
    }

    public void ExecuteNextCommand() {
      var (isBattleOver, command) = context.RemoveMin();
      IsBattleOver = isBattleOver;
      if (IsBattleOver) return;
      
      log.Info($"{context.CurrentTime}: {command}");
      command.Execute();
    }

    readonly AiContext context;
    static readonly Logger log = MainLog.GetLogger(nameof(BattleSimulation));
  }
}