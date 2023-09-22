using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootUnit;
    }


    private enum State
    {
        Aiming,
        Shooting,
        Idel
    }

    [SerializeField] private int maxShootDistance = 7;
    [SerializeField] private LayerMask obstaclesLayerMask;
    private State state = State.Idel;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShotBullet;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                float rotateSpeed = 10f;
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.Shooting:
                if (canShotBullet)
                {
                    Shoot();
                    canShotBullet = false;
                }
                break;
            case State.Idel:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }
    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                if (stateTimer <= 0f)
                {
                    state = State.Shooting;
                    float shootingStateTimer = .1f;
                    stateTimer = shootingStateTimer;
                }
                break;
            case State.Shooting:
                if (stateTimer <= 0f)
                {
                    state = State.Idel;
                    float coolOffStateTime = .5f;
                    stateTimer = coolOffStateTime;
                }
                break;
            case State.Idel:
                if (stateTimer <= 0f)
                {
                    //行动结束调用
                    ActionComplete();
                }
                break;
        }

        // Debug.Log(state);
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootUnit = unit
        });
        targetUnit.Damage(40, transform);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }
    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validActionGridPositionList = new();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //当坐标不在当前坐标范围时跳过当前不跳过循环，每个行动类都应包括
                    continue;
                }
                //对角线位置检测射击距离
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance) { continue; }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //当坐标无其他单位时跳过
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue;
                }

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight,
                        shootDir,
                        Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                        obstaclesLayerMask))
                {
                    // 被遮挡
                    continue;
                }


                validActionGridPositionList.Add(testGridPosition);
            }
        }
        //返回有敌人并且在范围内的GridPosition
        return validActionGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);

        // Debug.Log("瞄准");
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShotBullet = true;

        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "射击";
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormailzed()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

}

