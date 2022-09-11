using UnityEngine;

public class CC_InputControl
{
    //[Header("Controller")]
    private Vector2 axis;
    private bool holdJumpButton;
    private bool fixedJump;
    private bool jumpWithForgiveness;
    private float jumpForgiveness = 0.25f;
    private float jumpForgivenessTimer;
    private bool fixedFire;
    private bool dash;
    private bool fixedLightnignDash;
    public Vector2 Axis => axis;
    public bool HoldJump => holdJumpButton;
    public bool FixedJump => fixedJump;
    public bool JumpWithForgiveness => jumpWithForgiveness;
    public bool FixedFire => fixedFire;
    public bool Dash => dash;
    public bool FixedLightnignDash => fixedLightnignDash;

    public CC_InputControl(float forgivenessfTime)
    {
        this.jumpForgiveness = forgivenessfTime;
        this.jumpForgivenessTimer = forgivenessfTime;
        this.axis = Vector2.zero;
        this.holdJumpButton = false;
        this.fixedJump = false;
        this.jumpWithForgiveness = false;
        this.fixedFire = false;
        this.fixedLightnignDash = false;
        this.dash = false;
    }

    public void UpdateController()
    {
        jumpForgivenessTimer -= Time.deltaTime;
        axis = Vector2.right * Input.GetAxisRaw("Horizontal") + Vector2.up * Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            fixedJump = true;
            jumpWithForgiveness = true;
            jumpForgivenessTimer = jumpForgiveness;
        }

        if (jumpForgivenessTimer < 0)
        {
            jumpWithForgiveness = false;
        }

        holdJumpButton = Input.GetButton("Jump");


        if (Input.GetButtonDown("Fire1"))
        {
            fixedFire = true;
        }

        if (Input.GetButtonDown("Fire2")){
            fixedLightnignDash = true;
        }


        dash = false;
        if (Input.GetButton("Fire1"))
        {
            dash = true;
        }
    }

    public void ResetController()
    {
        fixedJump = false;
        fixedFire = false;
        fixedLightnignDash = false;
    }

    public bool GetIfDrop()
    {
        if (Axis.y < 0)
        {
            return true;
        }
        return false;
    }

    //Use to remove jumpForgiveness.
    public void ConsumeJumpForgiveness()
    {
        jumpForgivenessTimer = -1;
    }

    public void KillForgiveness()
    {
        jumpForgivenessTimer = -1;
        jumpWithForgiveness = false;
    }
}