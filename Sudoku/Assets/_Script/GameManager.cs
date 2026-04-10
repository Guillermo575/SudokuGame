using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SudokuBoard sudokuBoard;
    public CamaraController controller;
    private void Start()
    {
        sudokuBoard.CreateBoard();
        controller.InitiateCamera();
    }
}
