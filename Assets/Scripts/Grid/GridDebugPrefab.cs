using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    private void Update()
    {
        text.text = gridObject.ToString();
    }
    private GridObject gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
        text.text = gridObject.ToString();
    }
}
