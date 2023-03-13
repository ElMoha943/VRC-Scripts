using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SpeedPotion : UdonSharpBehaviour
{
    VRCPlayerApi localPlayer;
    public float defaultWalkSpeed = 2.0f;
    public float defaultRunSpeed = 4.0f;
    public float defualtStafeSpeed = 2.0f;
    public float increasedWalkSpeed = 4.0f;
    public float increasedRunSpeed = 8.0f;
    public float increasedStafeSpeed = 4.0f;
    [Tooltip("Duration in seconds")]
    public int duration = 60;
    public AudioClip soundEffect;

    void Start(){
        localPlayer = Networking.LocalPlayer;
    }

    public override void Interact(){
        gameObject.SetActive(false);
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
        localPlayer.SetWalkSpeed(increasedWalkSpeed);
        localPlayer.SetRunSpeed(increasedRunSpeed);
        localPlayer.SetStrafeSpeed(increasedStafeSpeed);
        SendCustomEventDelayedSeconds("ResetSpeed", duration);
    }

    public void ResetSpeed(){
        localPlayer.SetWalkSpeed(defaultWalkSpeed);
        localPlayer.SetRunSpeed(defaultRunSpeed);
        localPlayer.SetStrafeSpeed(defualtStafeSpeed);
    }
}
