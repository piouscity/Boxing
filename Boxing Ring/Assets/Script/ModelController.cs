using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    public static Queue<Action> ActionQueue = new Queue<Action>();
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") && ActionQueue.Count > 0)
        {
            Action a = ActionQueue.Dequeue();
            a.Play(gameObject);
        }
    }
}
