using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    private object gridObject;

    protected virtual void Update()
    {
        text.text = gridObject.ToString();
    }
    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
        text.text = gridObject.ToString();
    }
}
