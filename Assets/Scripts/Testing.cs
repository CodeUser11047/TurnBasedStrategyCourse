using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private GridSystemVisual gridSystemVisual;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            gridSystemVisual.HideAllGridPosition();
        }
    }
}
