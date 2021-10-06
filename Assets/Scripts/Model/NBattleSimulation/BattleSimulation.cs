using System;
using Model.Determinism;
using Model.NAI.Commands;
using Newtonsoft.Json;
using Shared.Addons.Examples.FixMath;
using Shared.Addons.OkwyLogging;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NBattleSimulation {
  public class BattleSimulation {
    public ICommand LastCommandBeingExecuted;
    public bool IsBattleOver { get; private set; }

    public BattleSimulation(AiContext context, Board board, AiHeap heap) {
      this.context = context;
      this.board = board;
      this.heap = heap;
      hashCalculator = new HashCalculator();
    }

    public void PrepareBattle(PlayerContext playerContext) {
      board.Reset(playerContext);
      
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

      hashResult = "";
    }

    public void ExecuteNextCommand() {
      var settings = new JsonSerializerSettings() {
        ContractResolver = new PrivateContractResolver(),
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
      };
      var serialized = JsonConvert.SerializeObject(context, Formatting.Indented/*, settings*/);
      var stringified = context.GetType().Name + serialized;
      log.Info($"stringified: {stringified}");
      
      var hash = hashCalculator.GetObjectHash(context);
      var hex = hashCalculator.BytesToHex(hash);
      
      var (isEmpty, priorityCommand) = context.RemoveMin();
      hashResult += "\n" + hex + " - " + priorityCommand;
      log.Info($"{context.CurrentTime}: {priorityCommand}, {nameof(hash)}: {hex}");
      IsBattleOver = isEmpty || context.IsBattleOver;
      if (IsBattleOver) return;
      
      foreach (var command in priorityCommand.Commands) {
        LastCommandBeingExecuted = command;
        command.Execute();
      }
    }

    public void ExecuteAllCommands() {
      while (!IsBattleOver) {
        ExecuteNextCommand();
      }
      log.Info($"{nameof(hashResult)}: {hashResult}");
    }

    public void ExecuteCommandsTill(F32 time) {
      while (!IsBattleOver && heap.HasEventInHeap && heap.NextEventTime < time) {
        ExecuteNextCommand();
        counter++;
        if (counter == 1000) {
          throw new Exception();
        }
      }
      log.Info($"{nameof(hashResult)}: {hashResult}");
    }

    int counter;

    readonly AiContext context;
    readonly AiHeap heap;
    readonly Board board;
    readonly HashCalculator hashCalculator;
    string hashResult;
    static readonly Logger log = MainLog.GetLogger(nameof(BattleSimulation));
  }
}