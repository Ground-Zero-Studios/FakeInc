using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Player : MonoBehaviour
{
    public Camera playerCamera;
    public float cameraSpeed = 1f;
    public float zoomSpeed = 2f;
    public float speedMultiplier = 2f;
    public float zoomMultiplier = 2f;

    public GameObject worldMap; // Game Area Object

    public static Country playerCountry;
    private float cameraZoom; 

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        cameraZoom = playerCamera.orthographicSize;

        if (GameManager.player == null)
        {
            GameManager.player = this;
        }


    }


    public bool IsOutOfBounds()
    {
        return IsOutOfBounds(transform.position);
    }
    public bool IsOutOfBounds(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(position);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject == worldMap)
            {
                return false;
            }
        }
        return true;
    }

    public float CameraSpeedMultiplier()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return 1.5f;
        } else
        {
            return 1f;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Events.Call(Events.firstPop);
        }

        if (!GameManager.instance.isUIOpen())
        {
            float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            cameraZoom -= mouseWheel * zoomMultiplier;
            playerCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(playerCamera.orthographicSize, cameraZoom, Time.deltaTime * zoomSpeed),1f,8f);



            if ((Input.mousePosition.y >= Screen.height * 0.95) || (Input.GetKey(KeyCode.W)))
            {
                // Move Up
                Vector2 targetPos = (Vector2) transform.position + Vector2.up;
                if (!IsOutOfBounds(targetPos))
                {
                    transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime * CameraSpeedMultiplier());
                }

            }
            else
            if ((Input.mousePosition.y <= Screen.height * 0.05) || (Input.GetKey(KeyCode.S)))
            {
                // Move Down
                Vector2 targetPos = (Vector2)transform.position + Vector2.down;
                if (!IsOutOfBounds(targetPos))
                {
                    transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime * CameraSpeedMultiplier());
                }
            }

            if ((Input.mousePosition.x >= Screen.width * 0.95) || (Input.GetKey(KeyCode.D)))
            {
                // Move Right
                Vector2 targetPos = (Vector2)transform.position + Vector2.right;
                if (!IsOutOfBounds(targetPos))
                {
                    transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime * CameraSpeedMultiplier());
                }
            }
            else
            if ((Input.mousePosition.x <= 0.05) || (Input.GetKey(KeyCode.A)))
            {
                // Move Left
                Vector2 targetPos = (Vector2)transform.position + Vector2.left;
                if (!IsOutOfBounds(targetPos))
                {
                    transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime * CameraSpeedMultiplier());
                }
            }
        }
    }

}
