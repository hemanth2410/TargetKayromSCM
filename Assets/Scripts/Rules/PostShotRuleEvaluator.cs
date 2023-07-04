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
    /// <summary>
    /// Resets the dictionary, it is adviced to reset dictionary for every shot
    /// </summary>
    public void ClearShotDictionary()
    {
        shotsDict.Clear();
    }
    /// <summary>
    /// sets the current faction
    /// </summary>
    /// <param name="faction">set the value here based on payer's turn</param>
    public void SetFaction(CoinType faction)
    {
        currentFaction = faction;
    }
    /// <summary>
    /// Adds the collision report to the dictionary to process
    /// </summary>
    /// <param name="reportingObject">Game object that is reporting collision</param>
    /// <param name="shot">Shot object(After math of collision)</param>
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
    /// <summary>
    /// This method evaluates the history of shots.
    /// </summary>
    /// <returns>bool = should the turn be scored, bool = should faction get the turn, 1,1 == Score and turn,\n 1,0 == score, no turn,\n 0,1 == reset,\n 0,0 == no score, no turn </returns>
    public (bool,bool) EvaluateEvents()
    {
        bool score = true;
        bool turn = true;
        foreach(KeyValuePair<GameObject, List<ShotReport>>  kvp in shotsDict)
        {
            // Check if striker collided with other faction? If yes, Then make it instantly illegal
            if(kvp.Key.GetComponent<Coin>())
            {
                if(kvp.Key.GetComponent<Coin>().CoinType == CoinType.Striker)
                {
                    // Now look for first coin typeObject.
                    foreach(ShotReport shot in kvp.Value)
                    {
                        if(shot.CollidedWith.GetComponent<Coin>())
                        {
                            if (shot.CollidedWith.GetComponent<Coin>().CoinType == currentFaction) //Legal move
                            {
                                score = true;
                            }

                            else if (shot.CollidedWith.GetComponent<Coin>().CoinType != currentFaction && shot.CollidedWith.GetComponent<Coin>().CoinType != CoinType.Queen) // Illegal move
                            {
                                score = false;
                                turn = true;
                                // trigger reset
                            }
                            else
                                continue;
                        }
                        
                    }
                }
            }
            // Check if Coin of same faction went in? If yes, then make it legal
            // Check if queen went into goal post when score is not maximum? If yes, Then make it instantly illegal
            // Check if coin is in baulk like and went into goal post? If yes, Make it instantly illegal
            // Check if striker went in directly? If yes, Make it instantly illegal
            if (kvp.Key.GetComponent<Goal>())
            {
                foreach(ShotReport shot in kvp.Value)
                {
                    if(shot.CollidedWith.GetComponent<Coin>() && shot.CollidedWith.GetComponent<Coin>().CoinType != CoinType.Striker)
                    {
                        // check if coin is already inside the baulk like before hitting it into goal
                        // check if coin is of same faction type when going into goal post
                        ShotReport _finalReport = shotsDict[shot.CollidedWith].Last();
                        if(_finalReport.BaulkTrigger)
                        {
                            score &= false;
                            turn &= true;
                            // this should trigger reset
                        }
                        else if(shot.CollidedWith.GetComponent<Coin>().CoinType == currentFaction)
                        {
                            score &= true;
                            turn &= true;
                            // Perfectly legal move turn + score.
                        }
                        else if(shot.CollidedWith.GetComponent<Coin>().CoinType == CoinType.Queen) // we need a score manager for check
                        {
                            score &= true;
                            turn &= false;
                            // triggers the end of the game
                        }
                    }
                    if (shot.CollidedWith.GetComponent<Coin>() && shot.CollidedWith.GetComponent<Coin>().CoinType == CoinType.Striker)
                    {
                        score &= false;
                        turn &= false;
                        // No score, no turn
                    }
                }
            }
        }
        return (score,turn);
    }


}

public class ShotReport
{
    float eventTime;
    GameObject collidedObject;
    GameObject collidedWith;
    bool baulkTrigger;
    /// <summary>
    /// Time at which the collision event happened
    /// </summary>
    public float EventTime { get { return eventTime; } }
    /// <summary>
    /// The object that raised the collision event, Can be Striker, Coin or goalPost
    /// </summary>
    public GameObject CollidedObject { get { return collidedObject; } }
    /// <summary>
    /// The object that is collided with Coin, wall
    /// </summary>
    public GameObject CollidedWith { get { return collidedWith; } }
    /// <summary>
    /// A flag to see if the coin is already in the baulk line before going into goal post, Only set it to true if Striker hits something in baulk line
    /// </summary>
    public bool BaulkTrigger { get {  return baulkTrigger; } }
    /// <summary>
    /// This is a constructor for ShotReport class.
    /// </summary>
    /// <param name="collidedObject">Self, Can be goal post or striker or coin</param>
    /// <param name="collidedWith">Should be either a coin or wall</param>
    /// <param name="eventTime">Time when the collision happened</param>
    /// <param name="isInBaulkLine">A flag to check if the coin is in baulk line when collision happened</param>
    public ShotReport(GameObject collidedObject, GameObject collidedWith, float eventTime, bool isInBaulkLine)
    {
        this.eventTime = eventTime;
        this.collidedObject = collidedObject;
        this.collidedWith = collidedWith;
        this.baulkTrigger = isInBaulkLine;
    }
}
