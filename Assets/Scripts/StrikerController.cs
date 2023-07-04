using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class StrikerController : MonoBehaviour
{

    //striker shooting is controlled by game manager


    PostShotRuleEvaluator ruleEvaluator;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnCollisionExit(Collision other)
    {
        var coin = other.gameObject.GetComponent<Coin>();

        if (coin != null)
        {
            var report = new ShotReport(this.gameObject, coin.gameObject, Time.time, coin.IsInBaulkLine);

        }
    }
}
