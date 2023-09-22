using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;

    private int gCost;//已走过的距离
    private int hCost;//期望距离
    private int fCost;//总距离
    private bool isWalkable = true;
    private PathNode cameFormPathNode;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {

        return gridPosition.ToString();
    }
    public int GetGCost()
    {
        return gCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
    public int GetFCost()
    {
        return fCost;
    }
    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }
    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public void RestCameFromNode()
    {
        cameFormPathNode = null;
    }

    public void SetCameFromNode(PathNode pathNode)
    {
        cameFormPathNode = pathNode;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public PathNode GetCameFromNode()
    {
        return cameFormPathNode;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetIsWalked(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
}
