using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;

    private void Start()
    {
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;
        UpdateActionPointsText();
    }
    private void Unit_OnAnyActionPointChanged(object sender, EventArgs e)
    {
        actionPointsText.text = "剩余行动点：" + unit.GetActionPoints().ToString();
    }
    private void UpdateActionPointsText()
    {
        actionPointsText.text = "剩余行动点：" + unit.GetActionPoints().ToString();
    }
}
