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

    public bool isDoneListAction;
    public int countAction;

    private void Start()
    {
        countAction = 0;
        isDoneListAction = false;
        ActionManager.InitAction();
        GamePlay.GenerateInputActions(GamePlay.level);
        actionInLevel.Add(GamePlay.level, GamePlay.inputActions);
    }

    private void playAction(string actionName) {
        if (model.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (ActionManager.GetAction(actionName) != null) {
                ActionManager.GetAction(actionName).Play(model);
            }
        }
    }

    public int RandomNumber(int min, int max)
    {
        System.Random random = new System.Random();
        return random.Next(min, max);
    }
    public void playListAction()
    {
        if (actionInLevel.ContainsKey(GamePlay.level))
        {
            for (int i = 0; i < actionInLevel[GamePlay.level].Count; i++)
            {
                if (countAction < actionInLevel[GamePlay.level].Count)
                {
                    playAction(actionInLevel[GamePlay.level][i]);
                    if (model.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        // Do something
                    }
                    else
                    {
                        countAction += 1;
                    }
                }
                if (i == actionInLevel[GamePlay.level].Count - 1)
                {
                    isDoneListAction = true;
                    countAction = 0;
                }
            }
        }
    }
    private void Update()
    {
        if (!isDoneListAction)
        {
            playListAction();
        }
        if (GamePlay.isLevelUp)
        {
            if (GamePlay.isDoneGenerateInputAction)
            {
                Debug.Log(GamePlay.inputActions.Count);
                actionInLevel.Add(GamePlay.level, GamePlay.inputActions);
                GamePlay.isLevelUp = false;
                isDoneListAction = false;
            }
        }
    }

}
