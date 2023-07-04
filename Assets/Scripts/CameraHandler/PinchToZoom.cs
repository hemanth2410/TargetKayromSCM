using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchToZoom : MonoBehaviour
{
    Vector2 touchZeroPrevPos;
    Vector2 touchOnePrevPos;
    Touch touchOne;
    Touch touchZero;
    float prevTouchDeltaMag;
    float touchDeltaMag;
    float deltaMagnitudeDiff;
    EmulatedAxisInputProvider axisInputProvider;
    [SerializeField] float m_ZoomSpeed = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        axisInputProvider = GetComponent<EmulatedAxisInputProvider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            // Store both touches.
            touchZero = Input.GetTouch(0);
            touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            // invoke a public methos in cinemachine;
            axisInputProvider.ZoomCamera(deltaMagnitudeDiff * Time.deltaTime);
        }
#if UNITY_EDITOR
        if(Input.mouseScrollDelta.sqrMagnitude > 0)
        {
            axisInputProvider.ZoomCamera(Input.mouseScrollDelta.y);
        }
#endif
    }
}
