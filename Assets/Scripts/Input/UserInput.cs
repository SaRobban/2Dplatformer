/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class UserInput
{
    public class KeySet
    {
        public KeyCode axisRight;
        public KeyCode axisLeft;
        public KeyCode jump;
        public KeyCode action;
        public KeyCode inventory;

        public KeySet()
        {
            //TODO: Read from disk
            axisRight = KeyCode.A;
            axisLeft = KeyCode.D;
            jump = KeyCode.Space;
            action = KeyCode.E;
            inventory = KeyCode.Tab;
        }
    }

    private static KeySet keys = new KeySet();


    //[Header("Controller")]
    public static Vector2 axis { get; private set; }
    public static bool fixedFireA { get; private set; }
    public static bool interact { get; private set; }

    public static event Action OnJump;
    public static event Action OnAction;
    public static event Action OnInventoryKey;

    public static void UpdateContolls()
    {
        if (Input.GetKeyDown(keys.jump))
        {

        }
        if (Input.GetKeyDown(keys.action))
        {

        }
        if (Input.GetKeyDown(keys.jump))
        {

        }
    }
    public static void UpdateController()
    {
        jumpForgivenessTimer -= Time.deltaTime;
        axis = Vector2.right * Input.GetAxisRaw("Horizontal") + Vector2.up * Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
           
        }

        if (jumpForgivenessTimer < 0)
        {
            jumpWithForgiveness = false;
        }

        holdJumpButton = Input.GetButton("Jump");


        if (Input.GetButtonDown("Fire1"))
        {
            fixedFireA = true;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            OnInventoryKey?.Invoke();
        }

        if (Input.GetButtonDown("Interact"))
        {
            interact = true;
        }
    }

    public static void ResetController()
    {
        fixedJump = false;
        fixedFireA = false;

        interact = false;
    }

    public static bool GetIfDrop()
    {
        if (axis.y < 0)
        {
            return true;
        }
        return false;
    }

    //Use to remove jumpForgiveness.
    public static void ConsumeJumpForgiveness()
    {
        jumpForgivenessTimer = -1;
    }

    public static void KillForgiveness()
    {
        jumpForgivenessTimer = -1;
        jumpWithForgiveness = false;
    }
}*/