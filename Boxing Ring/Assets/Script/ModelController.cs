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
        if (IsRunning)
        {
            if (GameController.instance.TimeOut > 0)
            {
                if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")
                    && ActionQueue.Count > 0)
                {
                    //Debug.Log("not idle");
                    Action a = ActionQueue.Dequeue();
                    Debug.Log("Queue:" + a.name);
                    a.Play(gameObject);
                }
                GameController.instance.TimeOut -= Time.deltaTime;
            }
            else
                IsRunning = false;
        
        }
        
    }
}
