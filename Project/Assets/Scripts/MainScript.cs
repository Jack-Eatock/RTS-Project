using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Gathers inputs from the user and handles most Functions.
public class MainScript : MonoBehaviour {

    // Variables \\
    public string Condition;
    public Camera Cam;
    public GameObject Unit_Pos_Prefab;
    public GameObject Unit_Pos_Controller;
    public float Last_time = 0f;
    public static bool Selected_Type_Unit = false;
    public static bool IsBuilding;
    public static bool Area_Clear = true;
    public static bool Panel_Active = false;

    public static List<GameObject> UnitsOnScreen = new List<GameObject>();
    public static List<GameObject> UnitsInDrag = new List<GameObject>();
    public static List<GameObject> Selected = new List<GameObject>();
    public static List<GameObject> Unit_Positioning = new List<GameObject>();


    public bool FirstLoop = true;
    public GameObject MarkerPrefab;
    private RaycastHit R_Click_hit;
    private GameObject Position_Controller;
    private GameObject Marker;


    // Drag select Variables \\
    private bool UserIsDragging = false;
    private bool FinishedDragOnThisFrame;
    private Vector3 MouseDownPoint;
    private float Box_Height;
    private float Box_Width;
    private float BoxTop;
    private float BoxLeft;
    private static Vector2 BoxStart;
    private static Vector2 BoxFinish;
    private Vector3 CurrentMousePoint; // World space

    [SerializeField]
    private GUIStyle MouseDragSkin;

    // ---------------------------------------------------   Usefull Functions   ------------------------------------------------------------------\\

    // RayCastHit Stores the Vector3 position of where the mouse is
    public static RaycastHit Raycast_hit()
    {
        RaycastHit hit = BuildingScript.Location();
        return (hit);
    }

    // Checks if the user clicks outside of the UI Panel.
    public static bool IfClickedOutsidePanel()
    {
        GameObject Panel = GameObject.Find("Canvas").GetComponent<CanvasScript>().Panel;
        if (Panel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(Panel.GetComponent<RectTransform>(), Input.mousePosition))
        {
            return (true); 
        }
        else return false;
       
    }

