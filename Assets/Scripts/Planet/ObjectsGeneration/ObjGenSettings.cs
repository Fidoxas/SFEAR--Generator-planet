 using System;
using UnityEngine;

[CreateAssetMenu]
public class ObjGenSettings : ScriptableObject
{
    public ObjectLayer[] objectLayers;
    [Range(1f, 100f)] public float spawnChance = 100f;

    [System.Serializable]
    public class ObjectLayer
    {
        public int high;
        public bool enabled = true;
        public GameObject objectPrefab;
        [Range(1f, 100f)] public int rarity;
    }
}