using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// creating a sphere using a cube and inflating it, covering it up with meshes for control over finer details
public class TerrainFace : MonoBehaviour
{
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;


    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp; //directional vector going up from top face of the cube

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA); // Vector perpendicular to localup and axisA - cross product
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        /*
            Resolution based on number of dots: 4
            r = 4
            Number of small squares: 9
            (r – 1)^2
            Number of triangles per square: 2
            (r – 1)^2 * 2
            Number of vertices of a triangle: 3
            (r – 1)^2 * 2 * 3
        */
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 2 * 3];

        int i = 0;
        int triIndex = 0;
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                Vector2 percent = new Vector2(x, y) / (resolution - 1); // telling us how close to complete each loop is (normalized coordinates)
                // we can use this to define where the vertex will be on the face
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                // The unit cube has sides of length 1, centered at the origin.
                // localUp  - face we're operating on

                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                /*  How normalization works:
                    Normalization means converting a vector to a unit vector, which has a length (or magnitude) of 1, 
                    while maintaining the same direction.
                    By calling pointOnUnitCube.normalized, you are taking the pointOnUnitCube vector 
                    (which could be any point on the cube's surface) and normalizing it, 
                    which effectively projects it onto the surface of a unit sphere.
                    This works because the center of both the cube and the sphere is the origin (0, 0, 0). 
                    The further a point is from the origin, the more the normalization "pulls" that point onto the unit sphere.
                */

                vertices[i] = pointOnUnitSphere;

                if (x != resolution - 1 && y != resolution - 1) // do this for triangles of all edges except for the right most and bottom edges
                {
                    // first triangle
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    //  second triangle
                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6; // because we added 6 vertices in our array
                }
                i++;
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
