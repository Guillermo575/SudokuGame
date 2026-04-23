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
}