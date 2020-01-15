using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameActionManager 
{
    public static int level = 1;
    public static List<string> inputActions = new List<string>();
    public static List<string> replayActions = new List<string>();
    public static int RandomNumber(int min, int max)
    {
        System.Random random = new System.Random();
        return random.Next(min, max);
    }
    public static void GenerateInputActions(int number)
    {
        inputActions = new List<string>();
        replayActions = new List<string>();
        while (inputActions.Count < number)
        {
            switch (RandomNumber(1, 5))
            {
                case 1:
                    if (!inputActions.Contains("Drink"))
                    {
                        inputActions.Add("Drink");
                        Debug.Log("Drink");
                    }
                    break;
                case 2:
                    if (!inputActions.Contains("Kick"))
                    {
                        inputActions.Add("Kick");
                        Debug.Log("Kick");
                    }
                    break;
                case 3:
                    if (!inputActions.Contains("Sit"))
                    {
                        inputActions.Add("Sit");
                        Debug.Log("Sit");
                    }
                    break;
                case 4:
                    if (!inputActions.Contains("Walk"))
                    {
                        inputActions.Add("Walk");
                        Debug.Log("Walk");
                    }
                    break;
                default:
                    if (!inputActions.Contains("ArmWave"))
                    {
                        inputActions.Add("ArmWave");
                        Debug.Log("ArmWave");
                    }
                    break;
            }
        }
    }
    public static bool Matching(List<string> replay, List<string> input)
    {
        for (int i = 0; i < replay.Count; ++i)
            Debug.Log(replay[i]);
        if (replay.Count < input.Count)
            return false;
        for (int i = 0; i < input.Count; ++i)
        {
            if (replay[i] != input[i])
                return false;
        }
        return true;
    }
}
