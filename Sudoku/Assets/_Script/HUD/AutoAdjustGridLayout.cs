using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoAdjustGridLayout : MonoBehaviour
{
    public int columnsLandscape = 4;
    public int columnsPortrait = 2;
    private GridLayoutGroup gridLayout;
    void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        AdjustGrid();
    }
    void Update()
    {
        if (gridLayout == null) return;
        AdjustGrid();
    }
    void AdjustGrid()
    {
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = Screen.width > Screen.height ? columnsLandscape : columnsPortrait;
    }
}