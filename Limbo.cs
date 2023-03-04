using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Limbo : UdonSharpBehaviour
{
    [Tooltip("Object to move")]
    [SerializeField] GameObject limboBar;
    [Tooltip("Distance to move the object on Y axis")]
    public float distance = 0.5f;
    [Tooltip("Amount of levels the bar can go down")]
    public int maxLevel = 10;

    [UdonSynced(UdonSyncMode.None)]
    int position; // 0 = original, up to 5 down

    Vector3 originalPosition;

    void Start()
    {
        originalPosition = limboBar.transform.position;
    }

    public void _moveDown()
    {
        Networking.SetOwner(Networking.LocalPlayer,gameObject);
        if(position!=0){
            position--;
            RequestSerialization();
            limboBar.transform.position = originalPosition + new Vector3(0, -distance * position, 0);
        }
    }

    public void _moveUp()
    {
        Networking.SetOwner(Networking.LocalPlayer,gameObject);
        if(position!=maxLevel){
            position++;
            RequestSerialization();
            limboBar.transform.position = originalPosition + new Vector3(0, -distance * position, 0);
        }
    }

    public override void OnDeserialization()
    {
        limboBar.transform.position = originalPosition + new Vector3(0, -distance * position, 0);
    }
}
