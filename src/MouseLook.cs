using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;

    [SerializeField] private Transform playerBody;

    float xRotation = 0f;
    bool MousePressed = false;
    bool MousePressedRight = false;
    float correctMouse;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        correctMouse = mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        float SlowMouse = mouseSensitivity / 3f;
          

        MousePressed = Input.GetMouseButton(0);
        MousePressedRight = Input.GetMouseButton(1);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector2.up * mouseX);
        if(MousePressed == false && MousePressedRight == false) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            mouseSensitivity = correctMouse;
        } 
        else
        {
            mouseSensitivity = SlowMouse;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }
    }
}
