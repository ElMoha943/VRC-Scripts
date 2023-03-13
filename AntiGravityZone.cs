using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AntiGravityZone : UdonSharpBehaviour
{
    public float defaultGravityStrength = 1.0f;
    public float increasedGravityStrength = 0.0f;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player){
        player.SetGravityStrength(increasedGravityStrength);
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player){
        player.SetGravityStrength(defaultGravityStrength);
    }
}
