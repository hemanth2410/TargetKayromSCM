using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCoinCheck : MonoBehaviour, IBoardRule
{
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool ExecuteRule(Vector3 strikerPosition, Vector3 strikerForceDirection, CoinType f, LayerMask layerMask)
    {
        RaycastHit hit;
        if(Physics.Raycast(strikerPosition, strikerForceDirection, out hit, 100.0f, layerMask))
        {
            if(hit.collider.GetComponent<Coin>().CoinType == f)
            {
                return true;
            }
        }
        return false;
    }
}
