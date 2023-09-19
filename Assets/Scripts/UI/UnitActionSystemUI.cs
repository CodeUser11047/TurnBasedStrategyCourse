using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform acctionButtonPrefab;
    [SerializeField] private Transform acctionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;
        UpdateUnitActionButtons();
        UpdateSelectedVisual();
    }
    private void UpdateUnitActionButtons()
    {
        //销毁所有在容器下的物体
        foreach (Transform buttonTransform in acctionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }
        actionButtonUIList.Clear();
        //调用当前选用的Unit
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        //遍历当前Unit中BaseAction类的个数来生成具体按钮数量
        if (selectedUnit != null)
        {
            foreach (BaseAction baseAction in selectedUnit.GetBaseActionArry())
            {
                Transform acctionButtonTransform = Instantiate(acctionButtonPrefab, acctionButtonContainerTransform);
                ActionButtonUI actionButtonUI = acctionButtonTransform.GetComponent<ActionButtonUI>();
                //设置每个按钮的行为以及对应名称
                actionButtonUI.SetBaseAction(baseAction);

                actionButtonUIList.Add(actionButtonUI);
            }
        }
    }
    /// <summary>
    /// 更新单位选择时的技能效果及数量
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        UpdateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }
    /// <summary>
    /// 更新选择效果
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();

    }
    /// <summary>
    /// 更新行动点数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
    /// <summary>
    /// 更新行动点数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointText.text = "Can Spend Action Points : " + selectedUnit.GetActionPoints();
    }


}
