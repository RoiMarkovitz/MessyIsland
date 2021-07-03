using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTarget : MonoBehaviour
{
    float x, y, z;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {       
        if (other.gameObject.tag == "Swat" || other.gameObject.tag == "Ninja")
        {
            Humanoid script = other.gameObject.GetComponent<Humanoid>();
            if (script.getIsNPC())
            {
                NPC npcScript = other.gameObject.GetComponent<NPC>();
                Vector3 targetPosition; 
               
                do
                {
                    targetPosition = setRandomPosition();                                   
                    this.transform.position = targetPosition;
                } while (!npcScript.isTargetReachable(targetPosition));            
            }
                      
        }
    }

    Vector3 setRandomPosition()
    {
        x = Random.Range(1, 119);
        z = Random.Range(1, 349);
        y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));
        return new Vector3(x, y, z);       
    }
}
