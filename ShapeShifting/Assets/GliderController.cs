using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderController : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterMove characterMove;
    public float speedFly;
    public float gravityFly;

    public float speedNotFly;
    public float gravityNotFly;

    public GameObject dir;
    public float maxDistance = 1;
    public GameObject hitt;
    private void Start()
    {
        characterMove = this.GetComponent<CharacterMove>();
        characterMove.speed = speedFly;
        characterMove.gravity = gravityFly;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(dir.transform.position, dir.transform.forward,out hit, maxDistance))
        {
            characterMove.gravity = gravityNotFly;
            characterMove.speed = speedNotFly;
            GetComponentInParent<PlayerManager>().isFlying = false;
        }
        else
        {
            characterMove.speed = speedFly;
            characterMove.gravity = gravityFly;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(dir.transform.position, dir.transform.position + dir.transform.forward * maxDistance);
    }

}
