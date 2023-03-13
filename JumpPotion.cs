
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class JumpPotion : UdonSharpBehaviour
{
    VRCPlayerApi localPlayer;
    public float defaultJumpImpulse = 3.0f;
    public float increasedJumpImpulse = 8.0f;
    [Tooltip("Duration in seconds")]
    public int duration = 60;
    public AudioClip soundEffect;

    void Start(){
        localPlayer = Networking.LocalPlayer;
    }

    public override void Interact(){
        gameObject.SetActive(false);
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        localPlayer.SetJumpImpulse(increasedJumpImpulse);
        SendCustomEventDelayedSeconds("ResetJump", duration);
    }

    public void ResetJump(){
        localPlayer.SetJumpImpulse(defaultJumpImpulse);
    }
}
