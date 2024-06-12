using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using OpenCvSharp;
using UnityEngine.UIElements;

public enum PrimitiveShape { Cube, Cylinder, Plane }

public class WallCreator : MonoBehaviour
{
    public float defaultWallHeight = 4f;
    public float wallThickness = 0.2f;
    public float floorHeight = 0.1f; // Thickness of the floor

    public Transform parent;

    public void CreateWalls(List<Vector2[]> wallContours, float scaleX, float scaleY)
    {
        foreach (var contour in wallContours)
        {
            for (int i = 0; i < contour.Length - 1; i++)
            {
                Vector3 start = new Vector3(contour[i].x * scaleX, defaultWallHeight / 2, contour[i].y * scaleY);
                Vector3 end = new Vector3(contour[i + 1].x * scaleX, defaultWallHeight / 2, contour[i + 1].y * scaleY);

                CreateWallSegment(start, end, PrimitiveShape.Cube); // Default is the Cube
            }
        }

        CreateFloor(wallContours, scaleX, scaleY); // Create the floor after the walls
    }

    //void CreateWallSegment(Vector3 start, Vector3 end)
    //{
    //    // Calculate wall direction and length
    //    Vector3 direction = end - start;
    //    float length = direction.magnitude;

    //    // Calculate a perpendicular vector for the wall's width
    //    Vector3 cross = Vector3.Cross(direction, Vector3.up).normalized;

    //    // Create a new mesh
    //    Mesh mesh = new Mesh();

    //    // Define vertices for a quad (two triangles forming a rectangle)
    //    Vector3[] vertices = new Vector3[]
    //    {
    //        start + Vector3.up * defaultWallHeight + cross * wallThickness / 2, // Top left
    //        start + cross * wallThickness / 2,                                 // Bottom left
    //        end + cross * wallThickness / 2,                                   // Bottom right
    //        end + Vector3.up * defaultWallHeight + cross * wallThickness / 2   // Top right
    //    };

    //    // Define triangle indices
    //    int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };

    //    // Define UVs (optional, for texture mapping)
    //    Vector2[] uv = new Vector2[] {
    //        new Vector2(0, 1),
    //        new Vector2(0, 0),
    //        new Vector2(length / wallThickness, 0),
    //        new Vector2(length / wallThickness, 1)
    //    };

    //    // Set mesh data
    //    mesh.vertices = vertices;
    //    mesh.triangles = triangles;
    //    mesh.uv = uv; // Optional
    //    mesh.RecalculateNormals(); // Ensure proper lighting

    //    // Create a GameObject to hold the mesh
    //    GameObject wallSegment = new GameObject("WallSegment");
    //    wallSegment.transform.position = (start + end) / 2;

    //    // Add MeshFilter and MeshRenderer components
    //    MeshFilter meshFilter = wallSegment.AddComponent<MeshFilter>();
    //    meshFilter.mesh = mesh;

    //    MeshRenderer meshRenderer = wallSegment.AddComponent<MeshRenderer>();
    //    meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit")); // Assign the wall material
    //    meshRenderer.material.color = Color.gray;
    //    // Rotate the segment to face the correct direction
    //    wallSegment.transform.LookAt(end);

    //    wallSegment.transform.SetParent(parent);
    //}

    void CreateWallSegment(Vector3 start, Vector3 end, PrimitiveShape shape)
    {
        GameObject segment = null;

        switch (shape)
        {
            case PrimitiveShape.Cube:
                segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            case PrimitiveShape.Cylinder:
                segment = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                segment.transform.localScale = new Vector3(wallThickness, defaultWallHeight / 2, wallThickness); // Adjust scaling
                break;
            case PrimitiveShape.Plane:
                segment = GameObject.CreatePrimitive(PrimitiveType.Plane);
                segment.transform.localScale = new Vector3(wallThickness, defaultWallHeight, 1f); // Adjust scaling
                segment.transform.Rotate(90, 0, 0);  // Rotate for a vertical plane
                break;
        }

        if (segment != null)
        {
            segment.transform.position = (start + end) / 2;
            Vector3 direction = end - start;
            float distance = direction.magnitude;

            // Set the length based on shape
            if (shape == PrimitiveShape.Plane)
            {
                segment.transform.localScale = new Vector3(wallThickness, segment.transform.localScale.y + defaultWallHeight, distance);
            }
            else
            {
                segment.transform.localScale = new Vector3(wallThickness, segment.transform.localScale.y + defaultWallHeight, distance);
            }
            segment.transform.LookAt(end);
            var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = Color.black;
            segment.GetComponent<MeshRenderer>().material = mat;
            segment.transform.SetParent(parent);
        }
    }

    void CreateFloor(List<Vector2[]> contours, float scaleX, float scaleY)
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";

        double largestArea = 0;
        Vector2[] floorContour = null;
        foreach (var contour in contours)
        {
            double area = Cv2.ContourArea(contour.Select(p => new Point((int)p.x, (int)p.y)).ToArray(), false);
            if (area > largestArea)
            {
                largestArea = area;
                floorContour = contour.Select(p => new Vector2((int)p.x, (int)p.y)).ToArray();
            }
        }

        if (floorContour != null)
        {
            Point2f[] floorPoints2f = floorContour.Select(p => new Point2f(p.x, p.y)).ToArray();

            RotatedRect boundingRect = Cv2.MinAreaRect(floorPoints2f);
            Size2f rectSize = boundingRect.Size;
            Point2f center = new Point2f(boundingRect.Center.X, boundingRect.Center.Y);

            floor.transform.position = new Vector3(center.X * scaleX, 0.75f, center.Y * scaleY);
            floor.transform.localScale = new Vector3((rectSize.Width / 10) * scaleX, 1, (rectSize.Height / 10) * scaleY);
            floor.transform.eulerAngles = new Vector3(0, -boundingRect.Angle, 0);

            GameObject roof = Instantiate(floor);
            roof.name = "Roof";
            roof.transform.eulerAngles = new Vector3(180, 0, 0);
            roof.transform.position = new Vector3(center.X * scaleX, 5.5f, center.Y * scaleY);
        }
        else
        {
            Debug.LogWarning("Floor contour not found!");
        }
    }
}