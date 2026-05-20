using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera mapCamera;

    private bool mapViewActive;

    void Start()
    {
        mainCamera.enabled = true;
        mapCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapViewActive = !mapViewActive;

            mainCamera.enabled = !mapViewActive;
            mapCamera.enabled = mapViewActive;
        }
    }
}
