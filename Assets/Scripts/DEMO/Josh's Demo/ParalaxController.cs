using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxController : MonoBehaviour
{
    [SerializeField]
    private float paralaxScale;

    private Vector3 initialOffset;
    private Camera cam;
    private float camSize;

    void Start()
    {
        cam = Camera.main;
        camSize = cam.orthographicSize * cam.aspect;
        transform.localScale = new Vector3(camSize / 10f, camSize / 10f);
        initialOffset = new Vector3(camSize, 0) * (transform.localPosition.x < 0 ? -1 : 1);
    }
    void LateUpdate()
    {
        transform.localPosition = initialOffset + new Vector3(cam.transform.position.x * paralaxScale, 0, 10);
        if (transform.localPosition.x < camSize * -2f)
        {
            initialOffset += Vector3.right * camSize * 4;
        }
        if (transform.localPosition.x > camSize * 2f)
        {
            initialOffset -= Vector3.right * camSize * 4;
        }
    }
}
