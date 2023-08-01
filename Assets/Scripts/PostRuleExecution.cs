using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PostRuleExecution : MonoBehaviour
{
    [SerializeField] UnityEvent PostScoreEvent;
    [SerializeField] UnityEvent PostScoreEventNoScore;
    public void ExecutePostScoreEvent()
    {
        PostScoreEvent.Invoke();
    }
    public void ExecutePostScoreEventNoScore() {  PostScoreEventNoScore.Invoke(); }
}
