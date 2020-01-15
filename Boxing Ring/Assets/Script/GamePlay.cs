using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameActionManager.replayActions.Add("Drink");
            Debug.Log("Replay: Drink");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameActionManager.replayActions.Add("StandUp");
            Debug.Log("Replay: StandUp");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameActionManager.replayActions.Add("Sit");
            Debug.Log("Replay: Sit");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameActionManager.replayActions.Add("PhoneCall");
            Debug.Log("Replay: PhoneCall");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameActionManager.replayActions.Add("ArmWave");
            Debug.Log("Replay: ArmWave");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameActionManager.replayActions.Add("Kick");
            Debug.Log("Replay: Kick");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameActionManager.replayActions.Add("Walk");
            Debug.Log("Replay: Walk");
        }

    }
}
