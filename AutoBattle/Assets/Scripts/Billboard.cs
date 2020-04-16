using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform anchor;
    private Transform cam;
    private Vector3 offset = new Vector3(0, 0, 1.5f);

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        transform.position = anchor.position + offset;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
