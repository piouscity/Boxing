using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Queue<Action> ActionQueue = new Queue<Action>();
    public static bool InProgress;
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
      
    }
}
