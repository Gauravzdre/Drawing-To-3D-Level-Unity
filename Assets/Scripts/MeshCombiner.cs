using UnityEngine;
using System.Collections.Generic;

public class MeshCombiner : MonoBehaviour
{
    public GameObject parentObject;

    void Test() 
    {
        CombineMeshes();
    }

    void CombineMeshes()
    {
        // 1. Gather components
        MeshFilter[] meshFilters = parentObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        const int MAX_VERTICES_PER_MESH = 65000;
        int currentVertexCount = 0;
        int meshIndex = 0;

        // 2. Prepare for combining meshes
        for (int i = 0; i < meshFilters.Length; i++)
        {
            // Check if we need to create a new mesh
            if (currentVertexCount + meshFilters[i].sharedMesh.vertexCount > MAX_VERTICES_PER_MESH)
            {
                //CreateCombinedMesh(meshIndex);  // Create and process the current combined mesh 
                GameObject combinedObject = new GameObject("Combined Mesh_" + meshIndex);
                combinedObject.transform.parent = parentObject.transform;

                // 2. Create and configure the mesh
                Mesh combinedMesh = new Mesh();
                combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                combinedMesh.CombineMeshes(combine);

                // 3. Add components and optimize
                combinedObject.AddComponent<MeshFilter>().mesh = combinedMesh;
                combinedObject.AddComponent<MeshRenderer>().material = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;

                currentVertexCount = 0; // Reset count for a new mesh
                meshIndex++;
            }

            // Add mesh to CombineInstance array
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            currentVertexCount += meshFilters[i].sharedMesh.vertexCount;
        }

        // Process the final combined mesh
        //CreateCombinedMesh(meshIndex);
        GameObject combinedObjectF = new GameObject("Combined Mesh_" + meshIndex);
        combinedObjectF.transform.parent = parentObject.transform;

        // 2. Create and configure the mesh
        Mesh combinedMeshF = new Mesh();
        combinedMeshF.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMeshF.CombineMeshes(combine);

        // 3. Add components and optimize
        combinedObjectF.AddComponent<MeshFilter>().mesh = combinedMeshF;
        combinedObjectF.AddComponent<MeshRenderer>().material = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
    }
}
