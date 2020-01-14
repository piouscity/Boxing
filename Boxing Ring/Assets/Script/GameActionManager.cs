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
            switch (RandomNumber(1, 7))
            {
                case 1:
                    if (!inputActions.Contains("Drink"))
                    {
                        inputActions.Add("Drink");
                        Debug.Log("Drink");
                    }
                    break;
                case 2:
                    if (!inputActions.Contains("StandUp"))
                    {
                        inputActions.Add("StandUp");
                        Debug.Log("StandUp");
                    }
                    break;
                case 3:
                    if (!inputActions.Contains("Kick"))
                    {
                        inputActions.Add("Kick");
                        Debug.Log("Kick");
                    }
                    break;
                case 4:
                    if (!inputActions.Contains("Sit"))
                    {
                        inputActions.Add("Sit");
                        Debug.Log("Sit");
                    }
                    break;
                case 5:
                    if (!inputActions.Contains("PhoneCall"))
                    {
                        inputActions.Add("PhoneCall");
                        Debug.Log("PhoneCall");
                    }
                    break;
                case 6:
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
    public static bool Matching(List<string> input, List<string> replay)
    {
        int res = 1;
        if (input.Count != replay.Count) { return false; }
        for (int i = 0; i < input.Count; i++)
        {
            if (input[i] != replay[i])
            {
                res *= 0;
            }
        }
        if (res == 0) { return false; }
        return true;
    }
}
