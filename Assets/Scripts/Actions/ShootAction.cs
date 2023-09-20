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
                Vector3 aimDir = (targetUnit.GetWorlPosition() - unit.GetWorlPosition()).normalized;

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
        List<GridPosition> validActionGridPositionList = new();

        GridPosition unitGridPosition = unit.GetGridPosition();
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

                validActionGridPositionList.Add(testGridPosition);
            }
        }

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

}

