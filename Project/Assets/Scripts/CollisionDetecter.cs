using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CollisionDetecter : MonoBehaviour
{

    bool Red_Switch = false;
    private int CollisionCounter = 0;
    void OnCollisionEnter(Collision collisionInfo)
    {

        if (collisionInfo.gameObject.layer != 9) // NOT Layer 9 (Ground)
        {
            CollisionCounter += 1;
            if (Red_Switch == false)
            {

                GameObject.Find("MainScript").GetComponent<MainScript>().Area_Clear = false;
                GameObject.Find("Ground").GetComponent<BuildingScript>().Building_Red_Pass(GameObject.Find("Ground").GetComponent<BuildingScript>().Global_Build, true);              
                Red_Switch = true;
               
            }
            
      
        }

    }


    void OnCollisionExit(Collision collisionInfo)
    {

        if (collisionInfo.gameObject.layer != 9) // NOT Layer 9 (Ground)
        {
            CollisionCounter -= 1;
            if (Red_Switch == true  & CollisionCounter == 0)
            {
                
                GameObject.Find("Ground").GetComponent<BuildingScript>().Building_Red_Pass(GameObject.Find("Ground").GetComponent<BuildingScript>().Global_Build, false);
                Red_Switch = false;
                GameObject.Find("MainScript").GetComponent<MainScript>().Area_Clear = true;
            }
           
        }
    }

    private void Update()
    {
        //Debug.Log(CollisionCounter);
        if (GameObject.Find("MainScript").GetComponent<MainScript>().IsBuilding == false)
        {

            
            Destroy(this);
        }


    }
}