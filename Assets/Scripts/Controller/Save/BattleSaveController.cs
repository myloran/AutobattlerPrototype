using System;
using System.Collections.Generic;
using System.Linq;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Exts;
using Shared.Poco;
using View.Presenters;
using View.UIs;

namespace Controller.Save {
  public class BattleSaveController : IDisposable {
    public BattleSaveController(PlayerContext playerContext, PlayerPresenterContext playerPresenterContext,
      BattleSaveUI ui, SaveInfoLoader saveInfoLoader, Dictionary<string, SaveInfo> saves) {
      this.playerContext = playerContext;
      this.playerPresenterContext = playerPresenterContext;
      this.ui = ui;
      this.saveInfoLoader = saveInfoLoader;
      this.saves = saves;
      ui.BAdd.onClick.AddListener(Save);
      ui.BLoad.onClick.AddListener(Load);
      ui.BLoadPrevious.onClick.AddListener(LoadPrevious);
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
    }

    Dictionary<Coord, string> GetUnits(Dictionary<Coord, IUnit> dict) => dict
      .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Name);

    void Load() {
      playerContext.DestroyAll();
      playerPresenterContext.DestroyAll();

      var save = saves[ui.GetSelectedSaveName];
      
      foreach (var (coord, name) in save.Player1BenchUnits) {
        playerContext.InstantiateToBench(name, coord, EPlayer.First);
        playerPresenterContext.InstantiateToBench(name, coord, EPlayer.First);
      }
      foreach (var (coord, name) in save.Player2BenchUnits) {
        playerContext.InstantiateToBench(name, coord, EPlayer.Second);
        playerPresenterContext.InstantiateToBench(name, coord, EPlayer.Second);
      }
      foreach (var (coord, name) in save.Player1BoardUnits) {
        playerContext.InstantiateToBoard(name, coord, EPlayer.First);
        playerPresenterContext.InstantiateToBoard(name, coord, EPlayer.First);
      }
      foreach (var (coord, name) in save.Player2BoardUnits) {
        playerContext.InstantiateToBoard(name, coord, EPlayer.Second);
        playerPresenterContext.InstantiateToBoard(name, coord, EPlayer.Second);
      }
    }
    
    void LoadPrevious() { }

    public void Dispose() {
      ui.BAdd.onClick.RemoveListener(Save);
      ui.BLoad.onClick.RemoveListener(Load);
      ui.BLoadPrevious.onClick.RemoveListener(LoadPrevious);
    }

    readonly BattleSaveUI ui;
    readonly SaveInfoLoader saveInfoLoader;
    readonly Dictionary<string, SaveInfo> saves;
    readonly PlayerContext playerContext;
    readonly PlayerPresenterContext playerPresenterContext;
  }
}