using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenCoinRule : MonoBehaviour, IBoardRule
{
    int currentScore = 11;
    int maximumScore = 12;
    public bool ExecuteRule(Vector3 strikerPosition, Vector3 strikerForceDirection, CoinType f, LayerMask layerMask)
    {
        // make sure that the score is maximum before scoring the red coin
        float _goalDistance = Mathf.Infinity;
        float _redDistance = Mathf.Infinity;
        RaycastHit[] hits = Physics.RaycastAll(strikerPosition, strikerForceDirection, 100.0f, layerMask);
        if (hits.Length != 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.GetComponent<Coin>() && hits[i].collider.GetComponent<Coin>().CoinType == CoinType.Striker)
                {
                    continue;
                }
                if (hits[i].collider.GetComponent<Coin>() && hits[i].collider.GetComponent<Coin>().CoinType == CoinType.Queen)
                {
                    _redDistance = Vector3.Distance(strikerPosition, hits[i].point);
                }
                if (hits[i].collider.GetComponent<Coin>() && hits[i].collider.GetComponent<Goal>())
                {
                    _goalDistance = Vector3.Distance(strikerPosition, hits[i].point);
                }
            }
        }

        if(currentScore == maximumScore)
        {
            return true;
        }
        if(currentScore < maximumScore)
        {
            // make hitting red illegal if player is aiming at red and goal Post
            if(_redDistance < _goalDistance) 
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}
