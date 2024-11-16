using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Planet : MonoBehaviour
{
   [Range(2, 256)] public  int resolution = 10;

   public bool autoUpdate = true;

   [HideInInspector] public bool shapeSettingsFoldout;
   [HideInInspector] public bool colourSettingsFoldout;
   [HideInInspector] public bool objGenSettingsFoldout;

   public ObjGenSettings objectGeneratorSettings;
   public ShapeSettings shapeSettings;
   public ColourSettings colourSettings;
   private PlanetFiller _planetFiller;
   private ShapeGenerator _shapeGenerator;
   private ColourGenerator _colourGenerator = new ColourGenerator();
   private ObjectGenerator _objectGenerator;

   
   [SerializeField, HideInInspector] MeshFilter[] _meshFilters;
   private TerrainFace[] _terrainFaces;



   void Initialize()
   
   {
      _shapeGenerator = GetComponent<ShapeGenerator>();
      _colourGenerator = new ColourGenerator();
      _shapeGenerator.UpdateSettings(shapeSettings);
      _colourGenerator.UpdateSettings(colourSettings);
      _objectGenerator = GetComponent<ObjectGenerator>();
      _objectGenerator.UpdateSettings(objectGeneratorSettings, shapeSettings);
      // _planetFiller = new PlanetFiller(resolution);
      
      if (_meshFilters == null || _meshFilters.Length == 0)
         _meshFilters = new MeshFilter[6];
      _terrainFaces = new TerrainFace[6];

      Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back, };

      for (int i = 0; i < 6; i++)
      {
         if (_meshFilters[i] == null)
         {
            GameObject meshObj = new GameObject("mesh");
            meshObj.transform.parent = transform;

            meshObj.AddComponent<MeshRenderer>();
            _meshFilters[i] = meshObj.AddComponent<MeshFilter>();
            _meshFilters[i].sharedMesh = new Mesh();
         }

         _meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;
         _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, resolution, directions[i]);
      }
      
   }


   public void GeneratePlanet()
   {
      Initialize();
      GenerateMesh();
      GenerateColours();
   }

   public void OnShapeSettingsUpdated()
   {
      if (autoUpdate)
      {
         Initialize();
         GenerateMesh();
      }
   }

   public void OnColourSettingUpdated()
   {
      if (autoUpdate)
      {
         Initialize();
         GenerateColours();
      }
   }

   void GenerateMesh()
   {
      ObjectGenerator.ClearSpotList();
      foreach (TerrainFace face in _terrainFaces)
      {
         face.ConstructMesh();
      }
      
      _colourGenerator.UpdateElevation(_shapeGenerator.elevationMinMax);
   }

   void GenerateColours()
   {
      _colourGenerator.UpdateColours();
   }

   public void GenerateObjects()
   {
      Debug.Log("wywołano generowanie obiektow");
      _objectGenerator.GenerateObjects();
   }
}
