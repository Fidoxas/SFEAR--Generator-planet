using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu]

    public class ShapeSettings : ScriptableObject
    {
        public float planetRadius = 1;
        public NoiseLayer[] _noiseLayers;

        [System.Serializable]
        public class NoiseLayer
        {
            public bool enabled = true;
            public bool useFirstLayerMask;
            public NoiseSettings noiseSettings;
        }
    }
