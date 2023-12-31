using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;


    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float stopDistance = .1f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private int maxMoveDistance = 4;
    private List<Vector3> positionList;
    private int currentPositionIndex;


    // private Vector3 targetPosition;

    protected override void Awake()
    {
        // targetPosition = transform.position;

        base.Awake();
    }

    private void Update()
    {
        MoveMent();
    }

    public override void TakeAction(GridPosition targetGridPosition, Action onActionComplete)
    {
        // this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), targetGridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    private void MoveMent()
    {
        if (!isActive) { return; }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDir = (targetPosition - transform.position).normalized;


        transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        if (Vector3.Distance(targetPosition, transform.position) > stopDistance)
        {
            transform.position += moveSpeed * moveDir * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                //行动结束调用
                ActionComplete();
            }
        }
    }
    /// <summary>
    /// 返回可以移动到的所有格子位置的列表
    /// </summary>
    /// <returns></returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new();

        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    //当坐标不在当前坐标范围时跳过当前不跳过循环
                    continue;
                }
                if (unitGridPosition == testGridPosition)
                {
                    //当坐标在当前坐标时跳过
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //当坐标有其他单位时跳过
                    continue;
                }

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }

                int pathfindingDistanceMultiplier = 10;
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
                {
                    // Path length is too long
                    continue;
                }

                validActionGridPositionList.Add(testGridPosition);

            }
        }

        return validActionGridPositionList;
    }

    public override string GetActionName()
    {
        return "移动";
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,

        };
    }
}
