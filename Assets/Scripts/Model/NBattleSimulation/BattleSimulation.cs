using System;
using Model.Determinism;
using Model.NAI.Commands;
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

      hashCalculator.Reset();
    }

    public void ExecuteNextCommand() {
      var (isEmpty, priorityCommand) = context.RemoveMin();
      hashCalculator.Calculate(context, priorityCommand, context.CurrentTime);
      
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
      hashCalculator.PrintReport();
    }

    public void ExecuteCommandsTill(F32 time) {
      counter = 0;
      while (!IsBattleOver && heap.HasEventInHeap && heap.NextEventTime < time) {
        ExecuteNextCommand();
        if (++counter == 1000) throw new Exception();
      }
    }

    int counter;

    readonly AiContext context;
    readonly AiHeap heap;
    readonly Board board;
    readonly HashCalculator hashCalculator;
  }
}