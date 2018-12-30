using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "New Building", menuName =" Buildings") ]
public class BuildingSO : ScriptableObject {


    public string Name;
    public int BuildingType;
    public GameObject Building_Prefab;

}
