using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform Target;
    public Vector3 offset = new Vector3(0,0, 1.0f);
    public float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(Target.localPosition.x- offset.x, Target.localPosition.y - offset.y, Target.localPosition.z - offset.z), speed*Time.deltaTime);
    }
}
