using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    NoiseSettings.SimpleNoiseSettings settings;
    Noise noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0; // (noise.Evaluate(point * settings.roughness + settings.centre) + 1) * .5f; 
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.centre);
            noiseValue += (v + 1) * .5f * amplitude; // noise.Evaluate gives a value between -1 and 1, to bring it between 0 and 1, we add 1 and divide by 2
            frequency *= settings.roughness; // when roughness is greater than 1, the frequency will increase with each layer
            amplitude *= settings.persistence; // if persistence is less than 1, the amplitude will decrease with each layer
        }
        noiseValue = Mathf.Max(0, noiseValue - settings.minValue); // this will allow the noise to recede into the base of the planet
        return noiseValue * settings.strength;
    }
}
