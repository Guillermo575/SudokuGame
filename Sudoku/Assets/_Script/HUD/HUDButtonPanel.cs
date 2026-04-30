using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class HUDButtonPanel : MonoBehaviour
{
    #region Public
    public HUDButtonNumber buttonPrefab;
    public Transform parentTransform;
    #endregion

    #region Private
    List<HUDButtonNumber> lstHUDButtonNumber;
    #endregion

    public void Initialize(int numberOfButtons, int activeButtons)
    {
        if (buttonPrefab == null || parentTransform == null)
        {
            Debug.LogError("Por favor asigna el prefab del botón y el parent en el inspector.");
            return;
        }
        lstHUDButtonNumber = new List<HUDButtonNumber>();
        for (int i = 0; i < numberOfButtons; i++)
        {
            var objNew = MakeButton(i + 1);
            if (i >= activeButtons)
            {
                objNew.gameObject.SetActive(false);
            }
            lstHUDButtonNumber.Add(objNew);
        }
        MakeButton(0);
    }
    public HUDButtonNumber MakeButton(int Index)
    {
        GameObject buttonObj = Instantiate(buttonPrefab.gameObject, parentTransform);
        buttonObj.name = "Button_" + Index;
        HUDButtonNumber btn = buttonObj.GetComponent<HUDButtonNumber>();
        if (btn != null)
        {
            btn.Initialize(Index);
        }
        return btn;
    }
    public void HideShowButtons(int activeButtons)
    {
        var lstActive = (from x in lstHUDButtonNumber where x.indexValue <=activeButtons select x).ToList();
        var lstHide = (from x in lstHUDButtonNumber where x.indexValue > activeButtons select x).ToList();
        foreach (var obj in lstActive)
        {
            obj.gameObject.SetActive(true);
        }
        foreach (var obj in lstHide)
        {
            obj.gameObject.SetActive(false);
        }
    }
}