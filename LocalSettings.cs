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

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LocalSettings : UdonSharpBehaviour
{
    [Header("Toggleable Objects")]
    [SerializeField] GameObject Lights;
    [SerializeField] GameObject Disco;
    [SerializeField] GameObject Shadders;
    [SerializeField] GameObject Season;
    [SerializeField] GameObject PotatoStageOn;
    [SerializeField] GameObject PotatoStageOff;
    [SerializeField] GameObject PostProcessing;
    [SerializeField] GameObject PickUps;
    [SerializeField] GameObject[] Colliders;

    [Header("Gui Elements, dont touch this!")]
    [SerializeField] GameObject btn_Lights;
    [SerializeField] GameObject btn_Disco;
    [SerializeField] GameObject btn_Potato;
    [SerializeField] GameObject btn_Shadder;
    [SerializeField] GameObject btn_Season;
    [SerializeField] GameObject btn_PickUps;
    [SerializeField] GameObject btn_Colliders;

    public void ButtonPress_PotatoStage()
    {
        // Toggle Potato Stage
        PotatoStageOn.SetActive(!PotatoStageOn.activeSelf);
        PotatoStageOff.SetActive(!PotatoStageOff.activeSelf);
        // Change the button color
        btn_Potato.GetComponent<Image>().color = PotatoStageOn.activeSelf ? Color.green : Color.red;
    }

    public void ButtonPress_PostProcessing(){
        PostProcessing.SetActive(!PostProcessing.activeSelf);
        btn_Shadder.GetComponent<Image>().color = PostProcessing.activeSelf ? Color.green : Color.red;
    }

    public void ButtonPress_PickUps(){
        PickUps.SetActive(!PickUps.activeSelf);
        btn_PickUps.GetComponent<Image>().color = PickUps.activeSelf ? Color.green : Color.red;
    }

    public void ButtonPress_Colliders(){
        for (int i = 0; i < Colliders.Length; i++)
            Colliders[i].SetActive(!Colliders[i].activeSelf);
        btn_Colliders.GetComponent<Image>().color = Colliders[0].activeSelf ? Color.green : Color.red;
    }

    public void ButtonPress_Lights(){
        Lights.SetActive(!Lights.activeSelf);
        btn_Lights.GetComponent<Image>().color = Lights ? Color.green : Color.red;
    }

    public void ButtonPress_Disco(){
        Disco.SetActive(!Disco.activeSelf);
        btn_Disco.GetComponent<Image>().color = Disco ? Color.green : Color.red;
    }

    public void ButtonPress_Shadders(){
        Shadders.SetActive(!Shadders.activeSelf);
        btn_Shadder.GetComponent<Image>().color = Shadders ? Color.green : Color.red;
    }

    public void ButtonPress_Season(){
        Season.SetActive(!Season.activeSelf);
        btn_Season.GetComponent<Image>().color = Season ? Color.green : Color.red;
    }
}
