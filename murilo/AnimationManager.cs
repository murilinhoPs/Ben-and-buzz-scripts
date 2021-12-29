using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [HideInInspector]public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void GetBool(string id) => animator.GetBool(id);
    public void SetBool(string id, bool value) => animator.SetBool(id, value);

    public void GetFloat(string id) => animator.GetFloat(id);
    public void SetFloat(string id, float value) => animator.SetFloat(id, value);
    
    public void Trigger(string id) => animator.SetTrigger(id);
}
