using UnityEngine;

public class NoiseFilter
{
    private NoiseSettings _settings;
    private Noise _noise = new Noise();

    public NoiseFilter(NoiseSettings settings)
    {
        this._settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.baseRoughness;
        float amplitude = 1;
        for (int i = 0; i < _settings.numLayers; i++)
        {
            float v = _noise.Evaluate(point * frequency + _settings.centre);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= _settings.roughness;
            amplitude *= _settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.minValue);
        return noiseValue * _settings.strength;
    }
}