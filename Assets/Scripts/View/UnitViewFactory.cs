using System.Collections.Generic;
using Shared;
using UnityEngine;

namespace View {
  public class UnitViewFactory : IUnitViewFactory {
    public UnitViewFactory(Dictionary<string, UnitInfo> unitInfos, UnitView unitPrefab) {
      this.unitInfos = unitInfos;
      this.unitPrefab = unitPrefab;
    }

    public UnitView Create(string name, Vector3 position, TileView tile, EPlayer player) {
      var unit = Object.Instantiate(unitPrefab, position.WithY(unitPrefab.Height), Quaternion.identity);
      unit.Info = unitInfos[name];
      unit.Tile = tile;
      unit.Player = player;
      tile.Unit = unit;
      return unit;
    }

    readonly UnitView unitPrefab;
    readonly Dictionary<string, UnitInfo> unitInfos;
  }
}