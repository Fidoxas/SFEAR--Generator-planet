using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    private ShapeSettings _settings;
    private NoiseFilter[] _noiseFilters;

    public MinMax elevationMinMax;
    private static HashSet<Vector3> _generatedPoints;

    public static List<Vector3> mountinePointsDown = new List<Vector3>();
    public static List<Vector3> mountinePointsUP = new List<Vector3>();
    
    public void UpdateSettings(ShapeSettings settings)
    {
        this._settings = settings;
        _noiseFilters = new NoiseFilter[_settings._noiseLayers.Length];
        for (int i = 0; i < _noiseFilters.Length; i++)
        {
            _noiseFilters[i] = new NoiseFilter(_settings._noiseLayers[i].noiseSettings);
        }

        elevationMinMax = new MinMax();
        _generatedPoints = new HashSet<Vector3>();
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (_noiseFilters.Length > 0)
        {
            firstLayerValue = _noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (_settings._noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 0; i < _noiseFilters.Length; i++)
        {
            if (_settings._noiseLayers[i].enabled)
            {
                float mask = (_settings._noiseLayers[i].useFirstLayerMask) ? firstLayerValue : 1;
                elevation += _noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        elevation = _settings.planetRadius * (1 + elevation);
        elevationMinMax.AddValue(elevation);

         Vector3 generatedPoint = pointOnUnitSphere * _settings.planetRadius * (1 + elevation);
         _generatedPoints.Add(pointOnUnitSphere);
        
        //if (elevation > 0)
        //{
        //    mountinePointsUP.Add(generatedPoint);
        //    mountinePointsDown.Add(pointOnUnitSphere * _settings.planetRadius);
        //}
        
         if (elevation == _settings.planetRadius)
         {
             ObjectGenerator.spotList.Add(generatedPoint);
         }
        
        return pointOnUnitSphere * elevation;
    }


    public float GetMinValue()
    {
        return _settings._noiseLayers[0].noiseSettings.minValue;
    }
    

    // Nowa funkcja do czyszczenia listy wygenerowanych punktów
    static public void ClearGeneratedPoints()
    {
        _generatedPoints.Clear();
    }

    public static List<Vector3> GetmountinePointsDown()
    {
        return mountinePointsDown;
    }

    public static List<Vector3> GetmountinePointsUP()
    {
        return mountinePointsUP;
    }
}
