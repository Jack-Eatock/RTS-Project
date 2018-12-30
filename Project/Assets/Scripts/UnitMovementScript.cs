using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovementScript : MonoBehaviour {

    public int Unit_Type;
    public Animator Anim;
    public NavMeshAgent Agent;
    public UnitSO Unit;
    public Vector3 Destination;
    public Vector3  Dest_Displacement = new Vector3 (0,0,0);
    public Quaternion Rotation;
    public bool Rotation_Directed = false;

    private void Start()
    {
        Destination = transform.position;
        Rotation = transform.rotation;
    }



    void Update () {
        
        if (Vector3.Distance(Destination, transform.position) > 0.1){
            Agent.isStopped = false;
            Agent.SetDestination(Destination + Dest_Displacement);
            

        }
        else
        {
            if (Rotation_Directed == true)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotation, 5);
            }
            
            Agent.isStopped = true;

        }

        
       
    }
    void OnAnimatorMove()
    {
        
        Anim.SetFloat("Walking", Agent.velocity.magnitude / Unit.Speed);
    }
}
