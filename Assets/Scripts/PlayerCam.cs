using Unity.Netcode;
using UnityEngine;

public class PlayerCam: NetworkBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    public float xRotation;
    public float yRotation;


    private void Start()
    {
        if (!IsOwner)
            this.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;   
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!IsOwner)
            return;
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= +mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
