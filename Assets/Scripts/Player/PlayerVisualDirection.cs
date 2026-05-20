using UnityEngine;

public class PlayerVisualDirection : MonoBehaviour
{
    public Transform visualRoot;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (visualRoot == null)
        {
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;

        if (mouseWorldPosition.x < transform.position.x)
        {
            visualRoot.localScale = new Vector3(-Mathf.Abs(visualRoot.localScale.x), visualRoot.localScale.y, visualRoot.localScale.z);
        }
        else
        {
            visualRoot.localScale = new Vector3(Mathf.Abs(visualRoot.localScale.x), visualRoot.localScale.y, visualRoot.localScale.z);
        }
    }
}