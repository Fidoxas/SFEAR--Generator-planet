using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColourGenerator
{
    ColourSettings _settings;
    private Texture2D texture;

    private const int textureResolution = 100;
    public void UpdateSettings(ColourSettings settings)
    {
        this._settings = settings;
        if (texture == null)
            texture = new Texture2D(textureResolution, 1);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        _settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min,elevationMinMax.Max));
    }

   public void UpdateColours()
   {
       Color[] colours = new Color[textureResolution];
       for (int i = 0; i < textureResolution; i++)
       {
           colours[i] = _settings.gradient.Evaluate(i / (textureResolution - 1f));
       }
       texture.SetPixels(colours);
       texture.Apply();
       _settings.planetMaterial.SetTexture("_texture", texture);
   }
    
    
    
}
