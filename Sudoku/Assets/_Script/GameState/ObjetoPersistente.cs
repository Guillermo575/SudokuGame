using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public abstract class ObjetoPersistente : ScriptableObject
{
    public void Guardar(string NameFile = null)
    {
        var bf = new BinaryFormatter();
        var file = File.Create(ObtenerRuta(NameFile));
        var json = JsonUtility.ToJson(this);
        bf.Serialize(file, json);
        file.Close();
    }
    public virtual void Cargar(string NameFile = null)
    {
        if (File.Exists(ObtenerRuta(NameFile)))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(ObtenerRuta(NameFile), FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), this);
            file.Close();
        }
    }
    public string ObtenerRuta(string NameFile = null)
    {
        var nombreArchivoCompleto = string.IsNullOrEmpty(NameFile) ? name : NameFile;
        return string.Format("{0}/{1}.json", Application.persistentDataPath, nombreArchivoCompleto);
    }
}