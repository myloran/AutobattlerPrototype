using System;
using Model.NBattleSimulation;
using Model.NUnit;
using Shared;
using Shared.OkwyLogging;

namespace Model {
  public class ModelContext {
    public ModelContext(Player[] players) {
      this.players = players;
    }
    
    public Unit GetUnit(Coord coord) {
      foreach (var player in players) {
        var (isExist, unit) = player.GetUnit(coord);
        if (isExist) return unit;        
      }
      log.Error($"Dict does not have coord: {coord}");
      throw new Exception();
    }

    readonly Player[] players;
    static readonly Logger log = MainLog.GetLogger(nameof(ModelContext));
  }
}