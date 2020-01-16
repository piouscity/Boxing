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
            switch (RandomNumber(1, 6))
            {
                case 1:
                    if (!inputActions.Contains("Drink"))
                    {
                        inputActions.Add("Drink");
                    }
                    break;
                case 2:
                    if (!inputActions.Contains("Kick"))
                    {
                        inputActions.Add("Kick");
                    }
                    break;
                case 3:
                    if (!inputActions.Contains("Sit"))
                    {
                        inputActions.Add("Sit");
                    }
                    break;
                case 4:
                    if (!inputActions.Contains("Walk"))
                    {
                        inputActions.Add("Walk");
                    }
                    break;
                default:
                    if (!inputActions.Contains("ArmWave"))
                    {
                        inputActions.Add("ArmWave");
                    }
                    break;
            }
        }
    }
    public static bool Matching(List<string> replay, List<string> input)
    {
        if (replay.Count < input.Count) return false;
        int expect = input.Count;
        List<bool> used = new List<bool>();
        for (int i = 0; i < input.Count; ++i)
            used.Add(false);
        for (int i = 0; i < replay.Count; ++i)
            for(int j=0; j<input.Count; ++j)
                if (!used[j] && replay[i]==input[j])
                {
                    used[j] = true;
                    --expect;
                    if (expect <= 0) return true;
                    break;
                }
        return expect <= 0;
    }
}
