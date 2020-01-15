using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    public Queue<Action> ActionQueue = new Queue<Action>();
    public bool IsRunning;
    void Start()
    {
        IsRunning = false;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(IsRunning);
        if (IsRunning)
        {
            if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log(ActionQueue.Count);
                if (ActionQueue.Count > 0)
                {
                    Action a = ActionQueue.Dequeue();
                    a.Play(gameObject);
                } else
                {
                    IsRunning = false;
                }
                
            }
        
        }
        
    }
}
