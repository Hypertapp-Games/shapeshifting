using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombiningMeshes : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(Combining());
    }

    public IEnumerator Combining()
    {
        yield return new WaitForSeconds(0.1f);
        
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 1;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            //meshFilters[i].gameObject.SetActive(false);
            var meshRenderer = meshFilters[i].gameObject.GetComponent<MeshRenderer>();
            Destroy(meshRenderer);
            Destroy(meshFilters[i]);
            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);
    }
    
   
}
