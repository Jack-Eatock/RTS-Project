using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasScript : MonoBehaviour {
    public GameObject Castle_UI;
    public GameObject Panel;
    public GameObject Basic_Building;
    public TextMeshProUGUI Selected_UI;


    public int UiNeeded_Last = 200;
    public  void ActivateUi( int UiNeeded)
    {
        Debug.Log("Ui Needed:" + UiNeeded+ "Last :"+ UiNeeded_Last);
        if (UiNeeded_Last != 200)
        { 
            if (UiNeeded_Last == 1)
            {
                Castle_UI.SetActive(false);
            }
            else if (UiNeeded_Last == 2)
            {
                Basic_Building.SetActive(false);
            }
            else if (UiNeeded_Last == 3)
            {

            }
            else if (UiNeeded_Last == 4)
            {

            }
        }

        UiNeeded_Last = UiNeeded;
        if (UiNeeded == 0)
        {
            Panel.SetActive(false);
            //Castle_UI.SetActive(false);
        }
        else if (UiNeeded == 1)
        {
            Panel.SetActive(true);
            Castle_UI.SetActive(true);
            Selected_UI.text = ("Selected : Castle");
        }
        else if (UiNeeded == 2)
        {
            Panel.SetActive(true);
            Basic_Building.SetActive(true);
            Selected_UI.text = ("Selected : House");
        }
        else if (UiNeeded == 3)
        {

        }
        else if (UiNeeded == 4)
        {

        }
    }
}
