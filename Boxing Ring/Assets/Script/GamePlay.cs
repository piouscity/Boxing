using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public static int level = 1;
    public static List<string> inputActions = new List<string>();
    public static List<string> replayActions = new List<string>();
    public static bool isLevelUp = false;
    public static bool isDoneGenerateInputAction = false;
    public static int RandomNumber(int min, int max)
    {
        System.Random random = new System.Random();
        return random.Next(min, max);
    }
    public static void GenerateInputActions(int number)
    {
        inputActions = new List<string>();
        while (inputActions.Count < number)
        {
            switch (RandomNumber(1, 7))
            {
                case 1:
                    if (!inputActions.Contains("Drink"))
                    {
                        inputActions.Add("Drink");
                    }
                    break;
                case 2:
                    if (!inputActions.Contains("StandUp"))
                    {
                        inputActions.Add("StandUp");
                    }
                    break;
                case 3:
                    if (!inputActions.Contains("Kick"))
                    {
                        inputActions.Add("Kick");
                    }
                    break;
                case 4:
                    if (!inputActions.Contains("Sit"))
                    {
                        inputActions.Add("Sit");
                    }
                    break;
                case 5:
                    if (!inputActions.Contains("PhoneCall"))
                    {
                        inputActions.Add("PhoneCall");
                    }
                    break;
                case 6:
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
        isDoneGenerateInputAction = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    int index = 0;
    // Update is called once per frame
    void Update()
    {
        index += 1;
        if (index == 600 * level)
        {
            level++;
            Debug.Log(level);
            isLevelUp = true;
            GenerateInputActions(level);
            index = 0;
        } 
    }

    private bool Matching(List<string> input, List<string> replay)
    {
        return input == replay;
    }
}
