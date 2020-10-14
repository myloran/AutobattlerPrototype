using System.Collections.Generic;
using Shared.Primitives;
using UnityEngine;
using View.Exts;
using View.NTile;

namespace View.NUnit {
  public class UnitViewFactory {
    public UnitViewFactory(Dictionary<string, UnitInfo> unitInfos, UnitViewInfoHolder unitViewInfoHolder, 
        CoordFinder coordFinder, Camera mainCamera) {
      this.unitInfos = unitInfos;
      this._unitViewInfoHolder = unitViewInfoHolder;
      this.coordFinder = coordFinder;
      this.mainCamera = mainCamera;
    }

    public UnitView Create(string name, Coord coord, EPlayer player) {
      var unit = _unitViewInfoHolder.Prefab;
      var position = coordFinder.PositionAt(coord).WithY(unit.Height);
      var rotation = player.ToQuaternion();
      var unitInfo = unitInfos[name];
      
      var obj = Object.Instantiate(unit, position, rotation);
      var model = _unitViewInfoHolder.Infos[name].Model;
      Object.Instantiate(model, obj.transform);
      
      var healthBar = obj.GetComponentInChildren<HealthBar>()
        .Init(player.ToColor(), unitInfo.Health, mainCamera);
       
      return obj.Init(new UnitInfo(unitInfo), player, healthBar);
    }

    readonly CoordFinder coordFinder;
    readonly Camera mainCamera;
    readonly UnitViewInfoHolder _unitViewInfoHolder;
    readonly Dictionary<string, UnitInfo> unitInfos;
  }
}