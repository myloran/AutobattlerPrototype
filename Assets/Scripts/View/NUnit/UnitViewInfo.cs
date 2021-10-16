using UnityEngine;

namespace Shared.Shared.Client {
  [CreateAssetMenu(fileName = "UnitViewInfo", menuName = "Data/SharedClient/UnitViewInfo", order = 1)]
  public class UnitViewInfo : ScriptableObject {
    public GameObject UnitModel;
    public GameObject ProjectileModel;
    public RuntimeAnimatorController AnimatorController;
  }
}