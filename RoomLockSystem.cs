using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;


public class RoomLockSystem : UdonSharpBehaviour
{
    [Header("Room Locks")]
    public GameObject R1;
    public GameObject R2;
    public GameObject R3;
    public GameObject R4;

    [HideInInspector][UdonSynced] public bool R1Locked = false;
    [HideInInspector][UdonSynced] public bool R2Locked = false;
    [HideInInspector][UdonSynced] public bool R3Locked = false;
    [HideInInspector][UdonSynced] public bool R4Locked = false;

    void Start(){
        _applyToggles();
    }

    // Door Locks

    public void ToggleR1(){
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        R1Locked = !R1Locked;
        RequestSerialization();
        Candado1.SetActive(R1Locked);
        R1.SetActive(!R1Locked);
    }

    public void ToggleR2(){
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        R2Locked = !R2Locked;
        RequestSerialization();
        Candado2.SetActive(R2Locked);
        R2.SetActive(!R2Locked);
    }

    public void ToggleR3(){
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        R3Locked = !R3Locked;
        RequestSerialization();
        Candado3.SetActive(R3Locked);
        R3.SetActive(!R3Locked);
    }

    public void ToggleR4(){
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        R4Locked = !R4Locked;
        RequestSerialization();
        Candado4.SetActive(R4Locked);
        R4.SetActive(!R4Locked);
    }
    
    // Networking

    public override void OnDeserialization(){
        _applyToggles();
    }

    // Misc

    public void _applyToggles(){
        R1.SetActive(!R1Locked);
        R2.SetActive(!R2Locked);
        R3.SetActive(!R3Locked);
        R4.SetActive(!R4Locked);
    }

    public void _reset(){
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        R1Locked = false;
        R2Locked = false;
        R3Locked = false;
        R4Locked = false;
        RequestSerialization();
        _applyToggles();
    }

    //Todo: Autounlock if noone inside
}
