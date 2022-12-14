using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class TpProgram : UdonSharpBehaviour
{
    public GameObject target;
    public bool useWhitelist = false;
    public string[] StaffList; 

    public override void Interact(){
        VRCPlayerApi player = Networking.LocalPlayer;
        foreach (string name in StaffList)
            if (player.displayName == name || !useWhitelist){
                Networking.LocalPlayer.TeleportTo(target.transform.position, target.transform.rotation);
                break;
            }
    }
}
