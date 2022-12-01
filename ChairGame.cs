using System.Collections;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using TMPro;
using UnityEngine.UI; 

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ChairGame : UdonSharpBehaviour
{
    [UdonSynced(UdonSyncMode.None)] private int chairCount;
    [UdonSynced(UdonSyncMode.None)] private int activeChairs;
    [UdonSynced(UdonSyncMode.None)] private int action;
    [UdonSynced(UdonSyncMode.None)] private int seed;
    //[UdonSynced(UdonSyncMode.None)] private float[] posX,posZ;

    public float spread;
    public GameObject initialPosition,sillas_parent;
    public Slider sldr_silla;
    public Text txt_silla, Start_but;
    public Button Next_but;

    int size;
    float posx, posz;

    public void Start(){
        size = sillas_parent.transform.childCount;
        sldr_silla.maxValue = size;
        activeChairs = (int)sldr_silla.value;
        Next_but.interactable=false;
        seed = Random.Range(0, 100000);
        //posX = new float[size];
        //posZ = new float[size];
    }

    public void NextRound(){
        Debug.Log("NextRound");
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        if (activeChairs > 0){
            activeChairs--;
            action = 1;
            /* for (int i = 0; i < size; i++){
                posX[i] = Random.Range(-spread, spread);
                posZ[i] = Random.Range(-spread, spread);
            } */
            seed = Random.Range(0, 100000);
            RequestSerialization();
            action1();
            if (activeChairs == 1) { Start_but.text = "Start"; Next_but.interactable = false; Debug.Log("EndGame"); }
        }
    }

    public void HideChairs(){
        Debug.Log("Chairs Hidden");
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        action = 2;
        Next_but.interactable = true;
        RequestSerialization();
        action2();
    }

    public void ResetGame(){
        Debug.Log("ResetGame");
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        Start_but.text = "Reset";
        Next_but.interactable = true;
        activeChairs = (int)sldr_silla.value;
        action = 3;
/*         for (int i = 0; i < size; i++)
        {
            posX[i] = Random.Range(-spread, spread);
            posZ[i] = Random.Range(-spread, spread);
        } */
        seed = Random.Range(0, 100000);
        RequestSerialization();
        action3();
    }

    public void ChairsCountChanged(){
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        // update the text
        Debug.Log("ChairsCountChanged: " + sldr_silla.value);
        txt_silla.text = "" + sldr_silla.value;
    }

    public void action1(){
        Debug.Log("action1");
        Random.InitState(seed);
        for (int i = 0; i < size; i++){
            // generate random numbers
            posx = Random.Range(-spread, spread);
            posz = Random.Range(-spread, spread);
            // disable all chairs after the activeChairs index
            sillas_parent.transform.GetChild(i).gameObject.SetActive(i < activeChairs);
            // move the chairs to a random position within a radius
            sillas_parent.transform.GetChild(i).gameObject.transform.localPosition = initialPosition.transform.localPosition + new Vector3(posx, 0, posz);
        }
    }

    public void action2(){
        Debug.Log("action2");
        for (int i = 0; i < size; i++){
            // disable all chairs
            sillas_parent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void action3(){
        Debug.Log("action3");
        Random.InitState(seed);
        for (int i = 0; i < activeChairs; i++)
        {
            // generate random numbers
            posx = Random.Range(-spread, spread);
            posz = Random.Range(-spread, spread);
            // enable all chairs and reset position
            sillas_parent.transform.GetChild(i).gameObject.SetActive(true);
            sillas_parent.transform.GetChild(i).gameObject.transform.localPosition = initialPosition.transform.localPosition;
            sillas_parent.transform.GetChild(i).position = sillas_parent.transform.GetChild(i).position + new Vector3(posx, 0, posz);
        }
    }

    public override void OnDeserialization(){
        Debug.Log("OnDeserialization, action: " + action);
            if(action == 1 ) action1();
            else if(action == 2) action2();
            else if(action == 3) action3();
    }
}
