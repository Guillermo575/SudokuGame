using UnityEngine;
public class SudokuSubBoard : MonoBehaviour
{
    public GameObject mainBoardPrefab;
    public SudokuNumberCell[,] nodes { get; set; }
}