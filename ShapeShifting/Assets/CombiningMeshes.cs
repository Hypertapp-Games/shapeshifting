using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombiningMeshes : MonoBehaviour
{
	public void Combining()
    {

	    MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
        
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(); 
        
        List<Material> materials = new List<Material>();
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.transform == transform)
                continue;
            Material[] localMats = renderer.sharedMaterials;
            foreach (Material localMat in localMats)
                if (!materials.Contains (localMat))
                    materials.Add (localMat);
        }
        List<CombineInstance> finalMeshCombineInstancesList = new List<CombineInstance>();
        

		for(int i = 0; i < materials.Count; i++) 
		{
			List<CombineInstance> submeshCombineInstancesList = new List<CombineInstance>();

			for(int j = 0; j < filters.Length-1; j++) 
			{
				if (filters[j + 1].GetComponent<Obstacle>() == null)
				{
					if(renderers[j+1] != null)
					{
						Material[] submeshMaterials = renderers[j+1].sharedMaterials;

						for(int k = 0; k < submeshMaterials.Length; k++)
						{
							if(materials[i] == submeshMaterials[k])
							{
								CombineInstance combineInstance = new CombineInstance();
								combineInstance.subMeshIndex = k; 
								combineInstance.mesh = filters[j+1].sharedMesh;
								combineInstance.transform = filters[j+1].transform.localToWorldMatrix;
								submeshCombineInstancesList.Add(combineInstance);
							}
						}
					}
				}
				
			}
			Mesh submesh = new Mesh();
			submesh.CombineMeshes(submeshCombineInstancesList.ToArray(), true);
			CombineInstance finalCombineInstance = new CombineInstance();
			finalCombineInstance.subMeshIndex = 0;
			finalCombineInstance.mesh = submesh;
			finalCombineInstance.transform = Matrix4x4.identity;
			finalMeshCombineInstancesList.Add(finalCombineInstance);
		}
		renderers[0].sharedMaterials = materials.ToArray();

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(finalMeshCombineInstancesList.ToArray(), false);
        filters[0].sharedMesh = combinedMesh;
        DeactivateCombinedGameObjects(filters);
        
    }
    private void DeactivateCombinedGameObjects(MeshFilter[] meshFilters)
    {
	    
	    for(int i = 0; i < meshFilters.Length-1; i++) 
	    {
		    if (meshFilters[i + 1].GetComponent<Obstacle>() == null)
		    {
			    Destroy(meshFilters[i+1].GetComponent<MeshRenderer>());
			    Destroy(meshFilters[i+1]);	
		    }
	    }
    }
    
   
}

