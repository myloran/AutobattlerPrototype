using System;
using System.Collections.Generic;
using System.Linq;
using Model.NBattleSimulation;
using Model.NUnit;
using Shared;
using View;
using View.UI;

namespace Controller.Save {
  public class BattleSaveController : IDisposable {
    public BattleSaveController(Player[] players, BenchView[] benches, BoardView board,
      BattleSaveUI ui, SaveInfoLoader saveInfoLoader, Dictionary<string, SaveInfo> saves) {
      this.players = players;
      this.benches = benches;
      this.board = board;
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
        Player1BenchUnits = GetUnits(players[0].BenchUnits),
        Player1BoardUnits = GetUnits(players[0].BoardUnits),
        Player2BenchUnits = GetUnits(players[1].BenchUnits),
        Player2BoardUnits = GetUnits(players[1].BoardUnits),
      };
      
      saveInfoLoader.Save(save);
    }

    Dictionary<Coord, string> GetUnits(Dictionary<Coord, Unit> dict) => dict
      .Select(pair => (pair.Key, pair.Value.Info.Name))
      .ToDictionary(kvp => kvp.Key, kvp => kvp.Name);

    void Load() {
      players[0].BenchUnits.Clear();
      players[1].BenchUnits.Clear();
      players[0].BoardUnits.Clear();
      players[1].BoardUnits.Clear();
      benches[0].Clear();
      benches[1].Clear();
      board.Clear();
      
      var save = saves[ui.GetSelectedSaveName];
      foreach (var unit in save.Player1BenchUnits) {
        players[0].AddBenchUnit(unit.Value, unit.Key, 0);
        benches[0].AddUnit(unit.Value, unit.Key);
      }
      foreach (var unit in save.Player2BenchUnits) {
        players[1].AddBenchUnit(unit.Value, unit.Key, 1);
        benches[1].AddUnit(unit.Value, unit.Key);
      }
      foreach (var unit in save.Player1BoardUnits) {
        players[0].AddBoardUnit(unit.Value, unit.Key, 0);
        board.AddUnit(unit.Value, unit.Key, EPlayer.First);
      }
      foreach (var unit in save.Player2BoardUnits) {
        players[1].AddBoardUnit(unit.Value, unit.Key, 1);
        board.AddUnit(unit.Value, unit.Key, EPlayer.Second);
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
    readonly BenchView[] benches;
    readonly BoardView board;
    readonly Player[] players;
  }
}