using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

// basic WASD-style movement control
public class FpsMovement : MonoBehaviour
{
    [SerializeField] private Camera headCam;

    public Interactable focus;

    public float speed = 6.0f;
    public float gravity = -9.8f;

    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;

    public float minimumVert = -90.0f;
    public float maximumVert = 90.0f;

    private float rotationVert = 0;

    private CharacterController charController;

    public Transform Target;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(PauseMenuScr.GameIsPaused == true)
        {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;

            return;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        MoveCharacter();
        RotateCharacter();
        RotateCamera();
        

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = headCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    RemoveFocus();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = headCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if(interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
    }

    void SetFocus(Interactable newFocus)
    {
        if(newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnDeFocused();
            }
            focus = newFocus;
            focus.OnFocused(transform);
        }

    }

    void RemoveFocus()
    {
        if(focus != null)
        {
            focus.OnDeFocused();
        }
        focus = null;
    }

    private void MoveCharacter()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        //Vector3 movement = transform.right * deltaX + transform.forward * deltaZ;
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y += gravity; //* Time.deltaTime;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        charController.Move(movement);
    }

    private void RotateCharacter()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        //transform.Rotate(0,(Input.GetAxis("Mouse X") * sensitivityHor * Time.deltaTime), 0);
        //this.transform.LookAt(Target);
    }

    private void RotateCamera()
    {
        rotationVert -= Input.GetAxis("Mouse Y") * sensitivityVert;
        //rotationVert -= Input.GetAxis("Mouse Y") + Input.GetAxis("Mouse Y") / Time.deltaTime;
        rotationVert = Mathf.Clamp(rotationVert, minimumVert, maximumVert);

        Vector2 posititionOnScreen = Camera.main.WorldToViewportPoint(headCam.transform.position);

        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = AngleBetweenTwoPoints(posititionOnScreen, mouseOnScreen);

        //headCam.transform.localEulerAngles = new Vector3(
        //    rotationVert, headCam.transform.localEulerAngles.y, 0
        //);

        headCam.transform.localRotation = Quaternion.Euler(rotationVert, 0f, 0f);
        //headCam.transform.localRotation = Quaternion.Euler(new Vector3(mouseOnScreen.x, mouseOnScreen.y, 0f));
        //headCam.transform.LookAt(headCam.transform.position, Target.position);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
