using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantPlayerData : MonoBehaviour
{
    private static PersistantPlayerData instance;

    public static PersistantPlayerData Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<PersistantPlayerData>();
            }
            return instance;
        }
    }
    public Player Player1 { get { return player1; } }
    public Player Player2 { get { return player2; } }

    Player player1;
    Player player2;

    private void Start()
    {
        loadSceneAditive(1);
    }
    public void RegisterPlayer1(Player player)
    {
        player1 = player;
    }
    public void RegisterPlayer2(Player player)
    {
        player2 = player;
    }

    void loadSceneAditive(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Additive);
    }
}

public class Player
{
    public string PlayerName { get { return playerName; } }
    public int SelectedIndex { get { return  selectedIndex; } }
    public CoinType PlayerFaction { get { return playerFaction; } }
    public bool PlayerWon { get { return playerWon; } }
    private string playerName;
    private int selectedIndex;
    private CoinType playerFaction;
    private bool playerWon;


    public Player(string playerName, CoinType playerFaction, int selectedIndex)
    {
        this.playerName = playerName; this.playerFaction = playerFaction; this.selectedIndex = selectedIndex;
    }

    public void setPlayerState(bool winState)
    {
        playerWon = winState;
    }
}
