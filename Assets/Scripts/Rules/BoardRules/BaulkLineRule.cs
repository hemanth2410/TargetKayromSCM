using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaulkLineRule : MonoBehaviour, IBoardRule
{
    public bool ExecuteRule(Vector3 strikerPosition, Vector3 strikerForceDirection, CoinType f, LayerMask layerMask)
    {
        float _distanceToGoal = Mathf.Infinity;
        float _distanceToCoin = Mathf.Infinity;
        Coin baulkCoin = null;
        // Requires position, direction of the ray. && needs to know if this raycast is intersecting with goal post
        RaycastHit[] hits = Physics.RaycastAll(strikerPosition, strikerForceDirection, 100.0f, layerMask);
        if (hits.Length > 0)
        {

            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.GetComponent<Goal>())
                {
                    _distanceToGoal = Vector3.Distance(strikerPosition, hit.point);
                }
                if(hit.collider.GetComponent<Coin>())
                {
                    baulkCoin = hit.collider.GetComponent<Coin>();
                    _distanceToCoin = Vector3.Distance(strikerPosition, hit.point);
                }
            }
        }
        if(baulkCoin != null && baulkCoin.IsInBaulkLine)
        {
            if(_distanceToCoin < _distanceToGoal)
            {
                return false;
            }
        }
        return true;
    }
}
