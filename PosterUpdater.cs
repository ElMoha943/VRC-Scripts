using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Image;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PosterUpdater : UdonSharpBehaviour
{
    [SerializeField, Tooltip("URL of the image to load")]
    VRCUrl imageUrl;
    
    [SerializeField, Tooltip("Renderer to show downloaded image.")]
    new Renderer renderer;
    
    VRCImageDownloader _imageDownloader;
    IUdonEventReceiver _udonEventReceiver;
    
    private void Start()
    {
        _imageDownloader = new VRCImageDownloader();
        _udonEventReceiver = (IUdonEventReceiver)this;
        var rgbInfo = new TextureInfo();
        rgbInfo.GenerateMipMaps = true;
        _imageDownloader.DownloadImage(imageUrl, renderer.material, _udonEventReceiver, rgbInfo);
    }

    public override void OnImageLoadSuccess(IVRCImageDownload result)
    {
        Debug.Log($"Image loaded: {result.SizeInMemoryBytes} bytes.");
    }

    public override void OnImageLoadError(IVRCImageDownload result)
    {
        Debug.Log($"Image not loaded: {result.Error.ToString()}: {result.ErrorMessage}.");
    }

    private void OnDestroy()
    {
        _imageDownloader.Dispose();
    }
}
