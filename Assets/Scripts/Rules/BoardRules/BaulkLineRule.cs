using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaulkLineRule : MonoBehaviour, IBoardRule
{
    public bool ExecuteRule(Vector3 strikerPosition, Vector3 strikerForceDirection, CoinType f, LayerMask layerMask)
    {
        // Requires position, direction of the ray.
        RaycastHit hit;
        if(Physics.Raycast(strikerPosition, strikerForceDirection, out hit, 100.0f, layerMask))
        {
            if(!hit.collider.GetComponent<Coin>().IsInBaulkLine)
            {
                return true;
            }
        }

        return false;
    }
}
