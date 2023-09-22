using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathfindingDebugObjcet : GridDebugPrefab
{
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;
    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        pathNode = (PathNode)gridObject;
        base.SetGridObject(gridObject);
    }

    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
    }
}
