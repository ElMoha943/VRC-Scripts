using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;
using VRC.Udon.Common.Interfaces;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class TabletDanny : UdonSharpBehaviour
{
    [Header("Teleport Points")]
    public GameObject zone_vip;
    public GameObject zone_stage;
    public GameObject zone_staffroom;

    [Header("Toggleable Objects")]
    public GameObject toggle_Lights;
    public GameObject toggle_Disco;
    public GameObject toggle_Stage;
    public GameObject toggle_Shadder;
    public GameObject toggle_Logo;
    public GameObject toggle_Season;

    [UdonSynced] bool isActive_Lights = false;
    [UdonSynced] bool isActive_Disco = false;
    [UdonSynced] bool isActive_Stage = false;
    [UdonSynced] bool isActive_Shadder = false;
    [UdonSynced] bool isActive_Logo = false;
    [UdonSynced] bool isActive_Season = false;

    [Header("Respawneable Objetcs")]
    public GameObject[] respawn_objects;
    Vector3[] respawn_positions;

    [Header("UI Elements (dont touch this!)")]
    public Dropdown modeDropdown;
    public GameObject buttons_Teleport;
    public GameObject buttons_Toggles;
    public GameObject buttons_Admin;


    void Start()
    {
        respawn_positions = new Vector3[respawn_objects.Length];
        for (int i = 0; i < respawn_objects.Length; i++)
            respawn_positions[i] = respawn_objects[i].transform.position;
    }

    public void DropdownChange(){
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        switch(modeDropdown.value){
            case 0:
                buttons_Teleport.SetActive(true);
                buttons_Toggles.SetActive(false);
                buttons_Admin.SetActive(false);
                break;
            case 1:
                buttons_Teleport.SetActive(false);
                buttons_Toggles.SetActive(true);
                buttons_Admin.SetActive(false);
                break;
            case 2:
                buttons_Teleport.SetActive(false);
                buttons_Toggles.SetActive(false);
                buttons_Admin.SetActive(true);
                break;
        }
    }

    #region teleports

        public void ButtonPress_VIP(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Networking.LocalPlayer.TeleportTo(zone_vip.transform.position, zone_vip.transform.rotation);
        }

        public void ButtonPress_Stage(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Networking.LocalPlayer.TeleportTo(zone_stage.transform.position, zone_stage.transform.rotation);
        }

        public void ButtonPress_Staffroom(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Networking.LocalPlayer.TeleportTo(zone_staffroom.transform.position, zone_staffroom.transform.rotation);
        }
        

    #endregion

    #region toggles

        public void ButtonPress_Lights(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            isActive_Lights = !isActive_Lights;
            RequestSerialization();
            applyToggles();
        }

        public void ButtonPress_Disco(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            isActive_Disco = !isActive_Disco;
            RequestSerialization();
            applyToggles();
        }

        public void ButtonPress_DiscoStage(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            isActive_Stage = !isActive_Stage;
            RequestSerialization();
            applyToggles();
        }

        public void ButtonPress_Shadder(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            isActive_Shadder = !isActive_Shadder;
            RequestSerialization();
            applyToggles();
        }

        public void ButtonPress_Logo(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            isActive_Logo = !isActive_Logo;
            RequestSerialization();
            applyToggles();
        }

        public void ButtonPress_Season(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            isActive_Season = !isActive_Season;
            RequestSerialization();
            applyToggles();
        }

    #endregion

    #region admin

        public void ButtonPress_RespawnObjects(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            for (int i = 0; i < respawn_objects.Length; i++){
                Networking.SetOwner(Networking.LocalPlayer, respawn_objects[i]);
                respawn_objects[i].transform.position = respawn_positions[i];
            }
        }

    #endregion

    public override void OnDeserialization(){
        applyToggles();
    }

    public void applyToggles(){
        toggle_Lights.SetActive(isActive_Lights);
        toggle_Disco.SetActive(isActive_Disco);
        toggle_Stage.SetActive(isActive_Stage);
        toggle_Shadder.SetActive(isActive_Shadder);
        toggle_Logo.SetActive(isActive_Logo);
        toggle_Season.SetActive(isActive_Season);
    }
}