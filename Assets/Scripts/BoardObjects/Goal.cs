using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    List<GameObject> coins = new List<GameObject>();

    GameManager gameManager;
    PostShotRuleEvaluator ruleEvaluator;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag(Constants.Tag_GameManager).GetComponent<GameManager>();  
        ruleEvaluator = gameManager.GetComponent<PostShotRuleEvaluator>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Coin>() != null && !coins.Contains(other.gameObject))
        {
            coins.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(coins.Contains(other.gameObject))
        {
            var coinScript = other.GetComponent<Coin>();

            coins.Remove(other.gameObject);

            ruleEvaluator.AppendShotReport(this.gameObject, new ShotReport(this.gameObject, other.gameObject, Time.time, coinScript.IsInBaulkLine));

            if(other.GetComponent<Coin>().CoinType != CoinType.Striker)
            {
                gameManager.CoinPucked(other.gameObject);
            }
                
        }
    }
}
