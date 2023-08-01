using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOtherObject : MonoBehaviour
{
    [SerializeField] GameObject ObjectToFollow;
    bool follow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(follow)
        {
            transform.position = ObjectToFollow.transform.position;
        }
       
    }
    public void StopFollowing()
    {
        follow = false;
    }
    public void BeginFollow()
    {
        follow = true;
    }

}
