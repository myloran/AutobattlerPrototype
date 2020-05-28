using System;
using Model.NAI.Visitors;
using Model.NBattleSimulation;
using View;
using View.UI;

namespace Controller {
  public class BattleSetupController : IDisposable {
    public BattleSetupController(Player[] players, BenchView[] benches, 
      BattleSetupUI battleSetupUI, BattleSimulation simulation, ICommandVisitor visitor) {
      this.players = players;
      this.benches = benches;
      this.battleSetupUI = battleSetupUI;
      this.simulation = simulation;
      this.visitor = visitor;
      battleSetupUI.BAdd.onClick.AddListener(AddUnit);
      battleSetupUI.BRemove.onClick.AddListener(RemoveUnit);
      battleSetupUI.BStartBattle.onClick.AddListener(StartBattle);
    }

    void AddUnit() {
      var playerId = battleSetupUI.GetSelectedPlayerId;
      var name = battleSetupUI.GetSelectedUnitName;
      var (isAdded, coord) = benches[playerId].AddUnit(name);
      if (isAdded) players[playerId].AddBenchUnit(name, coord, playerId); 
    }
    
    void RemoveUnit() {
      var id = battleSetupUI.GetSelectedPlayerId;
      var (isRemoved, coord) = benches[id].RemoveUnit();
      if (isRemoved) players[id].RemoveBenchUnit(coord);
    }

    void StartBattle() {
      simulation.PrepareBattle();
      simulation.ExecuteNextDecision();
      simulation.Command.Accept(visitor);
    }

    public void Dispose() {
      battleSetupUI.BAdd.onClick.RemoveListener(AddUnit);
      battleSetupUI.BRemove.onClick.RemoveListener(RemoveUnit);
      battleSetupUI.BStartBattle.onClick.RemoveListener(StartBattle);
    }

    readonly BattleSetupUI battleSetupUI;
    readonly BattleSimulation simulation;
    readonly BenchView[] benches;
    readonly Player[] players;
    readonly ICommandVisitor visitor;
  }
}