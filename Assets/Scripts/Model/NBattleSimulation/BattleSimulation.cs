using System.Diagnostics;
using MinMaxHeap;

namespace Model.NBattleSimulation {
  public class BattleSimulation {
    public bool IsBattleOver { get; private set; }

    public BattleSimulation(AiContext context) {
      this.context = context;
    }

    public void PrepareBattle(Player player1, Player player2) {
      context.Reset(player1, player2);
      IsBattleOver = context.IsPlayerDead;
      
      var heap = new MinHeap<string, int>();
      heap.Add("item1", 3);
      // heap.Add("item2", 4);
      heap.Add("item3", 5);
      heap.ChangeValue("item2", 1);
      var x = heap.ExtractMin().Value;
      log.Info($"x: {x}");
      x = heap.ExtractMin().Value;
      log.Info($"x: {x}");
      x = heap.ExtractMin().Value;
      log.Info($"x: {x}");
    }

    public void ExecuteNextDecision() {
      var node = context.AiHeap.RemoveMin();
      if (node == null) {
        log.Info("The battle is over");
        IsBattleOver = true;
        return;
      }
      var time = node.Key;
      var command = node.Data;
      
      if (context.IsPlayerDead && time > context.PlayerDeathTime) {
        IsBattleOver = true;
        return;
      }

      context.CurrentTime = time;
      command.Execute();
      log.Info($"{context.CurrentTime}: {command}");
    }

    readonly AiContext context;
    static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(BattleSimulation));
  }
}