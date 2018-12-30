using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {

    // For Main.cs
    public Vector2 ScreenPos;
    public bool OnScreen;
    public bool Selected = false;
    public GameObject SelectionBox;
    private bool SelectionTrigger = false;

    void Update()
    {
        // If the Unit is not selected, get screen space.
        if (Selected == false)
        {
            if (SelectionTrigger == false)
            {
                SelectionTrigger = true;
                SelectionBox.GetComponent<SelectedScript>().OnDeSelected();
            }
            // Track Screen position
            ScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);

            //if within the screen space.
            if (MainScript.UnitWthinScreenSpace(ScreenPos))
            {
                // and not already added to Units on screen add it
                if (!OnScreen)
                {
                    MainScript.UnitsOnScreen.Add(this.gameObject);
                    OnScreen = true;
                }

            }

            //unit is not in screen space...
            else
            {
                //remove if previously on the screen
                if(OnScreen)
                {
                    MainScript.RemoveFromOnScreenUnits(this.gameObject);
                }

            }

        }
        else
        {
           
            if (SelectionTrigger == true)
            {
                SelectionTrigger = false;
                SelectionBox.GetComponent<SelectedScript>().OnSelected();
            }
        }
    }
}