    // If Shift Key is being pressed.
    public static bool IsShiftKeyDown()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return true;
        }
        return false;
    }

    // If Unit is already selected.
    public static bool UnitAlreadyInSelected(GameObject Unit)
    {
        foreach (GameObject Item in Selected)
        {
            if (Item == Unit)
            {
                return true;
            }

        }
        return false;
    }

    // Deselect all / Selects all
    public static void SelectAll(bool x)
    {
        if (x == true){
            foreach (GameObject Item in Selected){
                Item.GetComponent<UnitScript>().Selected = true;
            }
        }
        else{
            foreach (GameObject Item in Selected){
                Item.GetComponent<UnitScript>().Selected = false;
            }
            MainScript.Selected.Clear();}
    }

    // Checks if a value is less than 0 if it is then it makes it positive.
    public static float Unsigned(float Val){
        if (Val < 0f) Val *= -1;
        return Val;
    }

    // Check if a unit is within the screen space to deal with mouse drag selecting
    public static bool UnitWthinScreenSpace(Vector2 UnitScreenPos){
        if (
            (UnitScreenPos.x < Screen.width && UnitScreenPos.y < Screen.height) &&
            (UnitScreenPos.x > 0f && UnitScreenPos.y > 0f)
            )
            return true;
        else return false;
    }

    // Remove a unit from Screen Units UnitsOnScreen Array
    public static void RemoveFromOnScreenUnits(GameObject Unit){
        for (int i = 0; i < UnitsOnScreen.Count; i++) {
            GameObject Unitobj = UnitsOnScreen[i] as GameObject;
            if (Unit == Unitobj)  {
                UnitsOnScreen.RemoveAt(i);
                Unitobj.GetComponent<UnitScript>().OnScreen = false;
                return;
            }
        }
        return;
    }

    // Is unit inside the Drag?
    public bool UnitInsideDrag(Vector2 UnitScreenPos)
    {
        if (
            (UnitScreenPos.x > BoxStart.x && UnitScreenPos.y < BoxStart.y) &&
            (UnitScreenPos.x < BoxFinish.x && UnitScreenPos.y > BoxFinish.y)
            ) return true;
        else return false;
    }

    // Check if unit is in UnitstInsideDrag array list
    public static bool UnitAlreadyInDraggedUnits(GameObject Unit){
        if (UnitsInDrag.Count > 0){
            for (int i = 0; i < UnitsInDrag.Count; i++){
                GameObject ArrayListUnit = UnitsInDrag[i] as GameObject;
                if (ArrayListUnit == Unit){
                    return true;
                }
            }
            return false;
        }
        else{
            return false;
        }
    }

    // Take all Units from UnitsInDrag, in to selectedUnits
    public static void PutDraggedUnitsInCurrentlySelectedUnits(){
        if (UnitsInDrag.Count > 0){
            for (int i = 0; i < UnitsInDrag.Count; i++){
                GameObject Unitobj = UnitsInDrag[i] as GameObject;
                // If unit is not already in Selected, Add it
                if (UnitAlreadyInSelected(Unitobj) == false){
                    Debug.Log("Added unit");
                    Selected.Add(Unitobj);
                }
            }
        }

        UnitsInDrag.Clear();






    }

    // Updates at the end of each frame.
    void LateUpdate()
    {
        UnitsInDrag.Clear();
        // If user is dragging, or finished on this frame, AND there are units to select on screen.
        if ((UserIsDragging || FinishedDragOnThisFrame) && UnitsOnScreen.Count > 0)   {

            // for loop though those units on screen
            for (int i = 0; i < UnitsOnScreen.Count; i++){
                GameObject Unitobj = UnitsOnScreen[i] as GameObject;  
                UnitScript unitScript = Unitobj.GetComponent<UnitScript>();
                GameObject SelectedObj = Unitobj.transform.Find("Selection box").gameObject;
                // Makes it so it can only drag select either buildings or Units.
              
                   

                //if not already in Dragged Units
                if (!UnitAlreadyInDraggedUnits(Unitobj)) {
                    if (UnitInsideDrag(unitScript.ScreenPos)) {
                        if (Unitobj.transform.tag == "Unit")
                        {
                            SelectedObj.SetActive(true);
                            UnitsInDrag.Add(Unitobj);
                        }       
                    }

                    // Unit is not in drag.
                    else {
                        //remove the selected graphic, if unit not already in currently selected units.
                        if (!UnitAlreadyInSelected(Unitobj)){
                            SelectedObj.SetActive(false);
                        }
                    }
                }
            }
            if (FinishedDragOnThisFrame)
            {
                FinishedDragOnThisFrame = false;
                PutDraggedUnitsInCurrentlySelectedUnits();
                Selected_Type_Unit = true;
            }
        }
    }

        // ---------------------------------------------------      Main Code    ------------------------------------------------------------------\\

    // Use this for initialization
    void Start() {
        GameObject.Find("Ground").GetComponent<BuildingScript>().BuildingType = 1;
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit = Raycast_hit();
        CurrentMousePoint = hit.point;
        SelectAll(true);
        if (Selected.Count == 0)
        {
            Selected_Type_Unit = false;
        }

        // ---------------------------------------------------    Left Click Functions  ------------------------------------------------------------------\\
 
        // Left click presed DOWN.
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownPoint = hit.point;
            Last_time = Time.time;
        }

        // Left Click UP.
        else if (Input.GetMouseButtonUp(0))
        {
            // If they click without pressing shift and no UI is open de select all.
            if (!IsShiftKeyDown())
            {
                if (!GameObject.Find("Canvas").GetComponent<CanvasScript>().Panel.activeSelf)
                {
                    MainScript.SelectAll(false);
                }
            }
            // If they release left mouse button within two second , just a click:
            if (Time.time - Last_time < 0.2)
            {
                //Debug.Log("Left Click");
                //If the user clicks and are not buidling.
                if (IsBuilding == false)
                {
                    // If Shift is not being pressed:
                    if (!IsShiftKeyDown()) {
                        MainScript.Selected_Type_Unit = false;
                        // If the user clicks on a building.
                        if (hit.transform.gameObject.tag == "Building")
                        {
                            MainScript.SelectAll(false);
                            MainScript.Selected_Type_Unit = false;
                            Debug.Log("Building clicked" + hit.transform.gameObject.name);
                            MainScript.Selected.Add(hit.transform.gameObject);
                            // Nifty stick, removes brackets and contents from the string. \\
                            string Value = hit.transform.gameObject.name;
                            int firstBracket = Value.IndexOf('(');
                            int lastBracket = Value.LastIndexOf(')');
                            int diff = lastBracket - firstBracket + 1;
                            Value = Value.Remove(firstBracket, diff);

                            if (Value == "Castle")
                            {
                                Debug.Log("Castle");
                                GameObject.Find("Canvas").GetComponent<CanvasScript>().ActivateUi(1);
                            }
                            else if (Value == "House")
                            {
                                Debug.Log("House");
                                GameObject.Find("Canvas").GetComponent<CanvasScript>().ActivateUi(2);
                            }

                        }

                        // if object is a unit.
                        else if (hit.transform.gameObject.tag == "Unit")
                        {
                            MainScript.Selected_Type_Unit = true;
                            MainScript.Selected.Add(hit.transform.gameObject);
                        }

                        else if (MainScript.IfClickedOutsidePanel())
                        {
                            GameObject.Find("Canvas").GetComponent<CanvasScript>().ActivateUi(0);
                            MainScript.SelectAll(false);
                        }
                    }
                    // If Shift is being pressed:
                    else {
                        Debug.Log("Shift click");
                        GameObject.Find("Canvas").GetComponent<CanvasScript>().ActivateUi(0);
                        Debug.Log(Selected_Type_Unit);
                        // If the user clicks on a building.  
                        if (MainScript.Selected_Type_Unit == false) {
                            if (hit.transform.gameObject.tag == "Building") {
                                bool Test = true;
                                foreach (GameObject Item in MainScript.Selected) {
                                    if (Item == hit.transform.gameObject) {
                                        Test = false;
                                    }
                                }
                                if (Test == true)
                                {
                                    MainScript.Selected.Add(hit.transform.gameObject);
                                }
                                Debug.Log("Building clicked");
                            }
                        }
                        else
                        {
                            if (hit.transform.gameObject.tag == "Unit")
                            {
                                if (MainScript.UnitAlreadyInSelected(hit.transform.gameObject) == false)
                                {
                                    MainScript.Selected.Add(hit.transform.gameObject);
                                    Debug.Log("Unit Selected");
                                }
                                else
                                {
                                    MainScript.Selected.Remove(hit.transform.gameObject);
                                    hit.transform.gameObject.GetComponent<UnitScript>().Selected = false;
                                    Debug.Log("Unit Deselected");
                                }
                            }
                        }
                    }
                }
                else if (Area_Clear == true){
                GameObject.Find("Ground").GetComponent<BuildingScript>().BuildingType = 0;  
                }
            }

            // If they release left mouse button after holding down left click.
            else{
                UserIsDragging = false;
                FinishedDragOnThisFrame = true;
                Condition = "";
            }
        }

        // User HELD down left click
        else if (Input.GetMouseButton(0)){
            if (Time.time - Last_time >= 0.2){
                UserIsDragging = true;
                if (IsBuilding == false) {
                    Condition = "SelectionBox";
                }
            }
        }

        // ---------------------------------------------------    Right Click Functions  ------------------------------------------------------------------\\
        // When Right click is pressed down, the position of the mouse is stored
        // and it takes note of the time at which they clicked to be used later 
        // to tell if it was held down or just a click.

        // Initial right click down. 
        else if (Input.GetMouseButtonDown(1)){
            Last_time = Time.time;
            R_Click_hit = Raycast_hit();
            if (Selected_Type_Unit == true){
                Marker = GameObject.Instantiate(MarkerPrefab, new Vector3(R_Click_hit.point.x, 0.55f, R_Click_hit.point.z), MarkerPrefab.transform.rotation);
            }

        }

        //  Right click once  \\
        else if (Input.GetMouseButtonUp(1)){
            Destroy(Marker);
            if (Time.time - Last_time < 0.2){
                if (Selected_Type_Unit == true) {
                    foreach (GameObject Unit in MainScript.Selected)
                    {
                        Unit.GetComponent<UnitMovementScript>().Destination = R_Click_hit.point;
                        Unit.GetComponent<UnitMovementScript>().Rotation_Directed = false;
                    }
                }
            }
        }

        //  If they hold down Right click  \\
        else if (Input.GetMouseButton(1)) {
            if (Time.time - Last_time >= 0.2) {
                if (Selected_Type_Unit == true){
                    if (FirstLoop == true) {
                        Destroy(Marker);
                        int Disp_ = -1;
                        FirstLoop = false;
                        Position_Controller = GameObject.Instantiate(Unit_Pos_Controller, R_Click_hit.point, Unit_Pos_Controller.transform.rotation);
                        foreach (GameObject Unit in MainScript.Selected){
                            Disp_ += 1;
                            GameObject Position_G = GameObject.Instantiate(Unit_Pos_Prefab, new Vector3(R_Click_hit.point.x + Disp_, 0.54f, R_Click_hit.point.z), Unit_Pos_Prefab.transform.rotation);
                            Position_G.transform.SetParent(Position_Controller.transform);
                            Unit_Positioning.Add(Position_G);
                        }
                    }
                    Position_Controller.transform.Rotate(0, Input.GetAxis("Mouse X") * 2.5f, 0);
                }
            }
        }

        // ---------------------------------------------------    Other Key Functions   ------------------------------------------------------------------\\

        // R key is pressed
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if (IsBuilding == true)
            {
                GameObject.Find("Ground").GetComponent<BuildingScript>().RotateBuilding();
            }
        }

        // F key is pressed
        else if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject.Find("Ground").GetComponent<BuildingScript>().BuildingType = 2;
        }

        // If the user wants to delete
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            // Deletes every object selected
            foreach (GameObject Item in Selected)
            {
                // Appart from the castle.
                if (Item.gameObject.name != "Castle(1)")
                {
                    Destroy(Item.gameObject);
                    UnitsOnScreen.Remove(Item.gameObject);
                }

            }
            SelectAll(false);
 
        }

        // If right click is not pressed, tells selected units to move to position and rotation.    --- ((NEEDS TO BE OPTIMISED!!)) ---
        else if (Input.GetMouseButton(1) == false){
            if (FirstLoop == false) {
                int i = 0;
                foreach (GameObject Unit in Selected){
                    Unit.GetComponent<UnitMovementScript>().Destination = Unit_Positioning[i].transform.position;
                    Unit.GetComponent<UnitMovementScript>().Rotation = Unit_Positioning[i].transform.rotation;
                    Unit.GetComponent<UnitMovementScript>().Rotation_Directed = true;
                    i += 1;
                }
                Destroy(Position_Controller);
                Unit_Positioning.Clear();
                FirstLoop = true;
                Last_time = Time.time;
            }
        }
    }

    void OnGUI()
    {
        switch (Condition)
        {

            case "SelectionBox":
                // Box width, height, top, left.
                Box_Width = Camera.main.WorldToScreenPoint(MouseDownPoint).x - Camera.main.WorldToScreenPoint(CurrentMousePoint).x;
                Box_Height = Camera.main.WorldToScreenPoint(MouseDownPoint).y - Camera.main.WorldToScreenPoint(CurrentMousePoint).y;
                BoxLeft = Input.mousePosition.x;
                BoxTop = (Screen.height - Input.mousePosition.y) - Box_Height;

                // Which corner of drag box is the mouse.

                // Mouse Top Left
                if (Box_Width > 0f && Box_Height < 0f)
                {
                    BoxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
                // Mouse Bottom Left
                else if (Box_Width > 0f && Box_Height > 0f)
                {
                    BoxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y + Box_Height);
                }
                // Mouse Top right
                else if (Box_Width < 0f && Box_Height < 0f)
                {
                    BoxStart = new Vector2(Input.mousePosition.x + Box_Width, Input.mousePosition.y);
                }
                // Mouse Bottom Right
                else if (Box_Width < 0f && Box_Height > 0f)
                {
                    BoxStart = new Vector2(Input.mousePosition.x + Box_Width, Input.mousePosition.y + Box_Height);
                }

                BoxFinish = new Vector2(
                                        BoxStart.x + Unsigned(Box_Width),
                                        BoxStart.y - Unsigned(Box_Height)
                    );

                Debug.Log(BoxStart + BoxFinish);

                GUI.Box(new Rect(BoxLeft,
                                    BoxTop,
                                    Box_Width,
                                    Box_Height), "", MouseDragSkin);


                break;

            case "":
                break;



        }
    }
}
