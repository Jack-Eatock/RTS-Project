using System.Collections;
using System.Collections.Generic;
using UnityEngine;





// All Functions used when building.
public class BuildingFunctions
{
    // Func Variables \\
    public static bool Placing_Flag = false; // Checks when Placement Function has looped once.
    public static int  Num = 0;  // Sets value after object is created, stopping repetition.
    public static Vector3 Placment_Var = new Vector3(0,0,0);
    public static GameObject Build;
    public static List<Color> Orig_Build_Col = new List<Color>();
    public static bool Build_Red_Flag = false;
    public static Renderer[] child = GameObject.Find("Ground").GetComponents<Renderer>();
    
    
    public static void RotateBuilding()
    {
        Vector3 Rotation_ = Build.transform.rotation.eulerAngles;
        Build.transform.eulerAngles = new Vector3 (Rotation_.x, Rotation_.y + 90, Rotation_.z);

        
    }


    public static void Building_Red( GameObject Build, bool Set_Red)
    {
        

        if (Set_Red == true)
        {
            
            child = Build.GetComponentsInChildren<Renderer>();
            Orig_Build_Col.Clear();
            foreach (Renderer Rend in child)
            {
                foreach(Material Rend_M in Rend.materials)
                {
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





    public static void Placement(BuildingSO Building)
    {
        
        if (Placing_Flag == false) // If first loop.
        {
            Num += 1; // Increments by 1 so we dont get repetive building names.
            Build = GameObject.Instantiate(Building.Building_Prefab, new Vector3(0, Building.Building_Prefab.gameObject.transform.position.y, 0), Building.Building_Prefab.transform.rotation); // Creates specified building.
            Build.name = (Building.name + "(" + Num + ")"); // Adds Num  to the end of the name to make it unique.
            Placing_Flag = true;

        }
        GameObject.Find("Ground").GetComponent<BuildingScript>().Global_Build = Build;
        RaycastHit hit = GameObject.Find("Ground").GetComponent<BuildingScript>().Location(); // Returns the 3d location of where the users mouse is.
        if (hit.transform != null)
        {
            if (hit.transform.gameObject.tag == "Buildable" ) // If the user hovers over ground that is tagged Buildable.
            {
                Placment_Var.x = Mathf.Floor(hit.point.x);  //   Rounds Down
                Placment_Var.z = Mathf.Floor(hit.point.z); //   To match Grid
                Placment_Var.y = (hit.point.y); // Sets Y axis of building to its height  + the Y axis of position clicked.
                Build.transform.position = Placment_Var;
                
            }
        }
        
       


    }
    

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
    

    

}







public class BuildingScript : MonoBehaviour
{

    // Converts the 2d location the users mouse into a 3d position.
    public RaycastHit Location()
    {
        Camera cam = Camera.main;
        //Tells the camera the position clicked, stored as "ray".
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        // Stores info of what the ray hits.
        RaycastHit hit;
        if (GameObject.Find("Ground").GetComponent<BuildingScript>().BuildingType == 0)
        {
            Physics.Raycast(ray, out hit);
        }
        else
        {


            float range = 1000;
            Physics.Raycast(ray, out hit, range, 1 << LayerMask.NameToLayer("Ground")); // Ignores all layers except Ground.


        }

        return hit;

    }



    public void Building_Red_Pass(GameObject Build, bool x)
    {
        //Debug.Log(Build.name + x);
        BuildingFunctions.Building_Red(Build, x);
        
    }
    public void RotateBuildingPass()
    {
        BuildingFunctions.RotateBuilding();
    }

    // Variables \\
    public int BuildingType = 0;
    private bool Grid_Hidden = true;
    public GameObject Global_Build;


    // Materials \\
    public Material Grids;
    public Material Grass;

    // Buildings \\
    public BuildingSO Castle;
    public BuildingSO House;
    public BuildingSO StockPile;




    public void Update()
    {
        if (BuildingType == 0)
        {

            if (Grid_Hidden == false) // If first time through.
            {  
                BuildingFunctions.Set_Grid(false); // Hides Grid.
                GameObject.Find("MainScript").GetComponent<MainScript>().IsBuilding = false; // Tells the Mainscript that the user is not building.
                BuildingFunctions.Placing_Flag = false;
                Grid_Hidden = true;
            }

        }
        else
        {
            if (Grid_Hidden == true) // If first time through.
            {
                BuildingFunctions.Set_Grid(true); // Creates Grid.
                GameObject.Find("MainScript").GetComponent<MainScript>().IsBuilding = true; // Tells the Mainscript that the user is building.
                Grid_Hidden = false;    

            }

            //         Types of Buidings          \\

            if (BuildingType == 1)          // Castle
            {
                BuildingFunctions.Placement(Castle);
            }
            else if (BuildingType == 2)     // House
            {
                BuildingFunctions.Placement(House);
            }
            else if (BuildingType == 3)     // Barracks
            {
                BuildingFunctions.Placement(StockPile);
            }
            else if (BuildingType == 4)     // Archery Field
            {

            }
            else if (BuildingType == 5)     // Stone Defence Tower
            {

            }
            else if (BuildingType == 6)     // Stables
            {

            }
        }


    }



}

