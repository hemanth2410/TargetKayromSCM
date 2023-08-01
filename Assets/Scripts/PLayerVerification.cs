using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerVerification : MonoBehaviour
{
    Player player1;
    Player player2;

    [SerializeField] AvatarSwitcher m_Loser;
    [SerializeField] AvatarSwitcher m_Winner;
    // Start is called before the first frame update
    void Start()
    {
        player1 = PersistantPlayerData.Instance.Player1;
        player2 = PersistantPlayerData.Instance.Player2;
        if(player1.PlayerWon)
        {
            m_Loser.loadAvatar(player2.SelectedIndex);
            m_Winner.loadAvatar(player1.SelectedIndex);
        }
        else
        {
            m_Loser.loadAvatar(player1.SelectedIndex);
            m_Winner.loadAvatar(player2.SelectedIndex);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
