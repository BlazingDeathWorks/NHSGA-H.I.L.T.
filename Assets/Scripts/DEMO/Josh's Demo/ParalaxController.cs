using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxController : MonoBehaviour
{
    [SerializeField]
    private float paralaxScale;

    private Vector3 initialOffset;
    private Camera cam;

    void Start()
    {
        initialOffset = transform.localPosition;
        cam = Camera.main;
    }
    void LateUpdate()
    {
        transform.localPosition = initialOffset + new Vector3(cam.transform.position.x * paralaxScale, 0, 10);
        if (transform.localPosition.x < cam.orthographicSize * cam.aspect * -2f)
        {
            initialOffset += Vector3.right * cam.orthographicSize * cam.aspect * 4;
        }
        if (transform.localPosition.x > cam.orthographicSize * cam.aspect * 2f)
        {
            initialOffset -= Vector3.right * cam.orthographicSize * cam.aspect * 4;
        }
    }
}
