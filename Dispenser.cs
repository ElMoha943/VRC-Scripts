using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Dispenser : UdonSharpBehaviour
{
    public GameObject[] Glowsticks = new GameObject[10];
    public AudioClip DispenseSound;
    public AudioClip EmptySound;
    public float cooldownTime = 0.5f;
    [HideInInspector][UdonSynced(UdonSyncMode.None)] public int active = -1;
    float lastUse;

    void OnPlayerJoined(VRCPlayerApi player)
    {
        lastUse = Time.time;
        for (int i = 0; i < active; i++)
            Glowsticks[i].SetActive(true);
    }

    public override void Interact()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        active++;
        RequestSerialization();
        Dispense();
    }

    void Dispense(){
        if (active < 10)
        {
            if (Time.time - lastUse > cooldownTime)
            {
                AudioSource.PlayClipAtPoint(DispenseSound, transform.position);
                Glowsticks[active].SetActive(true); 
                lastUse = Time.time;
            }
        }
        else
        {
            AudioSource.PlayClipAtPoint(EmptySound, transform.position);
        }
    }

    public void OnDeserialization(){
        Dispense();
    }
}
