using System.Collections.Generic;
using Shared;
using UnityEngine;
using View.Exts;
using View.NUnit;
using View.Presenters;

namespace View.Factories {
  public class UnitViewFactory {
    public UnitViewFactory(Dictionary<string, UnitInfo> unitInfos, UnitView unitPrefab, TilePresenter tilePresenter) {
      this.unitInfos = unitInfos;
      this.unitPrefab = unitPrefab;
      this.tilePresenter = tilePresenter;
    }

    public UnitView Create(string name, Coord coord, EPlayer player) {
      var position = tilePresenter.PositionAt(coord).WithY(unitPrefab.Height);

      var rotation = player == EPlayer.First
        ? Quaternion.identity
        : Quaternion.Euler(new Vector3(0, 180, 0));
      
      return Object.Instantiate(unitPrefab, position, rotation)
        .Init(new UnitInfo(unitInfos[name]), player);
    }

    readonly TilePresenter tilePresenter;
    readonly UnitView unitPrefab;
    readonly Dictionary<string, UnitInfo> unitInfos;
  }
}