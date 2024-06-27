using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent childEvent;

    private static EventManager _instance = null;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventManager();
            }

            return _instance;
        }
    }

    private EventManager()
    {

    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            childEvent.Invoke();
        }
    }

    public void AAA()
    {
        Debug.Log("AAAAAAAA");
    }
}
