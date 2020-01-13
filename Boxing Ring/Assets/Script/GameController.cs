using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject model;
    public GameObject player;
    public List<int> actions = new List<int>();
    private void Start()
    {
        ActionManager.InitAction();
        Debug.Log("Init action");
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (model.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log("Idle");
                ActionManager.GetAction("Sit").Play(model);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (model.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log("Idle");
                ActionManager.GetAction("Drink").Play(model);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (model.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log("Idle");
                ActionManager.GetAction("StandUp").Play(model);
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (model.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log("Idle");
                ActionManager.GetAction("Walk").Play(model);
            }
        }
        if (Input.GetKey(KeyCode.G))
        {
            if (model.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log("Idle");
                ActionManager.GetAction("ArmWave").Play(model);
            }
        }
        if (Input.GetKey(KeyCode.H))
        {
            if (model.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log("Idle");
                ActionManager.GetAction("Kick").Play(model);
            }
        }
        if (Input.GetKey(KeyCode.J))
        {
            if (model.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                Debug.Log("Idle");
                ActionManager.GetAction("PhoneCall").Play(model);
            }
        }
    }

}
