using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FactionScore : MonoBehaviour
{
    [SerializeField] string m_factionName;
    [SerializeField] CoinType m_coinType;
    [SerializeField] Color FactionColor;
    [SerializeField] TextMeshProUGUI m_factionNameText;
    [SerializeField] Transform m_coinHolder;
    [SerializeField] GameObject m_coinGameObject;
    [SerializeField] TextMeshProUGUI m_scoreText;
    List<GameObject> m_coins = new List<GameObject>();
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_factionNameText.text = m_factionName;
        GameController.Instance.OnFactionScored += Instance_OnFactionScored;
    }

    public int getScore()
    {
        return score;
    }

    private void Instance_OnFactionScored(CoinType faction, int _score)
    {
        if(faction == m_coinType)
        {
            score += _score;
            if(_score > 0)
            {
                var k = Instantiate(m_coinGameObject, m_coinHolder);
                k.GetComponent<Image>().color = FactionColor;
                m_coins.Add(k);
            }
            else
            {
                Destroy(m_coins.First());
            }
            m_scoreText.text = "Score : " + score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
