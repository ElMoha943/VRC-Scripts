using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class TpProgram : UdonSharpBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] bool useWhitelist = false;
    [SerializeField] string[] StaffList; 

    public override void Interact(){
        VRCPlayerApi player = Networking.LocalPlayer;
        foreach (string name in StaffList){
            if (player.displayName == name || !useWhitelist){
                Networking.LocalPlayer.TeleportTo(target.transform.position, target.transform.rotation);
                break;
            }
        }
    }
}
