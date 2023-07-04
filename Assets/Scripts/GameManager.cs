using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float StrikeForceMultiplier = 3.5f;
    public LineRenderer ShotRenderer;

    public int MaxSimulatedFrames = 600;
    [Space]
    List<GameObject> carromCoins;
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



        //Create ghost objects for physics simulation
        for (int i = 0; i < carromCoins.Count; i++)
        {
            var coinGO = carromCoins[i];

            var ghostGameObject = Instantiate(coinGO, coinGO.transform.position, coinGO.transform.rotation);

            if (ghostGameObject.tag == Constants.Tag_Striker) { ghostStriker = ghostGameObject; Destroy(ghostStriker.GetComponent<StrikerController>()); }
            ghostGameObject.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostGameObject, simulationScene);
            ghostCoins.Add(ghostGameObject);
            ghostsPreSimPos.Add(ghostGameObject.transform.position);
        }


        ruleEvaluator = GetComponent<PostShotRuleEvaluator>();
    }

    // Update is called once per frame
    void Update()
    {
        ruleEvaluator.SetFaction(CoinType.Faction1);
        if (isShotPlaying)
        {
            if (strikerRig.velocity.magnitude < 0.01f)
            {
                evaluateShot();
                Debug.Log("Shot complete. Evaluating...");
                isShotPlaying = false;
            }

        }

        Vector3 currentMousePos = Input.mousePosition;
        RaycastHit hit;
        var clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(clickRay, out hit, 50f/*, ignoreLayers*/);

        //Get viewport point to calculate for camera delta
        var clickViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);


        if (Input.GetMouseButton(0) && !touchIsDragging)
        {

            if (hit.collider.gameObject == striker)
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

                ShotRenderer.enabled = false;
            }
            dragStartPos.y = hitPoint.y = strikerTransfrom.position.y;
            ShotRenderer.transform.position = dragStartPos;
            var shotDir = dragStartPos - hitPoint;
            //ShotRenderer.SetPosition(0, strikerTransfrom.position);
            //ShotRenderer.SetPosition(1, strikerTransfrom.position + (shotDir * StrikeForceMultiplier));

        }

    }

    void evaluateShot()
    {
        Debug.Log(ruleEvaluator.EvaluateEvents());
    }



    //Called from Goal post to indicate which faction coin has been pucked
    public void CoinPucked(GameObject coin)
    {
        var index = carromCoins.IndexOf(coin);

        carromCoins.Remove(coin);
        coin.SetActive(false);
        //Destroy(coin);

        var ghost = ghostCoins[index];
        ghostCoins.Remove(ghost);
        //Destroy(ghost);
        ghost.SetActive(false);
    }
}
