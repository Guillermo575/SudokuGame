using UnityEngine;
public class SudokuBoard : MonoBehaviour
{
    #region Prefabs
    public GameObject mainBoardPrefab;
    public GameObject subBoardPrefab;
    public GameObject numberNodePrefab;
    #endregion

    #region Public
    public float spaceBetweenSubBoards = 0.05f;
    public float spaceBetweenNodes = 0.025f;
    public float sizeNumberNode = 1f;
    public int numberColumns = 3;
    public int numberRows = 3;
    #endregion

    private SudokuSubBoard[,] subBoards;
    public void CreateBoard()
    {
        subBoards = new SudokuSubBoard[numberColumns, numberRows];
        for (int i = 0; i < numberRows; i++)
        {
            for (int j = 0; j < numberColumns; j++)
            {
                GameObject subBoardObj = Instantiate(subBoardPrefab, Vector3.zero, Quaternion.identity, this.transform);
                subBoardObj.name = $"SubBoard_{j}_{i}";
                SudokuSubBoard subBoard = subBoardObj.GetComponent<SudokuSubBoard>();
                if (subBoard == null)
                {
                    subBoard = subBoardObj.AddComponent<SudokuSubBoard>();
                }
                subBoard.Initialize(this);
                subBoards[j, i] = subBoard;
            }
        }
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        var SizeNumber = sizeNumberNode + spaceBetweenNodes;
        var SizeBoard = numberColumns * numberRows * SizeNumber + (spaceBetweenSubBoards * 4);
        mainBoardPrefab.transform.localScale = new Vector3(SizeBoard, 1f, SizeBoard);
        for (int i = 0; i < numberRows; i++)
        {
            for (int j = 0; j < numberColumns; j++)
            {
                SudokuSubBoard subBoardObj = subBoards[j, i];
                Vector3 position = new Vector3(spaceBetweenSubBoards + (j * ((SizeNumber * numberRows) + spaceBetweenSubBoards)), 0.2f, -spaceBetweenSubBoards + (-i * ((SizeNumber * numberColumns) + spaceBetweenSubBoards)));
                subBoardObj.gameObject.transform.localPosition = position;
                subBoardObj.UpdatePosition();
            }
        }
    }
    private void Update()
    {
        UpdatePosition();
    }
}