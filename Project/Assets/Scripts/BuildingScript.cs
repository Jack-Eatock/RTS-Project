using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    // Converts the 2d location the users mouse into a 3d position.
    public static RaycastHit Location() {
        Camera cam = Camera.main;
        //Tells the camera the position clicked, stored as "ray".
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        // Stores info of what the ray hits.
        RaycastHit hit;
        if (GameObject.Find("Ground").GetComponent<BuildingScript>().BuildingType == 0) {
            Physics.Raycast(ray, out hit);
        }
        else{
            float range = 1000;
            Physics.Raycast(ray, out hit, range, 1 << LayerMask.NameToLayer("Ground")); // Ignores all layers except Ground.
        }
        return hit;
    }

    // Variables \\
    public int BuildingType = 0;
    private bool Grid_Hidden = true;
    public static GameObject Global_Build;

    // Building Variables \\
    public static bool Placing_Flag = false; // Checks when Placement Function has looped once. REMOVE
    public static int Num = 0;  // Sets value after object is created, stopping repetition.
    public static Vector3 Placment_Var = new Vector3(0, 0, 0);
    public static GameObject Build;
    public static List<Color> Orig_Build_Col = new List<Color>();
    public static bool Build_Red_Flag = false; // REMOVE
    public static Renderer[] child; 

    // Materials \\
    public Material Grids;
    public Material Grass;

    // Buildings \\
    public BuildingSO Castle;
    public BuildingSO House;
    public BuildingSO StockPile;

    // --------------------------------- Functions ---------------------------------\\ 

    // Rotates Building when R is pressed - Called by Main script.
    public void RotateBuilding()
    {
        Vector3 Rotation_ = Build.transform.rotation.eulerAngles;
        Build.transform.eulerAngles = new Vector3(Rotation_.x, Rotation_.y + 90, Rotation_.z);
    }

    // Sets Buildings textures to Red.
    public static void Building_Red(bool Set_Red)
    {
        if (Set_Red == true){
            child = Build.GetComponentsInChildren<Renderer>();
            Orig_Build_Col.Clear();
            foreach (Renderer Rend in child){
                foreach (Material Rend_M in Rend.materials){
                    Material material_ = Rend_M;
                    Orig_Build_Col.Add(Rend_M.color);
                    material_.color = Color.red;
                }
            }
        }
        else
        {
            int Counter = 0;
            foreach (Renderer Rend in child)
            {
                foreach (Material Rend_M in Rend.materials)
                {
                    Color Original_Col = Orig_Build_Col[Counter];
                    Material material_ = Rend_M;
                    material_.color = Original_Col;
                    Counter += 1;
                }
            }
        }
    }

    // Creates and moves the building to available positions.
    public static void Placement(BuildingSO Building)
    {
        if (Placing_Flag == false)
        { // If first loop.
            Num += 1; // Increments by 1 so we dont get repetive building names.
            Build = GameObject.Instantiate(Building.Building_Prefab, new Vector3(0, Building.Building_Prefab.gameObject.transform.position.y, 0), Building.Building_Prefab.transform.rotation); // Creates specified building.
            Build.name = (Building.name + "(" + Num + ")"); // Adds Num  to the end of the name to make it unique.
            Placing_Flag = true;
        }
        Global_Build = Build;
        RaycastHit hit = BuildingScript.Location(); // Returns the 3d location of where the users mouse is.
        if (hit.transform != null)
        {
            if (hit.transform.gameObject.tag == "Buildable")
            { // If the user hovers over ground that is tagged Buildable.
                Placment_Var.x = Mathf.Floor(hit.point.x);  //   Rounds Down
                Placment_Var.z = Mathf.Floor(hit.point.z); //   To match Grid
                Placment_Var.y = (hit.point.y); // Sets Y axis of building to its height  + the Y axis of position clicked.
                Build.transform.position = Placment_Var;
            }
        }
    }

    // Sets Grid On and off.
    public static void Set_Grid(bool Grid_On)
    {
        // Checks every child of Ground.
        foreach (Transform child in GameObject.Find("Ground").transform)
        {
            // Checks if the child is "Buildable" 
            if (child.tag == "Buildable")
            {
                // If setting Grid.
                if (Grid_On == true)
                {
                    // Sets Ground Material to Grids.
                    child.GetComponent<Renderer>().material = GameObject.Find("Ground").GetComponent<BuildingScript>().Grids;
                    // Sets the grids to tile the size of the cube.
                    Vector3 Current_Size = child.transform.localScale;
                    child.GetComponent<Renderer>().material.mainTextureScale = new Vector2(Current_Size.x, Current_Size.z);
                }
                // If setting Grass.
                else
                {
                    // Sets Ground Material to Grass.
                    child.GetComponent<Renderer>().material = GameObject.Find("Ground").GetComponent<BuildingScript>().Grass; ;
                }
            }
        }
    }

    // --------------------------------- Main Code ----------------------------------- \\

    // On start
    public void Start()
    {
        child = GameObject.Find("Ground").GetComponents<Renderer>();
    }
    public void Update()
    {
        // If the user is not currently Building.
        if (BuildingType == 0){
            // If not building but grid still showing.
            if (Grid_Hidden == false) // If first time through.
            {  
                // Deactive the Grid , making it invisible
                Set_Grid(false); // Hides Grid.
                MainScript.IsBuilding = false; // Tells the Mainscript that the user is not building.
                Placing_Flag = false;
                Grid_Hidden = true;
            }
        }
        else
        {
            // If the user is currently Building.
            if (Grid_Hidden == true) // If first time through.
            {
                // Actrivates Grids for building.
                Set_Grid(true); // Creates Grid.
                MainScript.IsBuilding = true; // Tells the Mainscript that the user is building.
                Grid_Hidden = false;    
            }

            // Stores data for each Building, used to spawn them in. \\
            switch (BuildingType)
            {
                case  1:        // Spawn a Castle.
                    Placement(Castle);
                    break;

                case 2:          // Spawn a House.
                    Placement(House);
                    break;

                case 3:          // Spawn a Barracks
                    Placement(House);
                    break;

                case 4:          // Spawn a Archery
                    Placement(House);
                    break;

                case 5:          // Spawn a Stables
                    Placement(House);
                    break;

                case 6:          // Spawn a Farm
                    Placement(House);
                    break;

                case 7:          // Spawn a StockPile
                    Placement(House);
                    break;

            }
            
        }


    }



}

