using UnityEngine;
public class DebugDetection : MonoBehaviour
{
    void Start()
    {
        #if UNITY_EDITOR
            EnableObject(true);
        #else
            if (Debug.isDebugBuild)
            {
                EnableObject(true);
            }
            else
            {
                EnableObject(false);
            }
        #endif
    }
    private void EnableObject(bool isEnabled)
    {
        gameObject.SetActive(isEnabled);
    }
}