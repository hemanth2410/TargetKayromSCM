using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCoinHelper : MonoBehaviour
{
    [SerializeField] bool waitingForExit = true;
    public bool WaitingForExit { get { return waitingForExit; }  }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MakeWait()
    {
        waitingForExit = true;
    }
    public void EnableNormalMode()
    {
        waitingForExit = false;
    }
}
