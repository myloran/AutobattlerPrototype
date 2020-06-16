using System.Collections.Generic;
using Shared;
using UnityEngine;
using View.Exts;
using View.Presenters;
using View.Views;

namespace View.Factories {
  public class UnitViewFactory {
    public UnitViewFactory(Dictionary<string, UnitInfo> unitInfos, UnitView unitPrefab, TilePresenter tilePresenter) {
      this.unitInfos = unitInfos;
      this.unitPrefab = unitPrefab;
      this.tilePresenter = tilePresenter;
    }

    public UnitView Create(string name, Coord coord, EPlayer player) {
      var position = tilePresenter.PositionAt(coord).WithY(unitPrefab.Height);
      
      return Object.Instantiate(unitPrefab, position, Quaternion.identity)
        .Init(new UnitInfo(unitInfos[name]), player);
    }

    readonly TilePresenter tilePresenter;
    readonly UnitView unitPrefab;
    readonly Dictionary<string, UnitInfo> unitInfos;
  }
}