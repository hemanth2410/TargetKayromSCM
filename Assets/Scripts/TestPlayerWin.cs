using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPlayerWin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void TestPlayerWinSimulate()
    {
        //PersistantPlayerData.Instance.Player1.setPlayerState(true);
        //PersistantPlayerData.Instance.Player2.setPlayerState(false);
        await SceneManager.UnloadSceneAsync(2);
        await SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
    }

    public async void LoadMenu()
    {
        await SceneManager.UnloadSceneAsync(3);
        await SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }
}
