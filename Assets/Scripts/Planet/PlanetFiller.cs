using System.Collections.Generic;
using UnityEngine;

public class PlanetFiller : MonoBehaviour
{
    private List<Vector3> mountainPointsDown;
    private List<Vector3> mountainPointsUp;
    private int resolution;

    public GameObject vertexPrefab; // Prefab do generowania na wierzchołkach

    public PlanetFiller(int res)
    {
        this.resolution = res;
    }

    //void Start()
    //{
    //    mountainPointsDown = ShapeGenerator.GetmountinePointsDown();
    //    mountainPointsUp = ShapeGenerator.GetmountinePointsUP();
    //    
    //    Debug.Log(mountainPointsDown.Count + " liczba punktow");
    //    CreateGrid();
    //}

    void CreateGrid()
    {
        GameObject gridObject = new GameObject("Grid");
        gridObject.transform.parent = transform;

        for (int i = 0; i < mountainPointsDown.Count; i++)
        {
            Vector3 bottomPoint = mountainPointsDown[i];
            Vector3 topPoint = mountainPointsUp[i];

            float stepSize = Vector3.Distance(bottomPoint, topPoint) / (resolution - 1);

            for (int j = 0; j < resolution; j++)
            {
                if (j == 0 || (resolution > 2 && j == resolution - 1))
                {
                    Vector3 vertexPosition = Vector3.Lerp(bottomPoint, topPoint, (float)j / (resolution - 1));

                    GameObject vertex = Instantiate(vertexPrefab, vertexPosition, Quaternion.identity);
                    vertex.transform.parent = gridObject.transform;

                    Debug.Log("Utworzono prefabrykat na pozycji: " + vertexPosition);
                }
                else
                {
                    Debug.Log("Nie utworzono prefabrykatu na pozycji: " + Vector3.Lerp(bottomPoint, topPoint, (float)j / (resolution - 1)));
                }
            }
        }
    }
}