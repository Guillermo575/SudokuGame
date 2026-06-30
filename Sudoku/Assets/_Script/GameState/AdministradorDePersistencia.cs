using System.Collections.Generic;
using UnityEngine;
public class AdministradorDePersistencia : MonoBehaviour
{
    public List<ObjetoPersistente> ObjetosAGuardar;
    public void OnEnable()
    {
        for (int i = 0; i < ObjetosAGuardar.Count; i++)
        {
            var so = ObjetosAGuardar[i];
            so.Cargar();
        }
    }
    public void OnDisable()
    {
        for (int i = 0; i < ObjetosAGuardar.Count; i++)
        {
            var so = ObjetosAGuardar[i];
            so.Guardar();
        }
    }
}