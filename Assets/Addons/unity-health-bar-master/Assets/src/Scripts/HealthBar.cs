using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
  public float DropSpeed = 0.5f;

  public void SetCurrentHealth(float currentHealth) => this.currentHealth = currentHealth;

  public HealthBar Init(Color color, float maxHealth, Camera mainCamera) {
    this.mainCamera = mainCamera;
    healthBar = transform.Find("Health").GetComponent<Image>();
    dropEffect = transform.Find("DropEffect").GetComponent<Image>();
    healthBar.color = color;
    this.maxHealth = maxHealth;
    currentHealth = maxHealth;
    return this;
  }

  void Update() {
    var healthPercentage = Mathf.Min(Mathf.Max(0, currentHealth / maxHealth), 1);
    healthBar.fillAmount = healthPercentage;

    HandleDropEffect(healthPercentage);
    UpdateRotation();
  }

  void UpdateRotation() {
    var position = transform.position;
    var targetPosition = mainCamera.transform.position; 
    var target = new Vector3(position.x, targetPosition.y, targetPosition.z);
    transform.LookAt(target);
  }

  void HandleDropEffect(float healthPercentage) {
    if (dropEffectPercentage > healthPercentage) {
      dropEffectPercentage -= Time.deltaTime * DropSpeed;
      dropEffect.fillAmount = dropEffectPercentage;
    }
    else {
      dropEffectPercentage = healthPercentage;
    }
  }

  Camera mainCamera;
  Image healthBar;
  Image dropEffect;
  float maxHealth;
  float currentHealth;
  float dropEffectPercentage = 1;
}