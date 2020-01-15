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
            if (GameController.instance.TimeOut > 0)
            {
                Debug.Log(ActionQueue.Count);
                if (ActionQueue.Count > 0)
                {
                    Action a = ActionQueue.Dequeue();
                    a.Play(gameObject);
                }
                GameController.instance.TimeOut -= Time.deltaTime;
            }
            else
                IsRunning = false;
        
        }
        
    }
}
