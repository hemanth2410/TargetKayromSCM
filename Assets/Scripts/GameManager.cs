using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float StrikeForceMultiplier = 3.5f;
    public LineRenderer ShotRenderer;

    public int MaxSimulatedFrames = 600;
    [Space]
    List<GameObject> carromCoins;
    List<Vector3> preShotPos = new List<Vector3>();                   //save pre shot positions of coins
    List<Rigidbody> coinRigs = new List<Rigidbody>();
    public Transform[] StaticPhysicsObjects;
    public LayerMask ignoreLayers;
    public Vector3 StrikerForceDirection { get { return strikerForceDirection; } }
    [Space]
    [Header("Debug Only")]

    //Get private references to striker and other coins
    GameObject striker;
    Rigidbody strikerRig;
    Transform strikerTransfrom;
    GameObject ghostStriker;
    GameObject queen;


    bool touchIsDragging;
    Vector3 dragStartPos;
    Vector3 dragEndPos;

    Scene simulationScene;
    PhysicsScene physicsSimulationScene;


    List<GameObject> ghostCoins = new List<GameObject>();
    List<Vector3> ghostsPreSimPos = new List<Vector3>();
    Vector3 strikerForceDirection;


    bool isShotPlaying;
    PostShotRuleEvaluator ruleEvaluator;
    List<GameObject> puckedCoins = new List<GameObject>();
    List<GameObject> puckedGhosts = new List<GameObject>();
    CoinType currentFaction = CoinType.Faction1;
    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.RegisterGameManager(this);
        Application.targetFrameRate = 60;
        striker = GameObject.FindGameObjectWithTag(Constants.Tag_Striker);
        strikerRig = striker.GetComponent<Rigidbody>();
        strikerTransfrom = striker.transform;

        queen = GameObject.FindGameObjectWithTag(Constants.Tag_Queen);

        simulationScene = SceneManager.CreateScene("SimulatedBoard", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsSimulationScene = simulationScene.GetPhysicsScene();


        //Find and add all coins on board into the list
        carromCoins = new List<GameObject>();
        carromCoins.Add(striker);

        var faction1coins = GameObject.FindGameObjectsWithTag(Constants.Tag_Faction1);
        var faction2coins = GameObject.FindGameObjectsWithTag(Constants.Tag_Faction2);

        carromCoins.AddRange(faction1coins);
        carromCoins.AddRange(faction2coins);

        carromCoins.Add(queen);

        foreach (Transform GO in StaticPhysicsObjects)
        {
            var go = Instantiate(GO.gameObject, GO.position, GO.rotation);
            SceneManager.MoveGameObjectToScene(go, simulationScene);

            try
            {
                go.GetComponent<Renderer>().enabled = false;
            }
            catch (Exception e)
            {
                continue;
            }
        }



        //Create ghost objects for physics simulation, add rigidbodies to collection
        for (int i = 0; i < carromCoins.Count; i++)
        {
            var coinGO = carromCoins[i];
            coinRigs.Add(coinGO.GetComponent<Rigidbody>());

            var ghostGameObject = Instantiate(coinGO, coinGO.transform.position, coinGO.transform.rotation);
            ghostGameObject.GetComponent<Coin>().enabled = false;
            if (ghostGameObject.tag == Constants.Tag_Striker)
            {
                ghostStriker = ghostGameObject;
                Destroy(ghostStriker.GetComponent<StrikerController>());
            }
            ghostGameObject.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostGameObject, simulationScene);
            ghostCoins.Add(ghostGameObject);
            ghostsPreSimPos.Add(ghostGameObject.transform.position);
        }


        ruleEvaluator = GetComponent<PostShotRuleEvaluator>();
        ruleEvaluator.SetFaction(CoinType.Faction1);

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 currentMousePos = Input.mousePosition;
        RaycastHit hit;
        var clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(clickRay, out hit, 50f/*, ignoreLayers*/);

        //Get viewport point to calculate for camera delta
        var clickViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);


        if (Input.GetMouseButton(0) && !touchIsDragging)
        {

            if (hit.collider.gameObject == striker && !isShotPlaying)
            {
                dragStartPos = strikerTransfrom.position;
                dragStartPos.y = 0;
                touchIsDragging = true;
                ShotRenderer.enabled = true;

            }
        }

        if (touchIsDragging)
        {
            var hitPoint = hit.point;
            Debug.DrawLine(strikerTransfrom.position, hitPoint);
            hitPoint.y = 0;
            strikerForceDirection = dragStartPos - hitPoint;
            for (int i = 0; i < carromCoins.Count; i++)
            {
                ghostCoins[i].transform.position = carromCoins[i].transform.position;
            }
            GameController.Instance.RuleEvaluator.EvaluateRules();
            ghostStriker.transform.position = striker.transform.position;
            ghostStriker.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ghostStriker.GetComponent<Rigidbody>().AddForce((strikerForceDirection) * StrikeForceMultiplier, ForceMode.Impulse);
            ShotRenderer.positionCount = MaxSimulatedFrames;
            for (int i = 0; i < MaxSimulatedFrames; i++)
            {
                physicsSimulationScene.Simulate(Time.fixedDeltaTime);
                ShotRenderer.SetPosition(i, ghostStriker.transform.position);

            }
            ShotRenderer.SetPosition(0, striker.transform.position);

            //Strike the striker when we release
            if (Input.GetMouseButtonUp(0))
            {
                dragEndPos = hit.point;
                dragEndPos.y = 0;
                touchIsDragging = false;
                isShotPlaying = true;

                ShotRenderer.positionCount = 0;


                strikerTransfrom.GetComponent<Rigidbody>().AddForce((dragStartPos - dragEndPos) * StrikeForceMultiplier, ForceMode.Impulse);
                StartCoroutine("checkforShotEnd");
                ShotRenderer.enabled = false;
                for (int i = 0; i < carromCoins.Count; i++)
                {
                    preShotPos.Add(carromCoins[i].transform.position);
                }

            }


            dragStartPos.y = hitPoint.y = strikerTransfrom.position.y;
            ShotRenderer.transform.position = dragStartPos;
            var shotDir = dragStartPos - hitPoint;
            //ShotRenderer.SetPosition(0, strikerTransfrom.position);
            //ShotRenderer.SetPosition(1, strikerTransfrom.position + (shotDir * StrikeForceMultiplier));

        }

    }

    IEnumerator checkforShotEnd()
    {
        yield return new WaitForSeconds(0.5f);

        while (isShotPlaying)
        {
            if (coinRigs.All(x => (x.velocity.magnitude <= 0.01f || !x.gameObject.activeSelf)))
            {
                evaluateShot();
                Debug.Log("Shot complete. Evaluating...");
                isShotPlaying = false;
            }
            yield return new WaitForFixedUpdate();
        }

    }

    void evaluateShot()
    {
        var eval = ruleEvaluator.EvaluateEvents();
        //item 1 = score
        //item 2 = change turn

        //1,1 == Score and retain turn,\n 1,0 == score, lose turn,\n 0,1 == reset and ose turn,\n 0,0 == no score, lose turn

        //Score and retain turn
        if (eval.Item1 == true && eval.Item2 == true)
        {
            processScore();
            preShotPos.Clear();
        }
        //Score and lose turn
        else if (eval.Item1 == true && eval.Item2 == false)
        {
            processScore();
            preShotPos.Clear();

            if (currentFaction == CoinType.Faction1) currentFaction = CoinType.Faction2;
            else currentFaction = CoinType.Faction1;

        }
        //Resert booard and lose turn
        else if (eval.Item1 == false && eval.Item2 == true)
        {
            foreach (GameObject coin in puckedCoins)
            {
                coin.SetActive(true);
            }
            foreach (GameObject ghost in puckedGhosts)
            {
                ghost.SetActive(true);
            }

            revertBoard();

            if (currentFaction == CoinType.Faction1) currentFaction = CoinType.Faction2;
            else currentFaction = CoinType.Faction1;

        }
        //Lose turn
        else if (eval.Item1 == false && eval.Item2 == false)
        {
            if (currentFaction == CoinType.Faction1) currentFaction = CoinType.Faction2;
            else currentFaction = CoinType.Faction1;

        }


        puckedCoins.Clear();
        puckedGhosts.Clear();


        ruleEvaluator.SetFaction(currentFaction);


    }

    private void revertBoard()
    {

        for (int i = 0; i < carromCoins.Count; i++)
        {
            carromCoins[i].transform.position = preShotPos[i];
        }

    }

    private void processScore()
    {
        foreach (GameObject coin in puckedCoins)
        {
            var i = carromCoins.IndexOf(coin);
            GameController.Instance.InvokeScoreEvent(coin.GetComponent<Coin>().CoinType, 1);

            coinRigs.RemoveAt(i);
            carromCoins.Remove(coin);
            Destroy(coin);
        }
        foreach (GameObject ghost in puckedGhosts)
        {
            ghostCoins.Remove(ghost);
            Destroy(ghost);
        }

    }

    //Called from Goal post to indicate which faction coin has been pucked
    public void CoinPucked(GameObject coin)
    {
        if (!isShotPlaying) return;


        var index = carromCoins.IndexOf(coin);
        coinRigs[index].velocity = Vector3.zero;
        coin.SetActive(false);
        puckedCoins.Add(coin);
        //carromCoins.Remove(coin);
        //Destroy(coin);

        var ghost = ghostCoins[index];
        ghost.SetActive(false);
        puckedGhosts.Add(ghost);

        //ghostCoins.Remove(ghost);
        //Destroy(ghost);
    }
}
