using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;

    [HideInInspector]
    public bool colourSettingsFoldout;

    ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector] // serialized:  value is saved and restored (by default shown in Inspector, hence HideInInspector)
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    /*      removed because we are now generating the planet in the OnInspectorGUI function in the PlanetEditor
    private void OnValidate() // Called whenever changes are made in the Unity Inspector
    {
        GeneratePlanet();
    }
    */

    void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);

        if (meshFilters == null || meshFilters.Length == 0) // if the meshFilter array doesn't already exist/is empty, create it
        {
            meshFilters = new MeshFilter[6]; // 6 faces of the cube
        }

        terrainFaces = new TerrainFace[6]; // terrain for each face of cube

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null) // if no meshfilter
            {
                GameObject meshObj = new GameObject("mesh"); // create a new GameObject for that mesh
                
                meshObj.transform.parent = transform; // sets the parent of meshObj to be the GameObject that this script is attached to, i.e. Planet
                                                      // meshObj will inherit the position, rotation, and scale of its parent

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                /*
                Explanation:
                meshObj.AddComponent<MeshRenderer>(): Adds a MeshRenderer component to the meshObj. 
                The MeshRenderer is responsible for rendering the mesh by displaying the material on it. 

                .sharedMaterial = new Material(Shader.Find("Standard")):
                Assigns a material to the MeshRenderer.
                Shader.Find("Standard") looks for the built-in "Standard" shader.
                If multiple objects share the same material, sharedMaterial ensures that they all use the same instance of the material, 
                rather than creating a unique copy for each object (which would happen with material instead of sharedMaterial).
                */

                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                /*
                Add a MeshFilter component to the meshObj at each index
                MeshFilter: holds actual mesh geometry to be rendered by the MeshRenderer
                array meshFilters stores references to each face's MeshFilter
                */
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);

        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }
    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace terrainFace in terrainFaces)
        {
            terrainFace.ConstructMesh();
        }
    }

    void GenerateColours()
    {
        foreach (MeshFilter meshFilter in meshFilters)
        {
            meshFilter.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.planetColor;
        }
    }
}