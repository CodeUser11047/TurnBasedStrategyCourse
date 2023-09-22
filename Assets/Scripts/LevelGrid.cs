using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : Singleton<LevelGrid>
{
    public event EventHandler OnAnyUnitMovedGridPosition;
    [SerializeField] private int height = 10;
    [SerializeField] private int width = 10;
    [SerializeField] private float cellsize = 2f;
    [SerializeField] private Transform debugPrefab;

    private GridSystem<GridObject> gridSystem;

    protected override void Awake()
    {
        base.Awake();

        gridSystem = new GridSystem<GridObject>(width, height, cellsize,
             (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //gridSystem.CreateDebugObjects(debugPrefab);

    }

    private void Start()
    {
        Pathfinding.Instance.SetUp(width, height, cellsize);
    }

    /// <summary>
    /// 将Unit附加到所在gridPosition的Gridobject上
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="unit"></param>
    public void AddUnitAtPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    /// <summary>
    /// 获取所在gridPosition的Unit
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public List<Unit> GetUnitAtPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    /// <summary>
    /// 用于清除所在gridPosition的Unit
    /// </summary>
    /// <param name="gridPosition"></param>
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    /// <summary>
    /// 用于Unit移动后调用的函数
    /// </summary>
    /// <param name="unit">当前Unit</param>
    /// <param name="fromGridPosition">之前所在的GridPosition</param>
    /// <param name="toGridPosition">当前所在的GridPosition</param>
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtPosition(toGridPosition, unit);

        OnAnyUnitMovedGridPosition.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public int GetWidth() => gridSystem.GetWidth();
    public int GetHeight() => gridSystem.GetHeight();
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }


}
