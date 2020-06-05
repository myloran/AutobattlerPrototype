using System.Collections.Generic;
using Shared;
using UnityEngine;
using View.Exts;
using View.Presenters;

namespace View {
  public class UnitViewFactory : IUnitViewFactory {
    public UnitViewFactory(Dictionary<string, UnitInfo> unitInfos, UnitView unitPrefab, TilePresenter tilePresenter) {
      this.unitInfos = unitInfos;
      this.unitPrefab = unitPrefab;
      this.tilePresenter = tilePresenter;
    }

    public UnitView Create(string name, Coord coord, EPlayer player) {
      var position = tilePresenter.PositionAt(coord).WithY(unitPrefab.Height);
      var unit = Object.Instantiate(unitPrefab, position, Quaternion.identity);
      unit.Info = new UnitInfo(unitInfos[name]);
      unit.Player = player;
      return unit;
    }

    readonly TilePresenter tilePresenter;
    readonly UnitView unitPrefab;
    readonly Dictionary<string, UnitInfo> unitInfos;
  }
}