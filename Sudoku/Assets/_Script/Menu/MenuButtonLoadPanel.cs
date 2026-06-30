using UnityEngine;
public class MenuButtonLoadPanel : MonoBehaviour
{
    public GameObject panelContainer;
    public MenuButtonLoad menuButtonLoadPrefab;
    private void OnEnable()
    {
        Initialize();
    }
    public void Initialize()
    {
        SavePlayerPref savePlayerPref = SavePlayerPref.GetSingleton();
        if (savePlayerPref == null) return;
        Transform[] children = panelContainer.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child != panelContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
        //if (saveGameSO.lastGameState != null)
        //{
        //    GameObject menuButtonLoad = Instantiate(menuButtonLoadPrefab.gameObject, Vector3.zero, Quaternion.identity, panelContainer.transform);
        //    menuButtonLoad.name = $"menuButtonLoad_Last";
        //    menuButtonLoad.GetComponent<MenuButtonLoad>().Initialize(saveGameSO.lastGameState);
        //}
        var lstGames = savePlayerPref.GetAllGames();
        for (int l = 0; l < lstGames.Count; l++)
        {
            var obj = lstGames[l];
            GameObject menuButtonLoad = Instantiate(menuButtonLoadPrefab.gameObject, Vector3.zero, Quaternion.identity, panelContainer.transform);
            menuButtonLoad.name = $"menuButtonLoad_{l}";
            menuButtonLoad.GetComponent<MenuButtonLoad>().Initialize(obj);
        }
    }
}