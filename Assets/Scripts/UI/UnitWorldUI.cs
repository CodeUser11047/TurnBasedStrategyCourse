using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Gradient healthBarGradient;


    private void Start()
    {
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealBar();
    }

    #region 事件函数
    private void Unit_OnAnyActionPointChanged(object sender, EventArgs e)
    {

        UpdateActionPointsText();
    }
    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealBar();
    }
    #endregion

    private void UpdateActionPointsText()
    {
        if (!unit.IsEnemy())
            actionPointsText.text = "剩余行动点：" + unit.GetActionPoints().ToString();
    }

    private void UpdateHealBar()
    {
        float fillAmount = healthBarImage.fillAmount;

        StopCoroutine(healthBarRoutine(fillAmount));
        StartCoroutine(healthBarRoutine(fillAmount));

        healthText.text = healthSystem.GetHealth().ToString();
    }

    private IEnumerator healthBarRoutine(float fillAmount)
    {
        float fillTimer = 0;

        while (healthBarImage.fillAmount != healthSystem.GetHealthNormalized())
        {
            fillTimer += Time.deltaTime;

            healthBarImage.fillAmount = Mathf.Lerp(fillAmount, healthSystem.GetHealthNormalized(), fillTimer * 3f);

            yield return null;
        }
        healthBarImage.color = healthBarGradient.Evaluate(healthBarImage.fillAmount);
    }
}
