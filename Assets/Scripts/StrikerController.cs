using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class StrikerController : MonoBehaviour
{
    public float StrikeForceMultiplier = 3.5f;


    bool touchIsDragging;
    Vector3 dragStartPos;
    Vector3 dragEndPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.touchCount > 0)
        //{
        //    var touch = Input.GetTouch(0);

        //    Debug.Log(touch.position);

        //}
        Vector3 currentMousePos = Input.mousePosition;
        RaycastHit hit;
        var clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(clickRay, out hit, 50f);

        if (Input.GetMouseButton(0) && !touchIsDragging)
        {

            if (hit.collider.gameObject == this.gameObject)
            {
                dragStartPos = transform.position;
                dragStartPos.y = 0;
                touchIsDragging = true;
            }
        }

        if (touchIsDragging)
        {
            Debug.DrawLine(transform.position, hit.point);
            if (Input.GetMouseButtonUp(0))
            {
                dragEndPos = hit.point;
                dragEndPos.y = 0;
                touchIsDragging = false;

                GetComponent<Rigidbody>().AddForce((dragStartPos - dragEndPos) * StrikeForceMultiplier, ForceMode.Impulse);
            }
        }
    }
}
