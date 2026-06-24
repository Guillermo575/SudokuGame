using UnityEngine;
/**
 * @class
 * @brief Clase padre que guarda las caracteristicas principales del menu
 */
public class _Menu : MonoBehaviour
{
    #region Variables
    /** @hidden*/ internal MenuManager menuManager;
    #endregion

    #region Start
    /** Inicializacion de los objetos */
    internal virtual void Start()
    {
        menuManager = MenuManager.GetSingleton();
    }
    #endregion
}