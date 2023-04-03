public class Invoke : UdonSharpBehaviour
{
    [SerializeField] string methodName;
    [SerializeField] UdonBehaviour target;

    public override void Interact(){
        target.SendCustomEvent(methodName);
    }
}
