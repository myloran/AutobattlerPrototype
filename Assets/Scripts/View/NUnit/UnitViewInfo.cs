using UnityEngine;

namespace Shared.Shared.Client {
  [CreateAssetMenu(fileName = "UnitViewInfo", menuName = "Data/SharedClient/UnitViewInfo", order = 1)]
  public class UnitViewInfo : ScriptableObject {
    public GameObject Model;
    public RuntimeAnimatorController AnimatorController;
  }
}