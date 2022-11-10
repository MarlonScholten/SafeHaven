using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float visionRange = 10f;
    public float visionAngle = 45f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkVision();
    }

    void checkVision()
    {
        Collider[] foundObjects = Physics.OverlapSphere(transform.position, visionRange);
        Collider player = getPlayer(foundObjects);
        if (player != null)
        {
            //check if player is in vision angle
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < visionAngle)
            {
                //check if player is in line of sight
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRange))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        Debug.Log("Player in sight");
                    }
                }
            }
        }
    }
    
    private Collider getPlayer(Collider[] objects)
    {
        foreach (Collider obj in objects)
        {
            if (obj.gameObject.tag == "Player")
            {
                return obj;
            }
        }
        return null;
    }
}
