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
    public GameObject spawn;
    public GameObject zone_vip;
    public GameObject zone_stage;
    public GameObject zone_staffroom;
    public GameObject zone_ban;

    [Header("Toggleable Objects")]
    public GameObject toggle_Lights;
    public GameObject toggle_Disco;
    public GameObject toggle_Stage;
    public GameObject toggle_Shadder;
    public GameObject toggle_Logo;
    public GameObject toggle_Season;

    [UdonSynced] bool isActive_Lights = true;
    [UdonSynced] bool isActive_Disco = false;
    [UdonSynced] bool isActive_Stage = false;
    [UdonSynced] bool isActive_Shadder = false;
    [UdonSynced] bool isActive_Logo = false;
    [UdonSynced] bool isActive_Season = true;

    [Header("Respawneable Objetcs")]
    public GameObject[] respawn_objects;
    Vector3[] respawn_positions;

    [Header("UI Elements (dont touch this!)")]
    public GameObject dropdown;
    public Dropdown modeDropdown;
    public GameObject buttons_Teleport;
    public GameObject buttons_Toggles;
    public GameObject buttons_Admin;
    public GameObject buttons_Member;
    public GameObject content;
    public TextMeshProUGUI title_Username;
    public TextMeshProUGUI title_Rank;
    public TextMeshProUGUI title_OnlineUsers;

    GameObject[] buttons = new GameObject[80]; // Buttons for user list
    VRCPlayerApi[] players = new VRCPlayerApi[80];
    int playerCount = 0;

    VRCPlayerApi target;
    [UdonSynced] int targetid;
    [UdonSynced] int casterid;
    [UdonSynced] int mode = 0;

    void Start(){
        title_Username.text = Networking.LocalPlayer.displayName;
        VRCPlayerApi.GetPlayers(players);
        for (int i = 0; i < content.transform.childCount; i++)
            buttons[i] = content.transform.GetChild(i).gameObject; // Get button
        updatePlayerList();
        respawn_positions = new Vector3[respawn_objects.Length];
        for (int i = 0; i < respawn_objects.Length; i++)
            respawn_positions[i] = respawn_objects[i].transform.position;
    }

    void updatePlayerList(){
        playerCount=0;
        for(int i =0;i<80;i++){
            if(players[i] != null){
                playerCount++; // Increase player count
                buttons[i].SetActive(true);
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = players[i].displayName;
            }
            else buttons[i].SetActive(false);
        }
        title_OnlineUsers.text = "Online Users: " + (playerCount-1);
    }

    public override void OnPlayerJoined(VRCPlayerApi player){
        VRCPlayerApi.GetPlayers(players); // Put all players into players[]
        updatePlayerList(); // Update player list 
    }

    public override void OnPlayerLeft(VRCPlayerApi player){
        for(int i = 0; i < players.Length; i++)
            if(players[i] == player) players[i] = null; // Remove player from list
        updatePlayerList(); // Update player list
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
            mode = 0;
            RequestSerialization();
            applyToggles();
        }

        public void ButtonPress_Disco(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            isActive_Disco = !isActive_Disco;
            mode = 0;
            RequestSerialization();
            applyToggles();
        }

        public void ButtonPress_DiscoStage(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            isActive_Stage = !isActive_Stage;
            mode = 0;
            RequestSerialization();
            applyToggles();
        }

        public void ButtonPress_Shadder(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            isActive_Shadder = !isActive_Shadder;
            mode = 0;
            RequestSerialization();
            applyToggles();
        }

        public void ButtonPress_Logo(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            isActive_Logo = !isActive_Logo;
            mode = 0;
            RequestSerialization();
            applyToggles();
        }

        public void ButtonPress_RespawnObjects(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            for (int i = 0; i < respawn_objects.Length; i++){
                Networking.SetOwner(Networking.LocalPlayer, respawn_objects[i]);
                respawn_objects[i].transform.position = respawn_positions[i];
            }
        }

    #endregion

    #region admin

        public void ButtonPress_BringPlayer(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            targetid = target.playerId; // Set targetid to target player id
            casterid = Networking.LocalPlayer.playerId; // Set casterid to local player id
            mode = 1;
            RequestSerialization(); // Request sync
        }

        public void ButtonPress_BanPlayer(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            targetid = target.playerId; // Set targetid to target player id
            mode = 2;
            RequestSerialization(); // Request sync
        }

        public void ButtonPress_AddtoVIP(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            targetid = target.playerId; // Set targetid to target player id
            mode = 3;
            RequestSerialization(); // Request sync
        }

        public void ButtonPress_GotoPlayer(){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Networking.LocalPlayer.TeleportTo(target.GetPosition(), target.GetRotation());
        }

        public void ButtonPress_Back(){
            buttons_Admin.SetActive(true);
            dropdown.SetActive(true);
            buttons_Member.SetActive(false);
            title_Username.text = Networking.LocalPlayer.displayName;
        }

    #endregion

    #region networking

    public override void OnDeserialization(){
        VRCPlayerApi local = VRCPlayerApi.GetPlayerById(casterid); // Get local player
        VRCPlayerApi targett = VRCPlayerApi.GetPlayerById(targetid); // Get target player
        switch(mode){
            case 0: // Toggles
                applyToggles();
                break;
            case 1:
                if(Networking.LocalPlayer == targett) // Bring
                    Networking.LocalPlayer.TeleportTo(local.GetPosition(), local.GetRotation()); // Teleport target to local
                break;
            case 2:
                if(Networking.LocalPlayer == targett){ //Ban
                    Networking.LocalPlayer.TeleportTo(zone_ban.transform.position, zone_ban.transform.rotation); // Teleport to ban zone
                    spawn.transform.position = zone_ban.transform.position; // Set spawn to ban zone
                }
                break;
            case 3:
                if(Networking.LocalPlayer == targett){ // VIP
                    GameObject.Find("Keypad").GetComponent<Keypad>().VIPLogin(targett);
                }
                break;
        }
    }

    public void applyToggles(){
        toggle_Lights.SetActive(isActive_Lights);
        toggle_Disco.SetActive(isActive_Disco);
        toggle_Stage.SetActive(isActive_Stage);
        toggle_Shadder.SetActive(isActive_Shadder);
        toggle_Logo.SetActive(isActive_Logo);
    }

    public void openprofile(VRCPlayerApi player){
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        buttons_Admin.SetActive(false);
        dropdown.SetActive(false);
        buttons_Member.SetActive(true);
        target = player;
        title_Username.text = target.displayName;
    }

    #endregion

    #region userlist

    public void btn_player_0(){
        if(players[0] !=  null)openprofile(players[0]);
    }

    public void btn_player_1(){
        if(players[1] !=  null)openprofile(players[1]);
    }

    public void btn_player_2(){
        if(players[2] !=  null)openprofile(players[2]);
    }

    public void btn_player_3(){
        if(players[3] !=  null)openprofile(players[3]);
    }

    public void btn_player_4(){
        if(players[4] !=  null)openprofile(players[4]);
    }

    public void btn_player_5(){
        if(players[5] !=  null)openprofile(players[5]);
    }

    public void btn_player_6(){
        if(players[6] !=  null)openprofile(players[6]);
    }

    public void btn_player_7(){
        if(players[7] !=  null)openprofile(players[7]);
    }

    public void btn_player_8(){
        if(players[8] !=  null)openprofile(players[8]);
    }

    public void btn_player_9(){
        if(players[9] !=  null)openprofile(players[9]);
    }

    public void btn_player_10(){
        if(players[10] !=  null)openprofile(players[10]);
    }

    public void btn_player_11(){
        if(players[11] !=  null)openprofile(players[11]);
    }

    public void btn_player_12(){
        if(players[12] !=  null)openprofile(players[12]);
    }

    public void btn_player_13(){
        if(players[13] !=  null)openprofile(players[13]);
    }

    public void btn_player_14(){
        if(players[14] !=  null)openprofile(players[14]);
    }

    public void btn_player_15(){
        if(players[15] !=  null)openprofile(players[15]);
    }

    public void btn_player_16(){
        if(players[16] !=  null)openprofile(players[16]);
    }

    public void btn_player_17(){
        if(players[17] !=  null)openprofile(players[17]);
    }

    public void btn_player_18(){
        if(players[18] !=  null)openprofile(players[18]);
    }

    public void btn_player_19(){
        if(players[19] !=  null)openprofile(players[19]);
    }

    public void btn_player_20(){
        if(players[20] !=  null)openprofile(players[20]);
    }

    public void btn_player_21(){
        if(players[21] !=  null)openprofile(players[21]);
    }

    public void btn_player_22(){
        if(players[22] !=  null)openprofile(players[22]);
    }

    public void btn_player_23(){
        if(players[23] !=  null)openprofile(players[23]);
    }

    public void btn_player_24(){
        if(players[24] !=  null)openprofile(players[24]);
    }

    public void btn_player_25(){
        if(players[25] !=  null)openprofile(players[25]);
    }

    public void btn_player_26(){
        if(players[26] !=  null)openprofile(players[26]);
    }

    public void btn_player_27(){
        if(players[27] !=  null)openprofile(players[27]);
    }

    public void btn_player_28(){
        if(players[28] !=  null)openprofile(players[28]);
    }

    public void btn_player_29(){
        if(players[29] !=  null)openprofile(players[29]);
    }

    public void btn_player_30(){
        if(players[30] !=  null)openprofile(players[30]);
    }

    public void btn_player_31(){
        if(players[31] !=  null)openprofile(players[31]);
    }

    public void btn_player_32(){
        if(players[32] !=  null)openprofile(players[32]);
    }

    public void btn_player_33(){
        if(players[33] !=  null)openprofile(players[33]);
    }

    public void btn_player_34(){
        if(players[34] !=  null)openprofile(players[34]);
    }

    public void btn_player_35(){
        if(players[35] !=  null)openprofile(players[35]);
    }

    public void btn_player_36(){
        if(players[36] !=  null)openprofile(players[36]);
    }

    public void btn_player_37(){
        if(players[37] !=  null)openprofile(players[37]);
    }

    public void btn_player_38(){
        if(players[38] !=  null)openprofile(players[38]);
    }

    public void btn_player_39(){
        if(players[39] !=  null)openprofile(players[39]);
    }

    public void btn_player_40(){
        if(players[40] !=  null)openprofile(players[40]);
    }

    public void btn_player_41(){
        if(players[41] !=  null)openprofile(players[41]);
    }

    public void btn_player_42(){
        if(players[42] !=  null)openprofile(players[42]);
    }

    public void btn_player_43(){
        if(players[43] !=  null)openprofile(players[43]);
    }

    public void btn_player_44(){
        if(players[44] !=  null)openprofile(players[44]);
    }

    public void btn_player_45(){
        if(players[45] !=  null)openprofile(players[45]);
    }

    public void btn_player_46(){
        if(players[46] !=  null)openprofile(players[46]);
    }

    public void btn_player_47(){
        if(players[47] !=  null)openprofile(players[47]);
    }

    public void btn_player_48(){
        if(players[48] !=  null)openprofile(players[48]);
    }

    public void btn_player_49(){
        if(players[49] !=  null)openprofile(players[49]);
    }

    public void btn_player_50(){
        if(players[50] !=  null)openprofile(players[50]);
    }

    public void btn_player_51(){
        if(players[51] !=  null)openprofile(players[51]);
    }

    public void btn_player_52(){
        if(players[52] !=  null)openprofile(players[52]);
    }

    public void btn_player_53(){
        if(players[53] !=  null)openprofile(players[53]);
    }

    public void btn_player_54(){
        if(players[54] !=  null)openprofile(players[54]);
    }

    public void btn_player_55(){
        if(players[55] !=  null)openprofile(players[55]);
    }

    public void btn_player_56(){
        if(players[56] !=  null)openprofile(players[56]);
    }

    public void btn_player_57(){
        if(players[57] !=  null)openprofile(players[57]);
    }

    public void btn_player_58(){
        if(players[58] !=  null)openprofile(players[58]);
    }

    public void btn_player_59(){
        if(players[59] !=  null)openprofile(players[59]);
    }

    public void btn_player_60(){
        if(players[60] !=  null)openprofile(players[60]);
    }

    public void btn_player_61(){
        if(players[61] !=  null)openprofile(players[61]);
    }

    public void btn_player_62(){
        if(players[62] !=  null)openprofile(players[62]);
    }

    public void btn_player_63(){
        if(players[63] !=  null)openprofile(players[63]);
    }

    public void btn_player_64(){
        if(players[64] !=  null)openprofile(players[64]);
    }

    public void btn_player_65(){
        if(players[65] !=  null)openprofile(players[65]);
    }

    public void btn_player_66(){
        if(players[66] !=  null)openprofile(players[66]);
    }

    public void btn_player_67(){
        if(players[67] !=  null)openprofile(players[67]);
    }

    public void btn_player_68(){
        if(players[68] !=  null)openprofile(players[68]);
    }

    public void btn_player_69(){
        if(players[69] !=  null)openprofile(players[69]);
    }

    public void btn_player_70(){
        if(players[70] !=  null)openprofile(players[70]);
    }

    public void btn_player_71(){
        if(players[71] !=  null)openprofile(players[71]);
    }

    public void btn_player_72(){
        if(players[72] !=  null)openprofile(players[72]);
    }

    public void btn_player_73(){
        if(players[73] !=  null)openprofile(players[73]);
    }

    public void btn_player_74(){
        if(players[74] !=  null)openprofile(players[74]);
    }

    public void btn_player_75(){
        if(players[75] !=  null)openprofile(players[75]);
    }

    public void btn_player_76(){
        if(players[76] !=  null)openprofile(players[76]);
    }

    public void btn_player_77(){
        if(players[77] !=  null)openprofile(players[77]);
    }

    public void btn_player_78(){
        if(players[78] !=  null)openprofile(players[78]);
    }

    public void btn_player_79(){
        if(players[79] !=  null)openprofile(players[79]);
    }

    #endregion
}
