using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public GameObject player;
    private Vector3 offset = new Vector3();

    void Start()
    {
        offset = gameObject.transform.position - player.transform.position;
    }
    private void Update()
    {
        if(player != null)
        {
            transform.position = player.transform.position + offset;
        }
        float dist = Vector3.Distance(player.transform.localToWorldMatrix.GetPosition() , gameObject.transform.position );
        Vector3 forward = player.transform.localToWorldMatrix.GetPosition() - gameObject.transform.position ;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, forward, out hit, dist-2 ))
        {
            for (int i = 0; i < hit.transform.parent.childCount; i++)
            {
                MeshFilter[] meshFilters = hit.transform.parent.GetComponentsInChildren<MeshFilter>();
                MeshRenderer[] mers = hit.transform.parent.GetComponentsInChildren<MeshRenderer>();
                foreach (var meshFilter in meshFilters)
                {
                    Destroy(meshFilter);
                }
                foreach (var mer in mers)
                {
                    Destroy(mer);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        float dist = Vector3.Distance(player.transform.localToWorldMatrix.GetPosition() , gameObject.transform.position );
        Vector3 forward = player.transform.localToWorldMatrix.GetPosition() - gameObject.transform.position ;
        Gizmos.color = Color.red;
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + forward );
    }
}
// public IEnumerator currentVehicleScaletoZero(float t)
//     {
//         changVehicleEffect.gameObject.SetActive(false);
//         changVehicleEffect.gameObject.SetActive(true);
//         changVehicleEffect.transform.position = currentVehicle.transform.localToWorldMatrix.GetPosition();
//         changVehicleEffect.transform.parent = currentVehicle.transform;
//         
//         //StartCoroutine(ShowEffect(t * 2.0f));
//         var mesh = currentVehicle.GetComponent<VehicleData>().mesh;
//         StartCoroutine(t.Tweeng((p) => mesh.gameObject.transform.localScale = p , mesh.gameObject.transform.localScale,  new Vector3(0, 0, 0)));
//         yield return new WaitForSeconds(t);
//         choseVehicle.transform.position = currentVehicle.transform.localToWorldMatrix.GetPosition();
//         currentVehicle.gameObject.SetActive(false);
//         currentVehicle = choseVehicle;
//         
//         changVehicleEffect.transform.parent = currentVehicle.transform;
//         
//         currentVehicle.transform.eulerAngles = new Vector3(0,90,0);
//         mesh = currentVehicle.GetComponent<VehicleData>().mesh;
//         StartCoroutine(t.Tweeng((p) => mesh.gameObject.transform.localScale = p , mesh.gameObject.transform.localScale,  new Vector3(1, 1, 1)));
//         currentVehicle.gameObject.SetActive(true);
//         if (currentVehicle.GetComponent<CarController>() != null)
//         {
//             currentVehicle.GetComponent<CarController>().AddForce();
//         }
//             
//
//         if (currentVehicle.GetComponent<HelicopterController>()!= null)
//         {
//             currentVehicle.GetComponent<HelicopterController>().UpdateCurentHight();
//         }
//
//         if (isPlayer)
//         {
//             cam.player = currentVehicle;
//         }
//         //StartCoroutine(0.5f.Tweeng())
//     }
//
//     public IEnumerator ShowEffect(float t)
//     {
//         float time = 0;
//         while (time < t)
//         {
//             yield return new WaitForSeconds(Time.deltaTime);
//             time += Time.deltaTime;
//             //changVehicleEffect.transform.position = Vector3.Lerp(startPosition, currentVehicle.transform.localToWorldMatrix.GetPosition(), Mathf.SmoothStep(0,1,percent) );
//             changVehicleEffect.transform.position = currentVehicle.transform.localToWorldMatrix.GetPosition();
//         }
//         
//     }
