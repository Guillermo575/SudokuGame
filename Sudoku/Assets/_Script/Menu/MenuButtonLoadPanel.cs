using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuButtonLoadPanel : MonoBehaviour
{
    public GameObject panelContainer;
    public MenuButtonLoad menuButtonLoadPrefab;
    public SaveGameSO saveGameSO;
	
    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (saveGameSO == null) return;
        Transform[] children = panelContainer.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child != panelContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
        if (saveGameSO.lastGameState != null)
        {
            GameObject menuButtonLoad = Instantiate(menuButtonLoadPrefab.gameObject, Vector3.zero, Quaternion.identity, panelContainer.transform);
            menuButtonLoad.name = $"menuButtonLoad_Last";
            menuButtonLoad.GetComponent<MenuButtonLoad>().Initialize(saveGameSO.lastGameState);
        }
        for (int l = 0; l < saveGameSO.lstGames.Count; l++)
        {
            var obj = saveGameSO.lstGames[l];
            GameObject menuButtonLoad = Instantiate(menuButtonLoadPrefab.gameObject, Vector3.zero, Quaternion.identity, panelContainer.transform);
            menuButtonLoad.name = $"menuButtonLoad_{l}";
            menuButtonLoad.GetComponent<MenuButtonLoad>().Initialize(obj);
        }
    }
}