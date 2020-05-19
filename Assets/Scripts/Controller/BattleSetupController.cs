using System;
using System.Collections.Generic;
using Model;
using Shared;
using View;
using View.UI;

namespace Controller {
  public class BattleSetupController : IDisposable {
    public BattleSetupController(Player[] players, BenchView[] benches, 
      Dictionary<string, UnitInfo> unitInfos, BattleSetupUI battleSetupUI) {
      this.players = players;
      this.benches = benches;
      this.unitInfos = unitInfos;
      this.battleSetupUI = battleSetupUI;
      battleSetupUI.BAdd.onClick.AddListener(AddUnit);
      battleSetupUI.BRemove.onClick.AddListener(RemoveUnit);
    }

    void AddUnit() {
      var unit = new Unit {Info = unitInfos[battleSetupUI.GetSelectedUnitName], Level = 1};
      var id = battleSetupUI.GetSelectedPlayerId;
      var (isAdded, coord) = benches[id].AddUnit(battleSetupUI.GetSelectedUnitName);
      if (isAdded) players[id].AddUnit(unit, coord); 
    }
    
    void RemoveUnit() {
      var id = battleSetupUI.GetSelectedPlayerId;
      var (isRemoved, coord) = benches[id].RemoveUnit();
      if (isRemoved) players[id].RemoveUnit(coord);
    }

    public void Dispose() {
      battleSetupUI.BAdd.onClick.RemoveListener(AddUnit);
      battleSetupUI.BRemove.onClick.RemoveListener(RemoveUnit);
    }

    readonly BattleSetupUI battleSetupUI;
    readonly BenchView[] benches;
    readonly Dictionary<string, UnitInfo> unitInfos;
    readonly Player[] players;
  }
}