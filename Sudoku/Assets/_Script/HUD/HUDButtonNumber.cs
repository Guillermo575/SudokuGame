using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HUDButtonNumber : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public int indexValue { get; private set; }
    private GameManager gameManager;
    public void Initialize(int indexValue)
    {
        this.indexValue = indexValue;
        numberText.text = Sudoku.Alphabet.getAlphaChar(indexValue);
        gameManager = GameManager.GetSingleton();
    }
    public void OnButtonClicked()
    {
        if (gameManager == null) return;
        gameManager.setCellSelectedValue(indexValue);
    }
}