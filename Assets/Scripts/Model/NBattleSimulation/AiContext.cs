using System.Collections.Generic;
using FibonacciHeap;
using Model.NAI.UnitCommands;
using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation {
  public class AiContext {
    readonly Player[] players;
    public Player Player1 => players[0];
    public Player Player2 => players[1];
    public TimePoint CurrentTime;

    public Board Board;
    public readonly FibonacciHeap<ICommand, TimePoint> AiHeap;
    
    public AiContext(Player[] players, Board board, FibonacciHeap<ICommand, TimePoint> aiHeap) {
      this.players = players;
      Board = board;
      AiHeap = aiHeap;
    }

    public IEnumerable<Unit> EnemyUnits(EPlayer player) {
      return players[(int) player.Opposite()].BoardUnits.Values;
    }

    public bool IsTileEmpty(Coord coord) => !Board.Units.ContainsKey(coord);
  }
}