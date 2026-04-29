using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDLogControl : MonoBehaviour
{
    public void OnBackClick()
    {
        GameManager.GetSingleton().LogBack();
    }
    public void OnForwardClick()
    {
        GameManager.GetSingleton().LogForward();
    }
    public void OnBackMaxClick()
    {
        GameManager.GetSingleton().LogBackMax();
    }
    public void OnForwardMaxClick()
    {
        GameManager.GetSingleton().LogForwardMax();
    }
}