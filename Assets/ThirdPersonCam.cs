using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform orientation;
    [SerializeField] public Transform player;
    [SerializeField] public Transform playerObj;
    [SerializeField] float speed;

    [SerializeField] private Rigidbody _rb;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = Vector3.Slerp(orientation.forward, viewDir.normalized, Time.deltaTime * speed);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if (inputDir != Vector3.zero)
        {
            player.forward = Vector3.Slerp(player.forward, orientation.forward, Time.deltaTime * speed);

        }
        if (Input.GetButton("Fire1"))
        {
            player.forward = Vector3.Slerp(player.forward, orientation.forward, Time.deltaTime * speed *5);
        }
        else if (verticalInput > 0f)
        {
            player.forward = Vector3.Slerp(player.forward, orientation.forward, Time.deltaTime * speed);
        }
    }

}