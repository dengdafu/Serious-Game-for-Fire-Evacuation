using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 90.0f;

    public Vector3 LOOK_AT = new Vector3(0, 0, 0);
    public Transform CAM_TRANSFORM;

    private Camera CAM;

    private float DISTANCE = 10.0f;
    private float CURRENT_X = 0.0f;
    private float CURRENT_Y = 60.0f;
    private float SENSIVITY_X = 4.0f;
    private float SENSIVITY_Y = 1.0f;
    private float TRANSFORMING_RATE = 0.2f;

    private bool LEFT_MOUSE_CLICKED = false;
    private bool RIGHT_MOUSE_CLICKED = false;
    private bool W_KEY_PRESSED = false;
    private bool S_KEY_PRESSED = false;
    private bool A_KEY_PRESSED = false;
    private bool D_KEY_PRESSED = false;
    private bool Q_KEY_PRESSED = false;
    private bool E_KEY_PRESSED = false;

    private Ray RAY;
    private RaycastHit HIT;
    private GameObject FOCUSED_GAMEOJECT;
    private GameObject PRE_FOCUSED_GAMEOJECT;
    private int LEFT_MOUSE_CLICK_COUNT = 0;
    private float LEFT_MOUSE_CLICK_TIME = 0;

    private void Start()
    {
        CAM_TRANSFORM = this.transform;
        CAM = Camera.main;
    }

    private void Update()
    {
        // Transformation
        if (Input.GetMouseButtonDown(0))
        {
            LEFT_MOUSE_CLICKED = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            LEFT_MOUSE_CLICKED = false;
        }
        if (LEFT_MOUSE_CLICKED)
        {
            LOOK_AT += (-0.5f * Input.GetAxis("Mouse X")) * CAM.transform.right + (-0.5f * Input.GetAxis("Mouse Y")) * CAM.transform.up;
        }
        if (Input.GetKeyDown("w"))
        {
            W_KEY_PRESSED = true;
        }
        if (Input.GetKeyUp("w"))
        {
            W_KEY_PRESSED = false;
        }
        if (W_KEY_PRESSED)
        {
            LOOK_AT += TRANSFORMING_RATE * CAM.transform.forward;
        }
        if (Input.GetKeyDown("s"))
        {
            S_KEY_PRESSED = true;
        }
        if (Input.GetKeyUp("s"))
        {
            S_KEY_PRESSED = false;
        }
        if (S_KEY_PRESSED)
        {
            LOOK_AT += -TRANSFORMING_RATE * CAM.transform.forward;
        }
        if (Input.GetKeyDown("a"))
        {
            A_KEY_PRESSED = true;
        }
        if (Input.GetKeyUp("a"))
        {
            A_KEY_PRESSED = false;
        }
        if (A_KEY_PRESSED)
        {
            LOOK_AT += -TRANSFORMING_RATE * CAM.transform.right;
        }
        if (Input.GetKeyDown("d"))
        {
            D_KEY_PRESSED = true;
        }
        if (Input.GetKeyUp("d"))
        {
            D_KEY_PRESSED = false;
        }
        if (D_KEY_PRESSED)
        {
            LOOK_AT += TRANSFORMING_RATE * CAM.transform.right;
        }
        if (Input.GetKeyDown("q"))
        {
            Q_KEY_PRESSED = true;
        }
        if (Input.GetKeyUp("q"))
        {
            Q_KEY_PRESSED = false;
        }
        if (Q_KEY_PRESSED)
        {
            LOOK_AT += 0.707f * (TRANSFORMING_RATE * CAM.transform.forward - TRANSFORMING_RATE * CAM.transform.right);
        }
        if (Input.GetKeyDown("e"))
        {
            E_KEY_PRESSED = true;
        }
        if (Input.GetKeyUp("e"))
        {
            E_KEY_PRESSED = false;
        }
        if (E_KEY_PRESSED)
        {
            LOOK_AT += 0.707f * (TRANSFORMING_RATE * CAM.transform.forward + TRANSFORMING_RATE * CAM.transform.right);
        }

        if (LOOK_AT.y < 0)
        {
            LOOK_AT.y = 0;
        }

        // Rotation around LOOK_AT
        if (Input.GetMouseButtonDown(1))
        {
            RIGHT_MOUSE_CLICKED = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            RIGHT_MOUSE_CLICKED = false;
        }
        if (RIGHT_MOUSE_CLICKED)
        {
            CURRENT_X += Input.GetAxis("Mouse X");
            CURRENT_Y += -Input.GetAxis("Mouse Y");
            CURRENT_Y = Mathf.Clamp(CURRENT_Y, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }

        // Zoom in and zoom out
        DISTANCE -= 2 * Input.GetAxis("Mouse ScrollWheel");
        if (DISTANCE < 1)
        {
            DISTANCE = 1;
        }

        // Upon double clicking a game object, set focus to that game object
        RAY = CAM.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(RAY, out HIT))
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (PRE_FOCUSED_GAMEOJECT == null)
                {
                    PRE_FOCUSED_GAMEOJECT = HIT.transform.gameObject;
                }
                FOCUSED_GAMEOJECT = HIT.transform.gameObject;
                if (FOCUSED_GAMEOJECT != PRE_FOCUSED_GAMEOJECT)
                {
                    PRE_FOCUSED_GAMEOJECT.GetComponent<MeshRenderer>().materials[0].DisableKeyword("_EMISSION");
                    PRE_FOCUSED_GAMEOJECT = FOCUSED_GAMEOJECT;
                    LEFT_MOUSE_CLICK_COUNT = 0;
                }
                if (LEFT_MOUSE_CLICK_COUNT == 0)
                {
                    FOCUSED_GAMEOJECT.GetComponent<MeshRenderer>().materials[0].EnableKeyword("_EMISSION");
                    LEFT_MOUSE_CLICK_COUNT += 1;
                    LEFT_MOUSE_CLICK_TIME = Time.time;
                }
                else if (LEFT_MOUSE_CLICK_COUNT == 1)
                {
                    FOCUSED_GAMEOJECT.GetComponent<MeshRenderer>().materials[0].DisableKeyword("_EMISSION");
                    LOOK_AT = FOCUSED_GAMEOJECT.transform.position;
                    CURRENT_X = 0;
                    CURRENT_Y = 45.0f;
                    DISTANCE = 10.0f;
                    LEFT_MOUSE_CLICK_COUNT = 0;
                }
            }
        }
        if (Time.time - LEFT_MOUSE_CLICK_TIME >= 1)
        {
            LEFT_MOUSE_CLICK_COUNT = 0;
        }


    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -DISTANCE);
        Quaternion rotation = Quaternion.Euler(CURRENT_Y, CURRENT_X, 0);
        CAM_TRANSFORM.position = LOOK_AT + rotation * dir;
        CAM_TRANSFORM.LookAt(LOOK_AT);
    }
}