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
        var SizeBoard = numberColumns * numberRows * (sizeNumberNode + spaceBetweenSubBoards);
        mainBoardPrefab.transform.localScale = new Vector3(SizeBoard, 1f, SizeBoard);
        subBoards = new SudokuSubBoard[numberColumns, numberRows];
        for (int i = 0; i < numberRows; i++)
        {
            for (int j = 0; j < numberColumns; j++)
            {
                Vector3 position = new Vector3(spaceBetweenSubBoards + (j * (sizeNumberNode + spaceBetweenSubBoards) * numberColumns), 0.2f, -spaceBetweenSubBoards + (-i * (sizeNumberNode + spaceBetweenSubBoards) * numberRows));
                GameObject subBoardObj = Instantiate(subBoardPrefab, Vector3.zero, Quaternion.identity, this.transform);
                subBoardObj.transform.localPosition = position;
                subBoardObj.name = $"SubBoard_{j}_{i}";
                //subBoardObj.transform.localScale = new Vector3(sizeNumberNode * numberRows, 1f, sizeNumberNode * numberColumns);
                SudokuSubBoard subBoard = subBoardObj.GetComponent<SudokuSubBoard>();
                if (subBoard == null)
                {
                    subBoard = subBoardObj.AddComponent<SudokuSubBoard>();
                }
                subBoard.Initialize(this);
                subBoards[j, i] = subBoard;
            }
        }
    }
    private void Start()
    {
        CreateBoard();
    }
}