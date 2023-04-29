using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
using VRC.SDK3.StringLoading;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class SimpleStringLoader : UdonSharpBehaviour
{
    [SerializeField] VRCUrl url;
    [SerializeField] TextMeshProUGUI text;
    IUdonEventReceiver EventReceiver;

    void Start(){
        EventReceiver = (IUdonEventReceiver)this;
        VRCStringDownloader.LoadUrl(url, EventReceiver);
    }

    public override void OnStringLoadSuccess(IVRCStringDownload result){
        text.text = result.Result;
    }

    public override void OnStringLoadError(IVRCStringDownload result){
        Debug.Log("Error loading string: " + result.ErrorCode + " - " + result.Error);
    }
}
