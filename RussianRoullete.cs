using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;
using VRC.Udon.Common.Interfaces;

public class DartGun : UdonSharpBehaviour
{
    public GameObject teleport;
    public AudioClip FireSound;
    public AudioClip EmptySound;

    public override void OnPickupUseDown()
    {
        if (Random.Range(0,5)==3){
            SendCustomNetworkEvent(NetworkEventTarget.All, "Fire");
            Networking.LocalPlayer.TeleportTo(teleport.transform.position, teleport.transform.rotation);
        }
        else{
            SendCustomNetworkEvent(NetworkEventTarget.All, "Empty");
        }
    }

    public void Fire()
    {
        AudioSource.PlayClipAtPoint(FireSound, transform.position);
    }

    public void Empty()
    {
        AudioSource.PlayClipAtPoint(EmptySound, transform.position);
    }
}
