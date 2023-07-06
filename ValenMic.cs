using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ValenMic : UdonSharpBehaviour
{
    [SerializeField,Tooltip("Value to override the default voice gain.")]
    int defaultVoiceGain = 12;
    [SerializeField,Tooltip("Value to override the default voice distance.")]
    int defaultVoiceDistance = 20;
    [SerializeField,Tooltip("Voice gain while holding the mic.")]
    int amplifiedVoiceGain = 18;
    [SerializeField,Tooltip("Voice distance while holding the mic.")]
    int amplifiedVoiceDistance = 100;

    public override void OnPlayerJoined(VRCPlayerApi player){
        //Reduce the default voice gain of the players
        player.SetVoiceDistanceFar(defaultVoiceDistance);
        player.SetVoiceGain(defaultVoiceGain);
    }

    public override void OnPickup(){
        //Networking.SetOwner(Networking.LocalPlayer, gameObject);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Amplify");
    }

    public override void OnDrop(){
        //Networking.SetOwner(Networking.LocalPlayer, gameObject);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UnAmplify");
    }

    public void Amplify(){
        VRCPlayerApi owner = Networking.GetOwner(gameObject);
        owner.SetVoiceDistanceFar(amplifiedVoiceDistance);
        owner.SetVoiceGain(amplifiedVoiceGain);
    }

    public void UnAmplify(){
        VRCPlayerApi owner = Networking.GetOwner(gameObject);
        owner.SetVoiceDistanceFar(defaultVoiceDistance);
        owner.SetVoiceGain(defaultVoiceGain);
    }
}
