using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class PanelActivator : UdonSharpBehaviour
{
    [SerializeField] Button[] collection;

    void OnEnable(){
        foreach (Button btn in collection)
            btn.interactable = true;
    }

    void OnDisable(){
        foreach (Button btn in collection)
            btn.interactable = false;
    }
}
