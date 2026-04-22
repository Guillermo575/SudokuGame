using UnityEngine;
using UnityEngine.UI;
public class HUDButtonPanel : MonoBehaviour
{
    public HUDButtonNumber buttonPrefab;
    public Transform parentTransform;
    public void Initialize(int numberOfButtons)
    {
        if (buttonPrefab == null || parentTransform == null)
        {
            Debug.LogError("Por favor asigna el prefab del botón y el parent en el inspector.");
            return;
        }
        for (int i = 0; i < numberOfButtons; i++)
        {
            MakeButton(i + 1);
        }
        MakeButton(0);
    }
    public void MakeButton(int Index)
    {
        GameObject buttonObj = Instantiate(buttonPrefab.gameObject, parentTransform);
        buttonObj.name = "Button_" + Index;
        HUDButtonNumber btn = buttonObj.GetComponent<HUDButtonNumber>();
        if (btn != null)
        {
            btn.Initialize(Index);
        }
    }
}