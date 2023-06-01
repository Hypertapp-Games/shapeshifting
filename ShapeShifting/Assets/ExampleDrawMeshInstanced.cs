using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDrawMeshInstanced : MonoBehaviour
{
    public GameObject cub;
    Mesh mesh;
    Material material;
    Matrix4x4[] matrices;

    private void OnEnable()
    {
        mesh = cub.GetComponent<MeshFilter>().sharedMesh;
        material = cub.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void Update()
    {
 
        matrices = new Matrix4x4[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            // Vector3 position = gameObject.transform.GetChild(i).transform.position;
            // Quaternion rotation = gameObject.transform.GetChild(i).transform.rotation;
            // Vector3 scale = gameObject.transform.GetChild(i).transform.localScale;
            matrices[i] = gameObject.transform.GetChild(i).transform.localToWorldMatrix;
        }
        Graphics.DrawMeshInstanced(mesh, 0, material , matrices );
    }
}