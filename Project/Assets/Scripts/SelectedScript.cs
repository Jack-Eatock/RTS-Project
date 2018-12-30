using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedScript : MonoBehaviour {

    void Start()
    {
        OnDeSelected();

    }

    public  void OnSelected()
    {
        gameObject.SetActive(true);


    }
    public  void OnDeSelected()
    { 
        gameObject.SetActive(false);
    }


}
