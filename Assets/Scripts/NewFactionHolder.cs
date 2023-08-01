using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NewFactionHolder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_PlayerNameText;
    [SerializeField] Image m_ScoreImage;
    [SerializeField] Image m_ProfilePicture;
    CoinType currentPlayerFaction;
    int score;
    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.OnFactionScored += Instance_OnFactionScored;
    }

    private void Instance_OnFactionScored(CoinType arg1, int arg2)
    {
        if(currentPlayerFaction == arg1)
        {
            score = UnityEngine.Mathf.Clamp((score + arg2), 0, Constants.MaximumScore);
            AddScore();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int getScore()
    {
        return score;
    }
    public void InitializeFaction(string playerName, Sprite playerProfile, CoinType playerFaction)
    {
        m_PlayerNameText.text = playerName;
        m_ProfilePicture.sprite = playerProfile;
        currentPlayerFaction = playerFaction;
    }
    public void AddScore()
    {
        float _normalizedScore = (float)score / Constants.MaximumScore;
        m_ScoreImage.fillAmount = _normalizedScore;
    }
}
