using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidgidNoiseFilter : INoiseFilter
{
    NoiseSettings.RidgidNoiseSettings settings;
    Noise noise = new Noise();

    public RidgidNoiseFilter(NoiseSettings.RidgidNoiseSettings settings)
    {
        this.settings = settings;
    }
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0; // (noise.Evaluate(point * settings.roughness + settings.centre) + 1) * .5f; 
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        /*
         * * * * * * * * * * * * Ridges * * * * * * * * * * * *
         * 
         * Ridges will be created by taking the absolute of the sin function and square it for sharpness
        we want the noise in the ridges to be a lot more detailed compared to the
        noise in the sort of valleys below the way we'll do this is by weighting the
        noise in each layer based on that came before it
        */


        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier); // regions below will remain less detailed, the higher you go, higher the detail
            
            noiseValue += v * amplitude; // noise.Evaluate gives a value between -1 and 1, to bring it between 0 and 1, we add 1 and divide by 2
            frequency *= settings.roughness; // when roughness is greater than 1, the frequency will increase with each layer
            amplitude *= settings.persistence; // if persistence is less than 1, the amplitude will decrease with each layer
        }
        noiseValue = Mathf.Max(0, noiseValue - settings.minValue); // this will allow the noise to recede into the base of the planet
        return noiseValue * settings.strength;
    }
}
