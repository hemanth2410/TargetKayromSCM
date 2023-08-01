using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarromLoader : MonoBehaviour
{
    [SerializeField] Transform m_SpawnPoint1;
    [SerializeField] Transform m_SpawnPoint2;
    [SerializeField] NewFactionHolder m_NewFactionHolder1;
    [SerializeField] NewFactionHolder m_NewFactionHolder2;
    [SerializeField] GameObject m_BasePrefab;
    [SerializeField] Sprite[] ProfilePictures;
    // Start is called before the first frame update
    void Start()
    {
        
        // loading player 1
        var player1 = Instantiate(m_BasePrefab, m_SpawnPoint1);
        player1.GetComponent<AvatarSwitcher>().loadAvatar(PersistantPlayerData.Instance.Player1.SelectedIndex);
        var player2 = Instantiate(m_BasePrefab,m_SpawnPoint2);
        player2.GetComponent<AvatarSwitcher>().loadAvatar(PersistantPlayerData.Instance.Player2.SelectedIndex);
        m_NewFactionHolder1.InitializeFaction(PersistantPlayerData.Instance.Player1.PlayerName, ProfilePictures[PersistantPlayerData.Instance.Player1.SelectedIndex],PersistantPlayerData.Instance.Player1.PlayerFaction);
        m_NewFactionHolder2.InitializeFaction(PersistantPlayerData.Instance.Player2.PlayerName, ProfilePictures[PersistantPlayerData.Instance.Player2.SelectedIndex], PersistantPlayerData.Instance.Player2.PlayerFaction);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
