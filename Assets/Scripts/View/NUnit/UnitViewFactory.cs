using System.Collections.Generic;
using Shared;
using Shared.Poco;
using UnityEngine;
using View.Exts;
using View.NTile;

namespace View.NUnit {
  public class UnitViewFactory {
    public UnitViewFactory(Dictionary<string, UnitInfo> unitInfos, UnitView unitPrefab, 
        CoordFinder coordFinder, Camera mainCamera) {
      this.unitInfos = unitInfos;
      this.unitPrefab = unitPrefab;
      this.coordFinder = coordFinder;
      this.mainCamera = mainCamera;
    }

    public UnitView Create(string name, Coord coord, EPlayer player) {
      var position = coordFinder.PositionAt(coord).WithY(unitPrefab.Height);
      var rotation = player.ToQuaternion();
      var unitInfo = unitInfos[name];
      
      var obj = Object.Instantiate(unitPrefab, position, rotation);
      
      var healthBar = obj.GetComponentInChildren<HealthBar>()
        .Init(player.ToColor(), unitInfo.Health, mainCamera);
       
      return obj.Init(new UnitInfo(unitInfo), player, healthBar);
    }

    readonly CoordFinder coordFinder;
    readonly Camera mainCamera;
    readonly UnitView unitPrefab;
    readonly Dictionary<string, UnitInfo> unitInfos;
  }
}