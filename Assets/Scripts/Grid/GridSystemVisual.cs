using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    private GridSystemVisualSingle[,] gridSystemVisualSingleArry;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }
    private void Start()
    {
        gridSystemVisualSingleArry = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new(x, z);
                Transform grisystemVisualSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleArry[x, z] = grisystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArry[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArry[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        if (selectedAction != null)
            ShowGridPositionList(selectedAction.GetValidActionGridPositionList());

    }
}
