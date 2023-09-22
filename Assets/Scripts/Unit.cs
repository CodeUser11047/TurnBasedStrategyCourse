using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static event EventHandler OnAnyActionPointChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;


    [SerializeField] private bool isEnemy;
    [SerializeField] private int maxActionPoint = 2;
    private GridPosition gridPosition;
    private BaseAction[] baseActionArry;
    private HealthSystem healthSystem;
    private int actionPoint = 2;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActionArry = GetComponents<BaseAction>();
    }

    private void Start()
    {
        actionPoint = maxActionPoint;

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArry)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorlPosition()
    {
        return transform.position;
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

    private void HealthSystem_OnDead(object sender, Transform e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public void Damage(int damageAmount, Transform damageSourceTransform)
    {
        healthSystem.Damage(damageAmount, damageSourceTransform);
    }

    public float GetHealthNormailzed()
    {
        return healthSystem.GetHealthNormalized();
    }
}
