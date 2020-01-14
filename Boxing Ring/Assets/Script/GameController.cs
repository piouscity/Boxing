using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject model;
    public GameObject player;
    public Dictionary<int, List<string>> actionInLevel = new Dictionary<int, List<string>>();

    private void Start()
    {
        ActionManager.InitAction();
        GameActionManager.GenerateInputActions(GameActionManager.level);
        actionInLevel.Add(GameActionManager.level, GameActionManager.inputActions);
        ModelController.ActionQueue = ActionManager.CreateListAction(actionInLevel[GameActionManager.level]);
    }
    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space))
        {
            List<string> replay = GameActionManager.replayActions;
            replay.Reverse();
            if (GameActionManager.Matching(GameActionManager.inputActions, replay))
            {
                Debug.Log("Matching!");
                PlayerController.ActionQueue = ActionManager.CreateListAction(GameActionManager.replayActions);
            } else
            {
                Debug.Log("Not match!");
            }
        }

       if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameActionManager.level++;
            GameActionManager.GenerateInputActions(GameActionManager.level);
            actionInLevel.Add(GameActionManager.level, GameActionManager.inputActions);
            ModelController.ActionQueue = ActionManager.CreateListAction(actionInLevel[GameActionManager.level]);
        }
    }

}
