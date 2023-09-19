using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static event EventHandler OnAnyActionPointChanged;

    [SerializeField] private bool isEnemy;
    [SerializeField] private int maxActionPoint = 2;
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArry;
    private int actionPoint = 2;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArry = GetComponents<BaseAction>();
    }

    private void Start()
    {
        actionPoint = maxActionPoint;

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorlPosition()
    {
        return transform.position;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public BaseAction[] GetBaseActionArry()
    {
        return baseActionArry;
    }

    public int GetActionPoints()
    {
        return actionPoint;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanTakeActionPointsToTakeAction(baseAction))
        {
            SpendActionPoint(baseAction.GetActionPointsCost());
            return true;
        }
        return false;
    }

    public bool CanTakeActionPointsToTakeAction(BaseAction baseAction)
    {
        if (actionPoint >= baseAction.GetActionPointsCost())
        {

            return true;
        }
        return false;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    private void SpendActionPoint(int amount)
    {
        actionPoint -= amount;
        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
        (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoint = maxActionPoint;
            OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Damage()
    {
        Debug.Log(transform + "受到攻击");
    }
}
