using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Addons.Assets.src.Scripts {
  public class ManaBar : MonoBehaviour {
    public ManaBar Init(float maxHealth, Camera mainCamera) {
      this.mainCamera = mainCamera;
      manaBar = transform.Find("Mana").GetComponent<Image>();
      this.maxHealth = maxHealth;
      return this;
    }

    void Update() => UpdateRotation();

    void UpdateRotation() {
      var position = transform.position;
      var targetPosition = mainCamera.transform.position; 
      var target = new Vector3(position.x, -targetPosition.y, -targetPosition.z);
      transform.LookAt(target);
    }
    
    public void SetCurrentMana(float amount) {
      tween?.Kill();
      var healthPercentage = Mathf.Clamp01(amount / maxHealth);
      tween = manaBar.DOFillAmount(healthPercentage, 1).SetEase(Ease.OutExpo);
    }

    Camera mainCamera;
    Image manaBar;
    float maxHealth;
    TweenerCore<float, float, FloatOptions> tween;
  }
}