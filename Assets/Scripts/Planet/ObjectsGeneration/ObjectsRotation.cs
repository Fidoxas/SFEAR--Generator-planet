using UnityEngine;

public class ObjectsRotation : MonoBehaviour
{
    private Transform planetCenter;

    void Awake()
    {
        planetCenter = GameObject.FindGameObjectWithTag("PlanetCenter").transform;
    }

    void Update()
    {
        if (planetCenter != null)
        {
            Vector3 directionToCenter = planetCenter.position - transform.position;
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -directionToCenter.normalized) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Adjust the speed with the last parameter
        }
    }
}