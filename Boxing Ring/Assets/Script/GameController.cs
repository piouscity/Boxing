using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public KinectQueue KinectQueue;
    public GestureSourceManager GestureSource;
    public GameObject model;
    public GameObject player;
    public Dictionary<int, List<string>> actionInLevel = new Dictionary<int, List<string>>();
    private bool IsDequeued;
    private float TimeSince;
    private bool isMatching;
    private bool IsModelRunning;
    private void Start()
    {
        ActionManager.InitAction();
        GameActionManager.GenerateInputActions(GameActionManager.level);
        actionInLevel.Add(GameActionManager.level, GameActionManager.inputActions);
        model.GetComponent<ModelController>().ActionQueue = ActionManager.CreateListAction(actionInLevel[GameActionManager.level]);
        GestureSource.IsMonitor = false;
        IsDequeued = false;
        isMatching = false;
        IsModelRunning = false;
        model.GetComponent<ModelController>().IsRunning = true;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (model.GetComponent<ModelController>().IsRunning == false)
        {
            Debug.Log("Player Turn");
            if (!IsDequeued)
            {
                KinectQueue.GestureQueue.Clear();
                GestureSource.IsMonitor = true;
                IsDequeued = true;
                TimeSince = 0;
            }
            TimeSince += Time.deltaTime;
            if (TimeSince > 10)
            {
                GestureSource.IsMonitor = false;
                IsDequeued = false;
                List<string> replayAction = GetActionOfPlayer();
                List<string> inputAction = GameActionManager.inputActions;
                if (GameActionManager.Matching(replayAction, inputAction))
                {
                    isMatching = true;
                }
                model.GetComponent<ModelController>().IsRunning = true;
                IsModelRunning = true;
            }
        }
        else
        {
            Debug.Log("Model Turn");
            if (isMatching == true)
            {
                GameActionManager.level++;
            }
            isMatching = false;
            if (IsModelRunning == true)
            {
                IsModelRunning = false;
                GameActionManager.GenerateInputActions(GameActionManager.level);
                actionInLevel.Add(GameActionManager.level, GameActionManager.inputActions);
                model.GetComponent<ModelController>().ActionQueue = ActionManager.CreateListAction(actionInLevel[GameActionManager.level]);
            }
        }

    }

    private List<string> GetActionOfPlayer()
    {
        List<string> result = new List<string>();
        string prevalue = "";
        while (KinectQueue.GestureQueue.Count > 0)
        {
            string value = KinectQueue.GestureQueue.Dequeue();
            if (prevalue == "")
                prevalue = value;
            else
                if (value != prevalue)
                {
                    result.Add(prevalue);
                    prevalue = value;
                }
        }
        if (prevalue != "")
            result.Add(prevalue);
        return result;
    }

}
