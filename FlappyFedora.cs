using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
using VRC.Udon.Common.Interfaces;

public class FlappyFedora : UdonSharpBehaviour
{
    [Tooltip("Jump velocity")]
    public float velocityY = 4.0f;
    [Tooltip("Number of jumps. -1 for infinite")]
    public int jumps = -1;

    int extra_jumps;
    VRCPlayerApi localPlayer;
    Vector3 jumpVelocity;

    void Start()
    {
        extra_jumps = jumps;
        localPlayer = Networking.LocalPlayer;
        jumpVelocity = new Vector3(0, velocityY, 0);
    }

    public void InputMoveHorizontal(float axisPosition, UdonInputEventArgs args)
    {
        if (localPlayer.IsPlayerGrounded() == true) extra_jumps = jumps;
    }

    public void InputMoveVertical(float axisPosition, UdonInputEventArgs args)
    {
        if (localPlayer.IsPlayerGrounded() == true) extra_jumps = jumps;
    }

    public void InputJump(bool isPressed, UdonInputEventArgs args){
        if(localPlayer.IsPlayerGrounded() == false && isPressed && (extra_jumps > 0 || extra_jumps == -1)){
            localPlayer.SetVelocity(jumpVelocity);
            if(extra_jumps != -1) extra_jumps -= 1;
        }
    }
}
