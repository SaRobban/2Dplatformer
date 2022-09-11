using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Master : MonoBehaviour
{
    [SerializeField] CC_InputControl inputControl;
    [SerializeField] CC_CapsuleOverlapFlags2D colliderCheck;
 //   [SerializeField] CC_ProcessInputByState movementStateMachine;

    // Start is called before the first frame update
    void Start()
    {
        inputControl = GetComponent<CC_InputControl>();
        colliderCheck = GetComponent<CC_CapsuleOverlapFlags2D>();
     //   movementStateMachine = GetComponent<CC_ProcessInputByState>();

      //  movementStateMachine.SetRefrences(inputControl, colliderCheck);
    }

    // Update is called once per frame
    void Update()
    {
      //  inputControl.UpdateJump();
    }

    private void FixedUpdate()
    {
      //  inputControl.UpdateInput();
      //  movementStateMachine.MovementState();
    }
}
