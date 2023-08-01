using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationExecutors : MonoBehaviour
{
    [SerializeField] string m_TriggerName;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger(m_TriggerName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
