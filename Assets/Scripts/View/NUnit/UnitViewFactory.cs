using System.Collections.Generic;
using Addons.Assets.src.Scripts;
using Shared.Primitives;
using UnityEngine;
using View.Exts;
using View.NTile;

namespace View.NUnit {
  public class UnitViewFactory {
    public UnitViewFactory(Dictionary<string, UnitInfo> unitInfos, UnitViewInfoHolder unitViewInfoHolder, 
        CoordFinder coordFinder, Camera mainCamera) {
      this.unitInfos = unitInfos;
      this.unitViewInfoHolder = unitViewInfoHolder;
      this.coordFinder = coordFinder;
      this.mainCamera = mainCamera;
    }

    public UnitView Create(string name, Coord coord, EPlayer player) {
      var unit = unitViewInfoHolder.Prefab;
      var position = coordFinder.PositionAt(coord).WithY(unit.Height);
      var rotation = player.ToQuaternion();
      var unitInfo = unitInfos[name];
      
      var obj = Object.Instantiate(unit, position, rotation);
      var model = unitViewInfoHolder.Infos[name].Model;
      Object.Instantiate(model, obj.transform);
      
      var healthBar = obj.GetComponentInChildren<HealthBar>()
        .Init(player.ToColor(), unitInfo.Health, mainCamera);
      
      var manaBar = obj.GetComponentInChildren<ManaBar>()
        .Init(100, mainCamera);
       
      return obj.Init(new UnitInfo(unitInfo), player, healthBar, manaBar);
    }

    readonly CoordFinder coordFinder;
    readonly Camera mainCamera;
    readonly UnitViewInfoHolder unitViewInfoHolder;
    readonly Dictionary<string, UnitInfo> unitInfos;
  }
}