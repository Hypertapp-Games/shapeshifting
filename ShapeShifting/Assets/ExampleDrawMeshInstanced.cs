using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDrawMeshInstanced : MonoBehaviour
{
    List<Mesh> mesh = new List<Mesh>();
    List<Material> material = new List<Material>();
    List<GameObject> draw = new List<GameObject>();
    Matrix4x4[] matrices;
    void OnEnable()
    {
         for (int i = 0; i < transform.childCount; i++)
         {
             if (gameObject.transform.GetChild(i).GetComponent<MeshFilter>() != null)
             {
                 mesh.Add(gameObject.transform.GetChild(i).GetComponent<MeshFilter>().mesh);
                 Destroy(gameObject.transform.GetChild(i).GetComponent<MeshFilter>());
                 material.Add(gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material);
                 Destroy(gameObject.transform.GetChild(i).GetComponent<MeshRenderer>());
                 draw.Add(gameObject.transform.GetChild(i).gameObject);
             }
         }
    }
    void Update()
    {
        for (int i = 0; i < draw.Count; i++)
       {
           matrices = new Matrix4x4[1];
           for (int j = 0; j < 1; j++)
           {
               Vector3 position = draw[i].transform.position;
               Quaternion rotation = draw[i].transform.rotation;
               Vector3 scale = draw[i].transform.localScale;
               matrices[j] = Matrix4x4.TRS(position, rotation, scale);
           }
           var meshs = mesh[i];
           var materials = material[i];
           Graphics.DrawMeshInstanced(meshs, 0, materials , matrices );
       }
    }
}
