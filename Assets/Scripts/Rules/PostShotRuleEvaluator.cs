using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PostShotRuleEvaluator : MonoBehaviour
{
    List<ShotReport> shots;
    CoinType currentFaction;
    Dictionary<GameObject, List<ShotReport>> shotsDict = new Dictionary<GameObject, List<ShotReport>>();
    //public List<ShotReport> Shots { get {  return shots; } }

    public void ClearShotDictionary()
    {
        shotsDict.Clear();
    }
    public void SetFaction(CoinType faction)
    {
        currentFaction = faction;
    }
    public void AppendShotReport(GameObject reportingObject, ShotReport shot)
    {
        if (shotsDict.ContainsKey(reportingObject))
        {
            List<ShotReport> _tempShots = shotsDict[reportingObject];
            _tempShots.Add(shot);
            shotsDict[reportingObject] = _tempShots;
        }
        else
        {
            List<ShotReport> _newShots = new List<ShotReport>();
            _newShots.Add(shot);
            shotsDict[reportingObject] = _newShots;
        }
    }

    public bool EvaluateEvents()
    {
        foreach(KeyValuePair<GameObject, List<ShotReport>>  kvp in shotsDict)
        {
            // Check if striker collided with other faction? If yes, Then make it instantly illegal

            // Check if queen went into goal post when score is not maximum? If yes, Then make it instantly illegal

            // Check if coin is in baulk like and went into goal post? If yes, Make it instantly illegal

            // Check if striker went in directly? If yes, Make it instantly illegal

            // Check if Coin of same faction went in? If yes, then make it legal
        }
        return false;
    }
}

public class ShotReport
{
    float eventTime;
    GameObject collidedObject;
    GameObject collidedWith;

    public float EventTime { get { return eventTime; } }
    public GameObject CollidedObject { get { return collidedObject; } }
    public GameObject CollidedWith { get { return collidedWith; } }

    public ShotReport(GameObject collidedObject, GameObject collidedWith, float eventTime)
    {
        this.eventTime = eventTime;
        this.collidedObject = collidedObject;
        this.collidedWith = collidedWith;
    }
}
