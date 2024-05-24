using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera camera;
    private Transform cameraTF;
    public Transform playerTF;
    private Rigidbody2D cameraRB;

    void Start()
    {
        camera = GetComponent<Camera>();
        cameraTF = GetComponent<Transform>();
        cameraRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        cameraRB.velocity = (playerTF.position + new Vector3(0f, 3.5f) - cameraTF.position) * 5f;
    }
}
