using UnityEngine;
public class SudokuSubBoard : MonoBehaviour
{
    public GameObject mainBoardPrefab;
    private SudokuNumberCell[,] nodes;
    private SudokuBoard sudokuBoard;
    public void Initialize(SudokuBoard sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard;
        nodes = new SudokuNumberCell[sudokuBoard.numberRows, sudokuBoard.numberColumns];
        for (int i = 0; i < sudokuBoard.numberColumns; i++)
        {
            for (int j = 0; j < sudokuBoard.numberRows; j++)
            {
                GameObject NumberNode = Instantiate(sudokuBoard.numberNodePrefab, Vector3.zero, Quaternion.identity, this.transform);
                NumberNode.name = $"Node_{j}_{i}";
                SudokuNumberCell subBoardCell = NumberNode.GetComponent<SudokuNumberCell>();
                if (subBoardCell == null)
                {
                    subBoardCell = NumberNode.GetComponentInChildren<SudokuNumberCell>();
                    if (subBoardCell == null)
                    {
                        subBoardCell = NumberNode.AddComponent<SudokuNumberCell>();
                    }
                }
                subBoardCell.Initialize(sudokuBoard);
                nodes[j, i] = subBoardCell;
            }
        }
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        var SizeNode = sudokuBoard.sizeNumberNode + sudokuBoard.spaceBetweenNodes;
        mainBoardPrefab.transform.localScale = new Vector3(SizeNode * sudokuBoard.numberRows + sudokuBoard.spaceBetweenNodes, 1f, SizeNode * sudokuBoard.numberColumns + sudokuBoard.spaceBetweenNodes);
        for (int i = 0; i < sudokuBoard.numberColumns; i++)
        {
            for (int j = 0; j < sudokuBoard.numberRows; j++)
            {
                SudokuNumberCell subBoardCell = nodes[j, i];
                Vector3 position = new Vector3(sudokuBoard.spaceBetweenNodes + (j * SizeNode), 0.4f, -sudokuBoard.spaceBetweenNodes + (-i * SizeNode));
                subBoardCell.gameObject.transform.localPosition = position;
            }
        }
    }
}