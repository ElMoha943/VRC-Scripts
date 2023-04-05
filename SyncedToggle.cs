public class SyncedToggle : UdonSharpBehaviour
{
    [SerializeField] GameObject[] objectsToToggle;
    bool state = false;


    void Start(){
        foreach (GameObject item in objectsToToggle)
            item.SetActive(state);
    }

    public override void Interact(){
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        state = !state;
        foreach (GameObject item in objectsToToggle)
            item.SetActive(state);
    }

    public override void OnDeserialization(){
        foreach (GameObject item in objectsToToggle)
            item.SetActive(state);
    }
}
