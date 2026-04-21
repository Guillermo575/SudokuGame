using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HUDButtonNumber : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    private int indexValue;
    private GameManager gameManager;
    public void Initialize(int indexValue)
    {
        this.indexValue = indexValue;
        numberText.text = indexValue.ToString();
        gameManager = GameManager.GetSingleton();
    }
    public void OnButtonClicked()
    {
        if (gameManager == null) return;
        gameManager.setCellSelectedValue(indexValue);
    }
}