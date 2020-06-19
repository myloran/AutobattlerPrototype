using System;
using System.Collections.Generic;
using System.Linq;
using Model.NBattleSimulation;
using Model.NUnit;
using Shared;
using View;
using View.Presenters;
using View.UIs;

namespace Controller.Save {
  public class BattleSaveController : IDisposable {
    public BattleSaveController(Player[] players, PlayerPresenter[] playerPresenters,
      BattleSaveUI ui, SaveInfoLoader saveInfoLoader, Dictionary<string, SaveInfo> saves) {
      this.players = players;
      this.playerPresenters = playerPresenters;
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
        Player1BenchUnits = GetUnits(players[0].BenchUnits.Units),
        Player1BoardUnits = GetUnits(players[0].BoardUnits.Units),
        Player2BenchUnits = GetUnits(players[1].BenchUnits.Units),
        Player2BoardUnits = GetUnits(players[1].BoardUnits.Units),
      };
      
      saveInfoLoader.Save(save);
    }

    Dictionary<Coord, string> GetUnits(Dictionary<Coord, Unit> dict) => dict
      .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Name);

    void Load() {
      foreach (var player in players) {
        player.BenchUnits.DestroyAll();
        player.BoardUnits.DestroyAll();
      }
      foreach (var player in playerPresenters) {
        player.BenchUnits.DestroyAll();
        player.BoardUnits.DestroyAll();
      }
      
      var save = saves[ui.GetSelectedSaveName];
      
      foreach (var (coord, name) in save.Player1BenchUnits) {
        players[0].BenchUnits.Instantiate(name, coord, EPlayer.First);
        playerPresenters[0].BenchUnits.Instantiate(name, coord, EPlayer.First);
      }
      foreach (var (coord, name) in save.Player2BenchUnits) {
        players[1].BenchUnits.Instantiate(name, coord, EPlayer.Second);
        playerPresenters[1].BenchUnits.Instantiate(name, coord, EPlayer.Second);
      }
      foreach (var (coord, name) in save.Player1BoardUnits) {
        players[0].BoardUnits.Instantiate(name, coord, EPlayer.First);
        playerPresenters[0].BoardUnits.Instantiate(name, coord, EPlayer.First);
      }
      foreach (var (coord, name) in save.Player2BoardUnits) {
        players[1].BoardUnits.Instantiate(name, coord, EPlayer.Second);
        playerPresenters[1].BoardUnits.Instantiate(name, coord, EPlayer.Second);
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
    readonly Player[] players;
    readonly PlayerPresenter[] playerPresenters;
  }
}