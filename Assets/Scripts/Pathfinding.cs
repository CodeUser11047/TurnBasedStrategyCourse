using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// A*寻路
/// </summary>
public class Pathfinding : Singleton<Pathfinding>
{
    private const int MOVE_STRAIGHT_COST = 10;//直角边常量
    private const int MOVE_DIAGONAL_COST = 14;//斜边常量
    [SerializeField] private Transform debugPrefab;
    private int width;
    private int height;
    private float cellsize;
    private GridSystem<PathNode> gridSystem;

    protected override void Awake()
    {
        base.Awake();
        gridSystem = new GridSystem<PathNode>(10, 10, 2f,
             (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(debugPrefab);
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> openList = new();
        List<PathNode> closedList = new();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.RestCameFromNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }
                //获取到达相邻点将要获得的GCost
                int tentativeGCost =
                    currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        //无路返回空值
        return null;
    }
    /// <summary>
    /// 计算期望权重
    /// </summary>
    /// <param name="gridPositionA">起点</param>
    /// <param name="gridPositionB">终点</param>
    /// <returns></returns>
    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;

        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        //返回MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance)部分为斜边个数路径，MOVE_STRAIGHT_COST * remaining;为直边个数路径
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    /// <summary>
    /// 获取最小总权重
    /// </summary>
    /// <param name="pathNodeList">openlist所有可移动点</param>
    /// <returns></returns>
    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }
    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    /// <summary>
    /// 获取当前Node的周边Node
    /// </summary>
    /// <param name="currentNode">当前Node</param>
    /// <returns></returns>
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourNodeList = new();

        GridPosition gridPosition = currentNode.GetGridPosition();
        if (gridPosition.x - 1 >= 0)
        {
            //左边的
            neighbourNodeList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
                //左下的
                neighbourNodeList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            if (gridPosition.z + 1 < gridSystem.GetHeight())
                //左上的
                neighbourNodeList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
        }
        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //右边的
            neighbourNodeList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
                //右下的
                neighbourNodeList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            if (gridPosition.z + 1 < gridSystem.GetHeight())
                //右上的
                neighbourNodeList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
        }
        if (gridPosition.z - 1 >= 0)
            //下边的
            neighbourNodeList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        if (gridPosition.z + 1 < gridSystem.GetHeight())
            //上边的
            neighbourNodeList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));

        return neighbourNodeList;
    }
    /// <summary>
    /// 返回最短的可行路径
    /// </summary>
    /// <param name="endNode">终点</param>
    /// <returns></returns>
    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new()
        {
            endNode
        };
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromNode());
            currentNode = currentNode.GetCameFromNode();
        }
        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }
        return gridPositionList;
    }
}
