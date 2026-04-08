using UnityEngine;
public class SudokuSubBoard : MonoBehaviour
{
    public GameObject mainBoardPrefab;
    private SudokuNumberNode[,] nodes;
    public int nodesPerSide = 3;
    public float nodeSpacing = 11f;
    public void Initialize(SudokuBoard sudokuBoard)
    {
        var SizeBoard = sudokuBoard.sizeNumberNode + sudokuBoard.spaceBetweenSubBoards;
        mainBoardPrefab.transform.localScale = new Vector3(sudokuBoard.sizeNumberNode * sudokuBoard.numberRows, 1f, sudokuBoard.sizeNumberNode * sudokuBoard.numberColumns);
        nodes = new SudokuNumberNode[sudokuBoard.numberRows, sudokuBoard.numberColumns];
        for (int i = 0; i < sudokuBoard.numberRows; i++)
        {
            for (int j = 0; j < sudokuBoard.numberColumns; j++)
            {
                Vector3 position = new Vector3(sudokuBoard.spaceBetweenNodes + (j * (sudokuBoard.sizeNumberNode + sudokuBoard.spaceBetweenNodes)), 0.4f, -sudokuBoard.spaceBetweenNodes + (-i * (sudokuBoard.sizeNumberNode + sudokuBoard.spaceBetweenNodes)));
                GameObject subBoardObj = Instantiate(sudokuBoard.numberNodePrefab, Vector3.zero, Quaternion.identity, this.transform);
                subBoardObj.transform.localPosition = position;
                subBoardObj.name = $"Node_{j}_{i}";
                SudokuNumberNode subBoard = subBoardObj.GetComponent<SudokuNumberNode>();
                if (subBoard == null)
                {
                    subBoard = subBoardObj.AddComponent<SudokuNumberNode>();
                }
                subBoard.Initialize();
                nodes[j, i] = subBoard;
            }
        }
    }
}