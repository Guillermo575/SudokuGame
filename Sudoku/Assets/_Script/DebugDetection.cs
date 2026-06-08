
using UnityEngine;

public class DebugDetection : MonoBehaviour
{
    void Start()
    {
        #if UNITY_EDITOR
            // Si estás en el editor, se considera modo debug
            EnableObject(true);
        #else
            // En construcciones, verificar si es una build de depuración
            if (Debug.isDebugBuild)
            {
                EnableObject(true);
            }
            else
            {
                // No es una build de debug, desactiva el objeto
                EnableObject(false);
            }
        #endif
    }

    private void EnableObject(bool isEnabled)
    {
        // Puedes desactivar el objeto completo o componentes específicos
        gameObject.SetActive(isEnabled);
    }
}