using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;

namespace Labthe3rd.Keypad.Keypad_Main
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Keypad_Main : UdonSharpBehaviour
    {
        [Header("Passwords")]
        public string adminPassword;
        public string staffPassword;
        public string dJPassword;
        public string vIPPassword;

        [Header("Set the objects you want to show and hide for each position")]
        [Header("Admin Show And Hide Objects")]
        public GameObject[] adminShowObjects;
        public GameObject[] adminHideObjects;
        [Header("Staff Show And Hide Objects")]
        public GameObject[] staffShowObjects;
        public GameObject[] staffHideObjects;
        [Header("DJ Show And Hide Objects")]
        public GameObject[] dJShowObjects;
        public GameObject[] dJHideObjects;
        [Header("VIP Show And Hide Objects")]
        public GameObject[] vIPShowObjects;
        public GameObject[] vIPHideObjects;

        [Header("Internal UI Stuff")]
        public TextMeshProUGUI InputScreen;

        private VRCPlayerApi player;
        private string inputString;
        private bool loggedIn = false;

        [HideInInspector] public string Key;

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
}
