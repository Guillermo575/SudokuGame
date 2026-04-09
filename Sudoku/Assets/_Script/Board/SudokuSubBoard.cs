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
        var SizeNode = sudokuBoard.sizeNumberNode + sudokuBoard.spaceBetweenNodes;
        mainBoardPrefab.transform.localScale = new Vector3(sudokuBoard.sizeNumberNode * sudokuBoard.numberRows, 1f, sudokuBoard.sizeNumberNode * sudokuBoard.numberColumns);
        nodes = new SudokuNumberNode[sudokuBoard.numberRows, sudokuBoard.numberColumns];
        for (int i = 0; i < sudokuBoard.numberColumns; i++)
        {
            for (int j = 0; j < sudokuBoard.numberRows; j++)
            {
                Vector3 position = new Vector3(sudokuBoard.spaceBetweenNodes + (j * SizeNode), 0.4f, -sudokuBoard.spaceBetweenNodes + (-i * SizeNode));
                GameObject NumberNode = Instantiate(sudokuBoard.numberNodePrefab, Vector3.zero, Quaternion.identity, this.transform);
                NumberNode.transform.localPosition = position;
                NumberNode.name = $"Node_{j}_{i}";
                SudokuNumberNode subBoard = NumberNode.GetComponent<SudokuNumberNode>();
                if (subBoard == null)
                {
                    subBoard = NumberNode.AddComponent<SudokuNumberNode>();
                }
                subBoard.Initialize();
                nodes[j, i] = subBoard;
            }
        }
    }
}