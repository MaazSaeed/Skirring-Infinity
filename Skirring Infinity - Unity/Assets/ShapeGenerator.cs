using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;

    public ShapeGenerator(ShapeSettings settings)
    {
        this.settings = settings; 
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        // we have the ability to change the radius now through shapeSettings,
        // so every time we change it, the shapeGenerator will be triggered
        // to generate the shape once more
        return pointOnUnitSphere * settings.planetRadius;
    }
}
