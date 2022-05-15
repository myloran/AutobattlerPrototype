using Addons.Assets.src.Scripts;
using Shared.Abstraction;
using Shared.Primitives;
using UnityEngine;
using View.Exts;
using View.NTile;
using View.NUnit.UI;

namespace View.NUnit {
  public class UnitViewFactory {
    public UnitViewFactory(IInfoGetter<UnitInfo> unitInfoGetter, UnitViewInfoHolder unitViewInfoHolder, 
        CoordFinder coordFinder, Camera mainCamera) {
      this.unitInfoGetter = unitInfoGetter;
      this.unitViewInfoHolder = unitViewInfoHolder;
      this.coordFinder = coordFinder;
      this.mainCamera = mainCamera;
    }

    public UnitView Create(string name, Coord coord, EPlayer player) {
      var unit = unitViewInfoHolder.Prefab;
      var position = coordFinder.PositionAt(coord).WithY(unit.Height);
      var rotation = player.ToQuaternion();
      var unitInfo = unitInfoGetter.Infos[name];
      
      var obj = Object.Instantiate(unit, position, rotation);
      var viewInfo = unitViewInfoHolder.Infos[name];
      Object.Instantiate(viewInfo.UnitModel, obj.transform);
      
      var healthBar = obj.GetComponentInChildren<HealthBar>()
        .Init(player.ToColor(), unitInfo.Health, mainCamera);
      
      var manaBar = obj.GetComponentInChildren<ManaBar>()
        .Init(100, mainCamera);
      
      var silenceCross = obj.GetComponentInChildren<SilenceCross>()
        .Init(mainCamera);
      
      obj.name = name;
      obj.Animator = obj.GetComponentInChildren<Animator>();
      obj.Animator.runtimeAnimatorController = viewInfo.AnimatorController;
      obj.Animator.applyRootMotion = false;
       
      return obj.Init(new UnitInfo(unitInfo), player, healthBar, manaBar, silenceCross, viewInfo.ProjectileModel);
    }

    readonly CoordFinder coordFinder;
    readonly Camera mainCamera;
    readonly UnitViewInfoHolder unitViewInfoHolder;
    readonly IInfoGetter<UnitInfo> unitInfoGetter;
  }
}