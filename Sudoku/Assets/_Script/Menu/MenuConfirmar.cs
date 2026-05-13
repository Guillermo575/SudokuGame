using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
/**
 * @class
 * @brief Menu que se abre para advertir al jugador si desea realizar alguna accion,
 * en caso de que el jugador seleccione que si se ejecutara un evento generico
 */
public class MenuConfirmar : _Menu
{
    #region Variables
    /** Mensaje que aparece en la ventana cambia el de default */
    public TMP_Text txtTitulo;
    /** Evento que reproduce la ventana en caso de confirmar que si */
    UnityEvent EventoConfirmar;
    #endregion

    #region Awake
    /** Inicializacion de los objetos */
    protected override void Start()
    {
        base.Start();
    }
    #endregion

    #region General
    public void MostrarPantallaConfirmar(UnityAction evt, String msg)
    {
        var menuConfirmar = MenuManager.GetSingleton().menuConfirmar;
        if (menuConfirmar != null)
        {
            UnityEvent objEvent = new UnityEvent();
            objEvent.AddListener(evt);
            menuConfirmar.OpenWindow(objEvent, msg);
        }
        else
        {
            evt();
        }
    }
    /**
     * Abre la ventana y agrega el evento generico
     * @param EventoConfirmar
     * @param titulo
     */
    public void OpenWindow(UnityEvent EventoConfirmar, string titulo = null)
    {
        menuManager = MenuManager.GetSingleton();
        menuManager.ShowMenu(this.gameObject);
        this.EventoConfirmar = EventoConfirmar;
        if (!string.IsNullOrEmpty(titulo))
        {
            txtTitulo.text = titulo;
        }
    }
    /** Evento que se activa en caso de seleccionar No, que seria regresar al menu anterior */
    public void ConfirmarNo()
    {
        menuManager.BackMenu();
    }
    /** Evento que se activa en caso de seleccionar Si, que seria regresar al menu anterior y activar el evento de confirmar */
    public void ConfirmarSi()
    {
        EventoConfirmar.Invoke();
        menuManager.BackMenu();
    }
    #endregion
}