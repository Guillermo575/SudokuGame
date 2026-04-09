using UnityEngine;
public class SudokuSubBoard : MonoBehaviour
{
    public GameObject mainBoardPrefab;
    private SudokuNumberNode[,] nodes;
    private SudokuBoard sudokuBoard;
    public void Initialize(SudokuBoard sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard;
        nodes = new SudokuNumberNode[sudokuBoard.numberRows, sudokuBoard.numberColumns];
        for (int i = 0; i < sudokuBoard.numberColumns; i++)
        {
            for (int j = 0; j < sudokuBoard.numberRows; j++)
            {
                GameObject NumberNode = Instantiate(sudokuBoard.numberNodePrefab, Vector3.zero, Quaternion.identity, this.transform);
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
                SudokuNumberNode subBoard = nodes[j, i];
                Vector3 position = new Vector3(sudokuBoard.spaceBetweenNodes + (j * SizeNode), 0.4f, -sudokuBoard.spaceBetweenNodes + (-i * SizeNode));
                subBoard.gameObject.transform.localPosition = position;
            }
        }
    }
}