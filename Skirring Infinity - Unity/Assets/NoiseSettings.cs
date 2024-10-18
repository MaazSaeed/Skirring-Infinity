using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Rigid };
    public FilterType filterType;

    [ConditionalHide("filterType", 0)] // 0 being Simple
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)] // 1 being Ridgid
    public RidgidNoiseSettings ridgidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strength = 1;
        [Range(1, 8)]
        public int numLayers = 1; // layers for different noise levels to then be merged
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = .5f; // amplitude will be halved with every layer
        public Vector3 centre;
        public float minValue;
    }

    [System.Serializable]
    public class RidgidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = .8f;
    }
    

    
}
