
using System.Collections.Generic;
using UnityEngine;

public class Action 
{
    public string name;
    public void Play(GameObject gameObject) {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger(name);
    }
    
}

public class ActionManager {
    public static Dictionary<string, Action> actions = new Dictionary<string, Action>();
    public static void InitAction()
    {
        List<Action> listAction = new List<Action>();
        listAction.Add(new Action() { name = "Drink" });
        //listAction.Add(new Action() { name = "StandUp" });
        listAction.Add(new Action() { name = "Sit" });
        //listAction.Add(new Action() { name = "PhoneCall" });
        listAction.Add(new Action() { name = "ArmWave" });
        listAction.Add(new Action() { name = "Kick" });
        listAction.Add(new Action() { name = "Walk" });
        foreach (Action action in listAction) {
            AddAction(action);
        }
    }

    public static Queue<Action> CreateListAction(List<string> listActionName)
    {
        Queue<Action> temp = new Queue<Action>();
        foreach (string name in listActionName)
        {
            temp.Enqueue(new Action() { name = name });
        }
        return temp;
    }

    public static void AddAction(Action action) {
        if (actions.ContainsKey(action.name))
        {
            return;
        }
        actions.Add(action.name, action);
    }

    public static Action GetAction(string actionName) {
        if (actions.ContainsKey(actionName))
        {
            return actions[actionName];
        }
        return null;
    }
}
