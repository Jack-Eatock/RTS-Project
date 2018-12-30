using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Unit", menuName = " Units")]
public class UnitSO : ScriptableObject {

    public string Name;
    public int UnitType;
    public GameObject Unit_Prefab;
    public float Speed;


}
