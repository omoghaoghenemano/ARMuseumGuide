using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject arFoundationCamera;



    public void SwitchToARFoundation()
    {
        if (arFoundationCamera == null   )
        {
            Debug.LogError("Cameras are not assigned in the inspector.");
            return;
        }
        arFoundationCamera.SetActive(true);
        Debug.Log("Switched to AR Foundation camera.");
    }
}
