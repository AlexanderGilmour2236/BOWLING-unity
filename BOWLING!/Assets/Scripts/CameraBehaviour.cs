using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0,0, 1.0f);
    public float speed = 0.5f;
    public GameObject[] cameraPoints;
    public Camera Camera { get; private set; }

    private void Start()
    {
        Camera = GetComponent<Camera>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.localPosition.x- offset.x, target.localPosition.y - offset.y, target.localPosition.z - offset.z), speed*Time.deltaTime);
    }
}
