using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectGenerator : MonoBehaviour
{
    private ObjGenSettings _genSettings;
    private ShapeSettings _shapeSettings;
    public static List<Vector3> spotList = new List<Vector3>();
    List<GameObject> ObjectsList = new List<GameObject>();

    private static List<GameObject> _generatedObjects = new List<GameObject>();

    public static void ClearSpotList()
    {
        spotList.Clear();
    }

    private void Awake()
    {
        spotList.Clear();
        spotList = new List<Vector3>();
        ObjectsList = new List<GameObject>();
    }

    private void CreateObjectList()
    {
        foreach (var layer in _genSettings.objectLayers)
        {
            if (layer.objectPrefab != null && layer.enabled) // Check if objectPrefab is not null
            {
                for (int i = 0; i < layer.rarity; i++)
                {
                    ObjectsList.Add(layer.objectPrefab);
                }
            }
            else
            {
                Debug.LogWarning("objectPrefab is null for one of the layers.");
            }
        }
    }


    public bool ShouldGenerateObject(float chance)
    {
        float randomValue = Random.Range(0.0f, 100.0f);
        return randomValue <= chance;
    }

    GameObject ChoosedObject
    {
        get
        {
            if (ObjectsList.Count == 0)
            {
                Debug.LogWarning("Lista obiektów jest pusta!");
                return null;
            }

            int randomIndex = Random.Range(0, ObjectsList.Count);
            return ObjectsList[randomIndex];
        }
    }

    
    public void GenerateObjects()
    {
        ClearGeneratedObjects();
        CreateObjectList();
        
        Debug.Log(spotList.Count);
        GameObject objects = new GameObject("GeneratedObjects");

        foreach (var spot in spotList)
        {
            if (spot != Vector3.zero & ShouldGenerateObject(_genSettings.spawnChance))
            {
                Debug.Log("wygenerowano obiekt");
                GameObject generatedObject = Instantiate(ChoosedObject, spot.normalized * _shapeSettings.planetRadius, Quaternion.identity, objects.transform);
                _generatedObjects.Add(generatedObject);
            }
        }
    }

    static void ClearGeneratedObjects()
    {
        foreach (var obj in _generatedObjects)
        {
            DestroyImmediate(obj);
        }
        _generatedObjects.Clear(); // Wyczyszczenie listy
    }

    public static Vector3 RandomPointInTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 center = (p1 + p2 + p3) / 3f;
        return center;
    }

    public void UpdateSettings(ObjGenSettings genSettings, ShapeSettings shapeSettings)
    {
        _genSettings = genSettings;
        _shapeSettings = shapeSettings;
    }
}