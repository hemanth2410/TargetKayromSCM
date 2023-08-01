using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PostShotRuleEvaluator : MonoBehaviour
{
    List<ShotReport> shots;
    CoinType currentFaction;
    Dictionary<GameObject, List<ShotReport>> shotsDict = new Dictionary<GameObject, List<ShotReport>>();
    [SerializeField] bool overrideTurn;
    [SerializeField] NewFactionHolder faction1;
    [SerializeField] NewFactionHolder faction2;
    List<GameObject> coins = new List<GameObject>();
    GameManager gameManager;
    private void Start()
    {
        GameController.Instance.RegisterPostShotEvaluator(this);
        gameManager = GameObject.FindGameObjectWithTag(Constants.Tag_GameManager).GetComponent<GameManager>();
    }

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

        Debug.Log(faction + " to play!");
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
            List<ShotReport> _newShots = new List<ShotReport>
            {
                shot
            };
            shotsDict[reportingObject] = _newShots;
        }
    }
    ///  <summary>
    ///  Evaluates the history of shots and returns a tuple of boolean flags representing the game state after the shot. The flags are:
    ///  shouldScore: Indicates if the player should score or not(true = score, false = no score).
    ///  shouldGetTurn: Indicates if the player should get the turn or not(true = get turn, false = no turn).
    ///  placeCarromMan: Indicates if the player should place a carrom man on the board(true = place carrom man, false = no placement).
    ///  extraTurn: Indicates if the player gets an extra turn(true = extra turn, false = no extra turn).
    ///  </summary>
    ///  <returns></returns>
    public (bool, bool, bool, bool, bool, GameObject) EvaluateRules()
    {
        int scoreToEvaluate = 0;
        bool score = false;
        bool placeCarromMan = false;
        bool extraTurn = false;
        bool queenReachedGoal = false;
        bool resetToPast = false;
        GameObject coinToPlace = null;
        switch (currentFaction)
        {
            case CoinType.Faction1:
                scoreToEvaluate = faction1.getScore();
                break;
            case CoinType.Faction2:
                scoreToEvaluate = faction2.getScore();
                break;
        }

        if (shotsDict.Count == 0)
        {
            score = false;
        }
        else
        {
            foreach (KeyValuePair<GameObject, List<ShotReport>> kvp in shotsDict)
            {
                // Check if striker collided with other faction? If yes, Then make it instantly illegal
                if (kvp.Key.GetComponent<Coin>())
                {
                    if (kvp.Key.GetComponent<Coin>().CoinType == CoinType.Striker)
                    {
                        // Now look for first coin typeObject.
                        foreach (ShotReport shot in kvp.Value)
                        {
                            if (shot.CollidedWith.GetComponent<Coin>())
                            {
                                if (shot.CollidedWith.GetComponent<Coin>().CoinType == currentFaction) //Legal move
                                {
                                    score = false;
                                }

                                if (shot.CollidedWith.GetComponent<Coin>().CoinType != currentFaction) // Illegal move
                                {
                                    score = false;
                                    placeCarromMan = true;
                                    coinToPlace = shot.CollidedWith.GetComponent<Coin>().gameObject;
                                }
                                if (shot.CollidedWith.GetComponent<Coin>().CoinType == CoinType.Queen)
                                {
                                    score = false;
                                    placeCarromMan = false;
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
                    foreach (ShotReport shot in kvp.Value)
                    {
                        if (shot.CollidedWith.GetComponent<Coin>() && shot.CollidedWith.GetComponent<Coin>().CoinType != CoinType.Striker)
                        {
                            // Check if the coin is in the baulk line when going into the goal post
                            ShotReport _finalReport = shotsDict[shot.CollidedWith].Last();
                            if (_finalReport.BaulkTrigger)
                            {
                                score = false;
                                // Check if the last collided object is the striker and the coin is already in the baulk line
                                if (_finalReport.CollidedWith.GetComponent<Coin>().CoinType == CoinType.Striker)
                                {
                                    placeCarromMan = true; // Illegal move, opponent gets the advantage of placing the carrom man
                                    coinToPlace = _finalReport.CollidedWith.GetComponent<Coin>().gameObject;
                                }
                                break;
                            }
                            else if (shot.CollidedWith.GetComponent<Coin>().CoinType == currentFaction)
                            {
                                score = true;
                                extraTurn = true;
                                // remove the coin here
                                // Perfectly legal move turn + score.
                            }
                            else if(shot.CollidedWith.GetComponent<Coin>().CoinType == currentFaction && shot.CollidedWith.GetComponent<Coin>().CoinType == CoinType.Queen)
                            {
                                coinToPlace = shot.CollidedWith.GetComponent<Coin>().gameObject;
                                placeCarromMan = true;
                            }
                            else if (shot.CollidedWith.GetComponent<Coin>().CoinType == CoinType.Queen && !shot.CollidedWith.GetComponent<RedCoinHelper>().WaitingForExit)
                            {
                                if (!shot.CollidedWith.GetComponent<RedCoinHelper>().WaitingForExit)
                                {
                                    switch (currentFaction)
                                    {
                                        case CoinType.Faction1:
                                            scoreToEvaluate = faction1.getScore();
                                            break;
                                        case CoinType.Faction2:
                                            scoreToEvaluate = faction2.getScore();
                                            break;
                                    }
                                    if (scoreToEvaluate == Constants.MaximumScore)
                                    {
                                        score = true;
                                        // ... (previous code, no changes)
                                        if (currentFaction == CoinType.Faction1)
                                        {
                                            PersistantPlayerData.Instance.Player1.setPlayerState(true);
                                        }
                                        else
                                        {
                                            PersistantPlayerData.Instance.Player2.setPlayerState(true);
                                        }
                                        GameController.Instance.InvokeGameOverEvent();
                                    }
                                    else
                                    {
                                        score = false;
                                        queenReachedGoal = true;
                                        resetToPast = true; // Trigger reset when Queen reaches goal post and score is not maximum.
                                    }
                                }
                            }
                        }
                    }
                }

                //// Check if the striker goes through the goal frame after a carrom man (coin)
                //// and set the 'placeCarromMan' flag to true.
                //if (kvp.Key.GetComponent<Coin>() && kvp.Key.GetComponent<Coin>().CoinType == CoinType.Striker)
                //{
                //    bool strikerThroughGoal = false;
                //    foreach (ShotReport shot in kvp.Value)
                //    {
                //        if (shot.CollidedWith.GetComponent<Coin>())
                //        {
                //            coinToPlace = shot.CollidedObject.GetComponent<Coin>().gameObject;
                //            strikerThroughGoal = true;
                //            continue;
                //        }

                //        if (strikerThroughGoal && shot.CollidedWith.GetComponent<Goal>())
                //        {
                //            placeCarromMan = true;
                //            break;
                //        }
                //    }
                //}

                //// Check if the striker goes through the goal frame without a carrom man preceding it
                //// and set the 'extraTurn' flag to true.
                //if (kvp.Key.GetComponent<Goal>())
                //{
                //    foreach (ShotReport shot in kvp.Value)
                //    {
                //        if (shot.CollidedWith.GetComponent<Coin>() == null)
                //        {
                //            extraTurn = true;
                //            break;
                //        }
                //    }
                //}

                //// Check if both the striker and carromMan reached the goal post.
                //if (kvp.Key.GetComponent<Coin>() && kvp.Key.GetComponent<Coin>().CoinType == CoinType.Striker)
                //{
                //    foreach (ShotReport shot in kvp.Value)
                //    {
                //        if (shot.CollidedWith.GetComponent<Coin>() && shot.CollidedWith.GetComponent<Coin>().CoinType != CoinType.Striker)
                //        {
                //            coinToPlace = shot.CollidedWith.GetComponent<Coin>().gameObject;
                //            placeCarromMan = true;
                //            break;
                //        }
                //    }
                //}
            }
        }

        overrideTurn = false;
        shotsDict.Clear();

        // Check if Queen reaches goal post and score is not maximum
        if (queenReachedGoal && scoreToEvaluate != Constants.MaximumScore)
        {
            score = false;
            placeCarromMan = false;
            extraTurn = false;
            resetToPast = true;
        }

        return (score, placeCarromMan, extraTurn, queenReachedGoal, resetToPast, coinToPlace);
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
    public bool BaulkTrigger { get { return baulkTrigger; } }
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
