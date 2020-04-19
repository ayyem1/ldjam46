using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    private void Update()
    {
        // Leaving this here as it used to work and I'm not sure why it doesn't anymore.
        //transform.LookAt(Camera.main.transform, Vector3.up);
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}