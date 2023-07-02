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
    public List<GameObject> CarromCoins;
    public Transform BoardBoundary;
    public LayerMask ignoreLayers;
    public Vector3 StrikerForceDirection { get { return strikerForceDirection; } }
    [Space]
    [Header("Debug Only")]
    public bool SimulatedPhysics = false;
    GameObject striker;
    Transform strikerTransfrom;
    GameObject ghostStriker;

    bool touchIsDragging;
    Vector3 dragStartPos;
    Vector3 dragEndPos;

    Scene simulationScene;
    PhysicsScene physicsSimulationScene;

    Dictionary<GameObject, Vector3> CoinPositions = new Dictionary<GameObject, Vector3>();
    Dictionary<GameObject, Vector3> GhostCoinPositions = new Dictionary<GameObject, Vector3>();

    List<GameObject> ghosts = new List<GameObject>();
    List<Vector3> ghostsPreSimPos = new List<Vector3>();
    Vector3 strikerForceDirection;

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.RegisterGameManager(this);
        Application.targetFrameRate = 60;
        striker = GameObject.FindGameObjectWithTag(Constants.Tag_Striker);
        strikerTransfrom = striker.transform;

        simulationScene = SceneManager.CreateScene("SimulatedBoard", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsSimulationScene = simulationScene.GetPhysicsScene();

        var ghostBoundary = Instantiate(BoardBoundary.gameObject, BoardBoundary.position, BoardBoundary.rotation);
        ghostBoundary.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostBoundary, simulationScene);

        for (int i = 0; i < CarromCoins.Count; i++)
        {
            var coinGO = CarromCoins[i];
            CoinPositions.Add(coinGO, coinGO.transform.position);

            var ghostGameObject = Instantiate(coinGO, coinGO.transform.position, coinGO.transform.rotation);

            if (ghostGameObject.tag == Constants.Tag_Striker) ghostStriker = ghostGameObject;
            ghostGameObject.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostGameObject, simulationScene);
            ghosts.Add(ghostGameObject);
            ghostsPreSimPos.Add(ghostGameObject.transform.position);
            GhostCoinPositions.Add(ghostGameObject, ghostGameObject.transform.position);
        }

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
            for (int i = 0; i < CarromCoins.Count; i++)
            {
                ghosts[i].transform.position = CarromCoins[i].transform.position;
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


            if (Input.GetMouseButtonUp(0))
            {
                dragEndPos = hit.point;
                dragEndPos.y = 0;
                touchIsDragging = false;

                ShotRenderer.positionCount = 0;

                if (SimulatedPhysics)
                {

                    //    ghostStriker.GetComponent<Rigidbody>().AddForce((dragStartPos - dragEndPos) * StrikeForceMultiplier, ForceMode.Impulse);
                    //    ShotRenderer.positionCount = MaxSimulatedFrames;
                    //    for (int i = 0; i < MaxSimulatedFrames; i++)
                    //    {
                    //        Physics.Simulate(Time.fixedDeltaTime);
                    //        ShotRenderer.SetPosition(i, strikerTransfrom.position);

                    //    }
                }
                else
                {

                    strikerTransfrom.GetComponent<Rigidbody>().AddForce((dragStartPos - dragEndPos) * StrikeForceMultiplier, ForceMode.Impulse);
                }
                ShotRenderer.enabled = false;
            }
            dragStartPos.y = hitPoint.y = strikerTransfrom.position.y;
            ShotRenderer.transform.position = dragStartPos;
            var shotDir = dragStartPos - hitPoint;
            //ShotRenderer.SetPosition(0, strikerTransfrom.position);
            //ShotRenderer.SetPosition(1, strikerTransfrom.position + (shotDir * StrikeForceMultiplier));

        }

    }
}
