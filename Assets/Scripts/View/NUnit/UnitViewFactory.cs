using System.Collections.Generic;
using Shared;
using Shared.Poco;
using UnityEngine;
using View.Exts;
using View.NTile;

namespace View.NUnit {
  public class UnitViewFactory {
    public UnitViewFactory(Dictionary<string, UnitInfo> unitInfos, UnitView unitPrefab, 
        TilePresenter tilePresenter, Camera mainCamera) {
      this.unitInfos = unitInfos;
      this.unitPrefab = unitPrefab;
      this.tilePresenter = tilePresenter;
      this.mainCamera = mainCamera;
    }

    public UnitView Create(string name, Coord coord, EPlayer player) {
      var position = tilePresenter.PositionAt(coord).WithY(unitPrefab.Height);
      var rotation = player.ToQuaternion();
      
      return Object.Instantiate(unitPrefab, position, rotation)
        .Init(new UnitInfo(unitInfos[name]), player, mainCamera);
    }

    readonly TilePresenter tilePresenter;
    readonly Camera mainCamera;
    readonly UnitView unitPrefab;
    readonly Dictionary<string, UnitInfo> unitInfos;
  }
}