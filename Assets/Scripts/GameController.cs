using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private RuleEvaluator ruleEvaluator;
    private GameManager gameManager;


    static GameController instance;

    public static GameController Instance { get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameController>();
            }
            return instance;
        } }
    
    public RuleEvaluator RuleEvaluator { get { return ruleEvaluator; } }
    public GameManager GameManager { get {  return gameManager; } }

    public event Action<CoinType, int> OnFactionScored;
    public event Action ResetPhysicsCoins;
    public void RegisterGameManager(GameManager manager)
    {
        gameManager = manager;
    }
    public void RegisterRuleEvaluator(RuleEvaluator Evaluator)
    {
        ruleEvaluator = Evaluator;
    }
    public void InvokeScoreEvent(CoinType faction, int value)
    {
        OnFactionScored?.Invoke(faction, value);
    }
    public void InvokeResetCoinPhysics()
    {
        ResetPhysicsCoins?.Invoke();
    }
}
