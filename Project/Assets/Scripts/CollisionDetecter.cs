using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CollisionDetecter : MonoBehaviour
{
    
    bool Red_Switch = false; // Prevents the Building been set / removed red more than once.
    private int CollisionCounter = 0; // Keeps track on wheter the building is in a collison, If Not 0 it is colliding.

    // For every new collision, Collision Counter is incremented 
    void OnCollisionEnter(Collision collisionInfo){
        if (collisionInfo.gameObject.layer != 9) // NOT Layer 9 (Ground)
        {
            CollisionCounter += 1;
            if (Red_Switch == false) {
                MainScript.Area_Clear = false;
                BuildingScript.Building_Red(true);
                Red_Switch = true;
               
            }
        }
    }

    // For every collision exited , Collision Counter is decremented 
    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.layer != 9) // NOT Layer 9 (Ground)
        {
            CollisionCounter -= 1;
            if (Red_Switch == true  & CollisionCounter == 0)
            {
                BuildingScript.Building_Red(false);
                Red_Switch = false;
                MainScript.Area_Clear = true;
            } 
        }
    }

    private void Update()
    {
        //
        if (MainScript.IsBuilding == false)
        {
            Destroy(this);
        }


    }
}