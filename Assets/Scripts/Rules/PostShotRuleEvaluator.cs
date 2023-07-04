using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PostShotRuleEvaluator : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class ShotReport
{
    float eventTime;
    GameObject collidedObject;

    public float EventTime { get { return eventTime; } }
    public GameObject CollidedObject { get {  return collidedObject; } }

    public ShotReport(GameObject collidedObject, float eventTime)
    {
        this.eventTime = eventTime;
        this.collidedObject = collidedObject;
    }
}
