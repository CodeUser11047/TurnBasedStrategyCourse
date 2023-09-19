using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private float cellsize;
    private GridObject[,] gridObjectArry;

    /// <summary>
    /// new GridSystem的一种重载方法
    /// </summary>
    /// <param name="width">横向格子数量</param>
    /// <param name="height">纵向格子数量</param>
    /// <param name="cellsize">格子大小</param>
    public GridSystem(int width, int height, float cellsize)
    {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;

        gridObjectArry = new GridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArry[x, z] = new GridObject(this, gridPosition);
            }
        }
    }
    /// <summary>
    /// 根据当前gridPosition（二维）返回到世界坐标
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellsize;
    }
    /// <summary>
    /// 根据当前世界坐标返回GetGridPosition
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition
        (
         Mathf.RoundToInt(worldPosition.x / cellsize),
         Mathf.RoundToInt(worldPosition.z / cellsize)
        );
    }
    /// <summary>
    /// 在每个GridObject的位置上创建调试用的预制体
    /// </summary>
    /// <param name="debugPrefab"></param>
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);

                GridDebugPrefab gridDebugObject = debugTransform.GetComponent<GridDebugPrefab>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }
    /// <summary>
    /// 根据当前gridPosition索引GetGridObject的二维数组对应的GridObject
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArry[gridPosition.x, gridPosition.z];
    }

    /// <summary>
    /// 判断格子位置是否是在格子系统内
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
          gridPosition.z >= 0 &&
          gridPosition.x < width &&
          gridPosition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}
