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
    public GameObject TextTime;
    public GameObject TextLevel;
    private bool IsDequeued;
    public float TimeOut;
    private int numAction;
    private float TimeSince;
    private bool isMatching;
    private bool IsModelRunning;
    private void Start()
    {
        ActionManager.InitAction();
        GameActionManager.GenerateInputActions(GameActionManager.level);
        actionInLevel.Add(GameActionManager.level, GameActionManager.inputActions);
        model.GetComponent<ModelController>().ActionQueue = ActionManager.CreateListAction(actionInLevel[GameActionManager.level]);
        TimeOut = model.GetComponent<ModelController>().ActionQueue.Count * 4;
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
        TextLevel.GetComponent<TextMesh>().text = "Level: " + GameActionManager.level.ToString();
        if (model.GetComponent<ModelController>().IsRunning == false)
        {
            //Debug.Log("Player Turn");
            if (!IsDequeued)
            {
                KinectQueue.GestureQueue.Clear();
                GestureSource.IsMonitor = true;
                IsDequeued = true;
                TimeSince = GameActionManager.level * 6;
               /* Debug.Log("Player Turn");
                Debug.Log(GameActionManager.level);*/
            }
            TimeSince -= Time.deltaTime;
            TextTime.GetComponent<TextMesh>().text = "Time: " + (TimeSince >= 0 ? Math.Floor(TimeSince).ToString() : "x");
                
            if (TimeSince <= 0)
            {
                GestureSource.IsMonitor = false;
                IsDequeued = false;
                List<string> replayAction = GetActionOfPlayer();
                List<string> inputAction = GameActionManager.inputActions;
                if (GameActionManager.Matching(replayAction, inputAction))
                {
                    isMatching = true;
                    GameActionManager.level++;
                }
                TimeOut = GameActionManager.level * 4;
                model.GetComponent<ModelController>().IsRunning = true;
                
                IsModelRunning = true;
            }
        }
        else
        {
            //Debug.Log("Model Turn");
            
            isMatching = false;
            if (IsModelRunning == true)
            {
                //Debug.Log("Model Turn");
                IsModelRunning = false;
                GameActionManager.GenerateInputActions(GameActionManager.level);
                if (!actionInLevel.ContainsKey(GameActionManager.level))
                    actionInLevel.Add(GameActionManager.level, GameActionManager.inputActions);
                else
                    actionInLevel[GameActionManager.level] = GameActionManager.inputActions;
                model.GetComponent<ModelController>().ActionQueue = ActionManager.CreateListAction(actionInLevel[GameActionManager.level]);
                TimeOut = model.GetComponent<ModelController>().ActionQueue.Count * 4;
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
