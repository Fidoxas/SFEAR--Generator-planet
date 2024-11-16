using System;
using Unity.VisualScripting;
using UnityEngine;

public class Gravitation : MonoBehaviour
{
    public float gravityStrength = 9.81f;
    private Transform planetCenter;
    private Vector3 gravityDirection;
    private float gravityForce;
    private float airTime = 0f;
    private Rigidbody rb;
    [SerializeField] GameObject Gpole;
    private PMove pMove;

    void Start()
    {
        planetCenter = GameObject.FindGameObjectWithTag("PlanetCenter")?.transform;
        rb = GetComponent<Rigidbody>();
        pMove = GetComponent<PMove>();
        if (planetCenter == null)
        {
            Debug.LogError("Nie znaleziono obiektu o tagu 'PlanetCenter'. Upewnij się, że obiekt ten posiada poprawny tag.");
        }
    }

    void FixedUpdate()
    {
        Gravity();
        //Gpole.transform.rotation = Quaternion.FromToRotation(Vector3.up, gravityDirection);
    }

    private void Gravity()
    {
        if (pMove != null && !pMove.grounded)
        {
            if (gravityForce < 2000)
            {
                airTime++;
            }
            Vector3 directionToCenter = planetCenter.position - transform.position;
            gravityDirection = directionToCenter.normalized;
            gravityForce = gravityStrength * airTime;
            rb.AddForce(gravityDirection * gravityForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
        else if (pMove.grounded && pMove != null)
        {
            airTime = 0f;
        }
    }
}