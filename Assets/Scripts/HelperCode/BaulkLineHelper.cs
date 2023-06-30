using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaulkLineHelper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Coin>())
        {
            other.GetComponent<Coin>().UpdateBaulkLine(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Coin>())
        {
            other.GetComponent<Coin>().UpdateBaulkLine(false);
        }
    }
}
