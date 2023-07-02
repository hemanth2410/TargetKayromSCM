using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    List<GameObject> coins = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
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
            coins.Remove(other.gameObject);
            
            if(other.GetComponent<Coin>().CoinType != CoinType.Striker)
            {
                GameController.Instance.InvokeScoreEvent(other.GetComponent<Coin>().CoinType, 1);
                Destroy(other.gameObject);
            }
                
        }
    }
}
