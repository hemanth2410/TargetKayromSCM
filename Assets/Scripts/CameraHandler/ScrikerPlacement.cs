using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrikerPlacement : MonoBehaviour
{
    [SerializeField] float DistanceFromCenter = 7.5f;
    [SerializeField] GameObject Striker;
    [SerializeField] GameObject dummyObject;
    Camera mainCamera;
    Vector3 direction;
    Vector3 ReprojectedVector;
    // Make the striker kinamatic and disable game manager script or make it not active until striker is palced.
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        direction = mainCamera.transform.position - new Vector3(0, 0, 0);
        ReprojectedVector = new Vector3(direction.x, 0, direction.z);
        dummyObject.transform.position = ReprojectedVector.normalized * (DistanceFromCenter / 2.0f);
    }

    public void PlaceStriker()
    {
        Striker.transform.position = ReprojectedVector.normalized * (DistanceFromCenter / 2.0f);
        Striker.transform.position += new Vector3(0, 0.065f, 0);
    }
}
