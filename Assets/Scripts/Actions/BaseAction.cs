using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponentInChildren<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    public virtual bool IsVaildActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> vaildGridPositionList = GetValidActionGridPositionList();
        return vaildGridPositionList.Contains(gridPosition);
    }
    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
    }
}
