using System.Collections.Generic;
using System.Linq;
using Controller.Exts;
using Controller.NDebug;
using Model.NUnit.Abstraction;
using Shared.Exts;
using Shared.Primitives;
using UnityEngine;
using View.UIs;

namespace Controller.Save {
  public class BattleSaveController {
    public BattleSaveController(PlayerSharedContext playerContext, 
      BattleSaveUI ui, SaveInfoLoader saveInfoLoader, Dictionary<string, SaveInfo> saves,
      BattleSimulationDebugController battleSimulationController) {
      this.playerContext = playerContext;
      this.ui = ui;
      this.saveInfoLoader = saveInfoLoader;
      this.saves = saves;
      this.battleSimulationController = battleSimulationController;
    }

    public void SubToUI() {
      ui.BAdd.Sub(Save);
      ui.BLoad.Sub(Load);
      // ui.BLoadPrevious.Sub(LoadPrevious);
    }

    void Save() {
      var save = new SaveInfo {
        Name = ui.SaveName,
        Player1BenchUnits = GetUnits(playerContext.GetBenchUnitDict(EPlayer.First)),
        Player1BoardUnits = GetUnits(playerContext.GetBoardUnitDict(EPlayer.First)),
        Player2BenchUnits = GetUnits(playerContext.GetBenchUnitDict(EPlayer.Second)),
        Player2BoardUnits = GetUnits(playerContext.GetBoardUnitDict(EPlayer.Second)),
      };
      
      saveInfoLoader.Save(save);
      //TODO: reload saves, so that if we save to an existing one, it's updated
    }

    Dictionary<Coord, string> GetUnits(Dictionary<Coord, IUnit> dict) => dict
      .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Name);

    void Load() {
      var timeScaleBefore = Time.timeScale;
      Time.timeScale = 0;
      
      playerContext.DestroyAll(); 

      var save = saves[ui.GetSelectedSaveName];
      
      foreach (var (coord, name) in save.Player1BenchUnits) {
        playerContext.InstantiateToBench(name, coord, EPlayer.First);
      }
      foreach (var (coord, name) in save.Player2BenchUnits) {
        playerContext.InstantiateToBench(name, coord, EPlayer.Second);
      }
      foreach (var (coord, name) in save.Player1BoardUnits) {
        playerContext.InstantiateToBoard(name, coord, EPlayer.First);
      }
      foreach (var (coord, name) in save.Player2BoardUnits) {
        playerContext.InstantiateToBoard(name, coord, EPlayer.Second);
      }
      battleSimulationController.ResetBattle();
      Time.timeScale = timeScaleBefore;
    }
    
    void LoadPrevious() { }

    readonly BattleSaveUI ui;
    readonly SaveInfoLoader saveInfoLoader;
    readonly Dictionary<string, SaveInfo> saves;
    readonly BattleSimulationDebugController battleSimulationController;
    readonly PlayerSharedContext playerContext;
  }
}