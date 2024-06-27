using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoozyRandomOnMenu : MonoBehaviour
{
    public RuntimeAnimatorController[] animators;

    void Start()
    {
        GetComponent<Animator>().runtimeAnimatorController = animators[Random.Range(0, animators.Length)];
    }
}
