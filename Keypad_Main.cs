using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Keypad_Main : UdonSharpBehaviour
{
    [Tooltip("Set this to true if you the master of the instance to be logged as Admin")]
    public bool isMasterAdmin = false;

    [Header("Passwords")]
    [SerializeField] string adminPassword;
    [SerializeField] string staffPassword;
    [SerializeField] string dJPassword;
    [SerializeField] string vIPPassword;

    [Header("Write Admin Display Names Here")]
    public string[] Admin;
    [Header("Write Staff Display Names Here")]
    public string[] Staff;
    [Header("Write DJ Display Names Here")]
    public string[] DJ;
    [Header("Write VIP Display Names Here")]
    public string[] VIP;

    [Header("Set the objects you want to show and hide for each position")]
    [Header("Admin Show And Hide Objects")]
    [SerializeField] GameObject[] adminShowObjects;
    [SerializeField] GameObject[] adminHideObjects;
    [Header("Staff Show And Hide Objects")]
    [SerializeField] GameObject[] staffShowObjects;
    [SerializeField] GameObject[] staffHideObjects;
    [Header("DJ Show And Hide Objects")]
    [SerializeField] GameObject[] dJShowObjects;
    [SerializeField] GameObject[] dJHideObjects;
    [Header("VIP Show And Hide Objects")]
    [SerializeField] GameObject[] vIPShowObjects;
    [SerializeField] GameObject[] vIPHideObjects;

    [Header("Internal UI Stuff")]
    [SerializeField] TextMeshProUGUI InputScreen;

    VRCPlayerApi player;
    string inputString;
    bool loggedIn = false;

    [HideInInspector] public string Key;

    public void Start(){
        Debug.Log("Setting Tags");

        //Make sure local player is valid
        if (!Utilities.IsValid(Networking.LocalPlayer)) return;

        //Get variables since GET is expensive
        VRCPlayerApi targetPlayer = Networking.LocalPlayer;
        string displayName = targetPlayer.displayName;

        // Autologin Master
        if (isMasterAdmin && targetPlayer.isMaster){
            AdminLogin(targetPlayer);
            Debug.Log(displayName + " has been given the tag Admin because they are the master of the instance");
            return;
        }

        //Loop through admin names then send staff auto login to keypad
        for (int i = 0; i < Admin.Length; i++){
            if (string.IsNullOrEmpty(Admin[i])) continue;
            if (Admin[i] == displayName){	
                AdminLogin(targetPlayer);
                Debug.Log(displayName + " has been given the tag Admin");
                return;
            }
        }

        //Loop through staff names then send staff auto login to keypad
        for (int i = 0; i < Staff.Length; i++){
            if (string.IsNullOrEmpty(Staff[i])) continue;
            if (Staff[i] == displayName){
                StaffLogin(targetPlayer);
                Debug.Log(displayName + " has been given the tag Staff");
                return;
            }
        }

        //Loop through DJ names then send DJ auto login to keypad
        for (int i = 0; i < DJ.Length; i++){
            if (string.IsNullOrEmpty(DJ[i])) continue;
            if (DJ[i] == displayName){
                DJLogin(targetPlayer);
                Debug.Log(displayName + " has been given the tag DJ");
                return;
            }
        }

        //Loop through VIP names then send VIP auto login to keypad
        for (int i = 0; i < VIP.Length; i++){
            if (string.IsNullOrEmpty(VIP[i])) continue;
            if (VIP[i] == displayName){
                VIPLogin(targetPlayer);
                Debug.Log(displayName + " has been given the tag VIP");
                return;
            }
        }
    }

    public void KeyPressed(){
        if (loggedIn == true) return;
        inputString = string.Concat(inputString, Key);
        InputScreen.text = inputString;
    }

    public void Enter(){
        player = Networking.LocalPlayer;
        if (loggedIn == true) return;
        if (!string.IsNullOrEmpty(inputString)){
            if (inputString == adminPassword) AdminLogin(player);
            else if (inputString == staffPassword) StaffLogin(player);
            else if (inputString == dJPassword) DJLogin(player);
            else if (inputString == vIPPassword) VIPLogin(player);
            else{
                InputScreen.text = "INVALID";
                inputString = "";
            }
        }
    }

    public void AdminLogin(VRCPlayerApi player){
        if (loggedIn == true) return;
        ToggleObjects(adminShowObjects, adminHideObjects);
        ToggleObjects(staffShowObjects, staffHideObjects);
        ToggleObjects(dJShowObjects, dJHideObjects);
        ToggleObjects(vIPShowObjects, vIPHideObjects);
        loggedIn = true;
        player.SetPlayerTag("Position", "Admin");
        InputScreen.text = "ADMIN LOGGED IN";
        inputString = "";
    }

    public void StaffLogin(VRCPlayerApi player){
        if (loggedIn == true) return;
        ToggleObjects(staffShowObjects, staffHideObjects);
        ToggleObjects(dJShowObjects, dJHideObjects);
        ToggleObjects(vIPShowObjects, vIPHideObjects);
        loggedIn = true;
        player.SetPlayerTag("Position", "Staff");
        InputScreen.text = "STAFF LOGGED IN";
        inputString = "";
    }

    public void DJLogin(VRCPlayerApi player){
        if (loggedIn == true) return;
        ToggleObjects(dJShowObjects, dJHideObjects);
        ToggleObjects(vIPShowObjects, vIPHideObjects);
        loggedIn = true;
        player.SetPlayerTag("Position", "DJ");
        InputScreen.text = "D-J LOGGED IN";
        inputString = "";
    }

    public void VIPLogin(VRCPlayerApi player){
        if (loggedIn == true) return;
        ToggleObjects(vIPShowObjects, vIPHideObjects);
        loggedIn = true;
        player.SetPlayerTag("Position", "VIP");
        InputScreen.text = "VIP LOGGED IN";
        inputString = "";
    }

    public void Clear(){
        inputString = "";
        InputScreen.text = inputString;
        loggedIn = false;
    }

    private void ToggleObjects(GameObject[] ObjectArray, GameObject[] ObjectArray2){
        for (int i = 0; i < ObjectArray.Length; i++)
            if (Utilities.IsValid(ObjectArray[i]))
                ObjectArray[i].SetActive(true);
        for (int i = 0; i < ObjectArray2.Length; i++)
            if (Utilities.IsValid(ObjectArray2[i]))
                ObjectArray2[i].SetActive(false);
    }
}
