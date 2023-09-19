using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : Singleton<UnitActionSystem>
{

    //事件
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;
    //参数
    [SerializeField] private Unit selectedUnit = null;
    [SerializeField] private LayerMask UnitsLayer;

    //组件
    private BaseAction selectedAction;

    private bool isBusy;

    private void Start()
    {
        // SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy) { return; }
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleMouseUnit())
        {
            return;
        }

        HandleSelectedAction();

    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    public bool TryHandleMouseUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, UnitsLayer))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        //检查为当前选择对象时返回false
                        return false;
                    }
                    if (unit.IsEnemy())
                    {
                        //检查为敌人时返回false
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }
    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMousePosition());

            if (!selectedAction.IsVaildActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);
            //以上等同于以下
            // if (selectedAction.IsVaildActionGridPosition(mouseGridPosition))
            // {
            //     if (selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            //     {
            //         SetBusy();
            //         selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            //     }
            // }
        }
    }

    private void SetSelectedUnit(Unit unit)
    {

        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);

    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

}
