using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScrikerPlacement : MonoBehaviour
{
    [SerializeField] float DistanceFromCenter = 7.5f;
    [SerializeField] GameObject Striker;
    [SerializeField] GameObject dummyObject;
    [SerializeField] float m_StrikerRadius;
    [SerializeField] LayerMask m_DetectionMask;
    [SerializeField, ColorUsage(true, true)] Color m_ValidColor; 
    [SerializeField, ColorUsage(true, true)] Color m_InValidColor;
    [SerializeField] Button m_PlaceButton;
    [SerializeField] CoinToPlaceObject m_CoinToPlaceObject;
    [SerializeField] LayerMask m_PlacementLayerMask;
    [SerializeField] Vector3 m_PlacementOffset;
    [SerializeField] CinemachineVirtualCamera m_CoinPlacementCamera;
   Material dummyMaterial;
    bool inValidPlacement;
    Camera mainCamera;
    Vector3 direction;
    Vector3 ReprojectedVector;
    IEnumerator vibrationRoutine;
    // Make the striker kinamatic and disable game manager script or make it not active until striker is palced.
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        dummyMaterial = dummyObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        if(m_CoinToPlaceObject.Value != null)
        {
            return;
        }
        if(m_CoinToPlaceObject.Value == null)
        {
            direction = mainCamera.transform.position - new Vector3(0, 0, 0);
            ReprojectedVector = new Vector3(direction.x, 0, direction.z);
            dummyObject.transform.position = ReprojectedVector.normalized * (DistanceFromCenter / 2.0f);
            m_PlaceButton.enabled = !inValidPlacement;
        }
        if (inValidPlacement)
        {
            
            if (vibrationRoutine == null)
            {
                vibrationRoutine = NiceVibration();
                StartCoroutine(vibrationRoutine);
            }
        }
    }
    private void FixedUpdate()
    {
        detectImproperPlacement(dummyObject.transform.position);
        if (m_CoinToPlaceObject.Value != null && m_CoinToPlaceObject.Value.GetComponent<Coin>().CoinType != CoinType.Striker)
        {
            m_CoinPlacementCamera.Priority = 1000;
            Ray detectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(detectionRay,out hit,Mathf.Infinity, m_PlacementLayerMask))
            {
                Vector3 point = hit.point;
                dummyObject.transform.position = point;
                if (Input.GetMouseButtonUp(0) && !inValidPlacement)
                {
                    // capture position here to place the coin
                    // perform a raycast from screen to raycast plane and get a position, finally add offset.
                    // again perform proper placement check from the position

                    m_CoinToPlaceObject.Value.SetActive(true);
                    m_CoinToPlaceObject.Value.transform.position = point + m_PlacementOffset;
                    m_CoinToPlaceObject.Value = null;
                    GameController.Instance.InvokePlacementComplete();
                    m_CoinPlacementCamera.Priority = 0;
                }
            }
            
        }
    }

    void detectImproperPlacement(Vector3 position)
    {
        Collider[] _coins = Physics.OverlapSphere(position, m_StrikerRadius, m_DetectionMask);
        List<Collider> _detection = _coins.ToList();
        _detection.RemoveAll(x => x.GetComponent<Coin>() == null);
        inValidPlacement = _detection.Any(x => x.GetComponent<Coin>().CoinType != CoinType.Striker);
        dummyMaterial.SetColor("_EmissionColor", inValidPlacement ? m_InValidColor : m_ValidColor);
    }
    public void PlaceStriker()
    {
        Striker.transform.position = ReprojectedVector.normalized * (DistanceFromCenter / 2.0f);
        Striker.transform.position += new Vector3(0, 0.065f, 0);
    }

    IEnumerator NiceVibration()
    {
        Handheld.Vibrate();
        yield return new WaitForSeconds(0.5f);
        vibrationRoutine = null;
    }
}
