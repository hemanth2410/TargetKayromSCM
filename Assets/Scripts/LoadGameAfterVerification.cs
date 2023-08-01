using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameAfterVerification : MonoBehaviour
{
    [SerializeField] ProfileRandomizer m_Player1;
    [SerializeField] ProfileRandomizer m_Player2;
    [SerializeField] Button m_GameButton;
    bool allOK;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_GameButton.interactable = allOK;
    }

    public void TriggerVerification()
    {
        allOK = (m_Player1.SelectedIndex != -1 && !string.IsNullOrEmpty(m_Player1.PlayerName)) && (m_Player2.SelectedIndex != -1 && !string.IsNullOrEmpty(m_Player2.PlayerName));

    }

    public async void LoadScene()
    {
        // register players here
        Player player1 = new Player(m_Player1.PlayerName, CoinType.Faction1, m_Player1.SelectedIndex);
        Player player2 = new Player(m_Player2.PlayerName, CoinType.Faction2, m_Player2.SelectedIndex);
        PersistantPlayerData.Instance.RegisterPlayer1(player1);
        PersistantPlayerData.Instance.RegisterPlayer2(player2);
        await SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        await SceneManager.UnloadSceneAsync(1);
    }

}
