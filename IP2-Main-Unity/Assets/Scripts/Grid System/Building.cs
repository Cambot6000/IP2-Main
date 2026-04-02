//last edited 25/02/2026
//documentation and comments needed

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps; //tile map package
using UnityEngine.InputSystem.DualShock; //Used for playstation controller

public class Building : MonoBehaviour
{
    [Header("Building Script Settings")]
    public static Building current;

    public Transform player;
    private PlayerControls controls;
    public float placeDistance = 1f;         // how far in front of player to place preview
    public GridLayout gridSize;             // reference to the grid layout
    private Grid grid;                      // cached grid component
    public bool wheelOpen;

    // main tile map
    public Tilemap MainTilemap;             // tracks which cells are occupied
    public TileBase greenTile;              // tile used to mark taken cells

    [Header("Player Icon Stuff")]
    public SpriteRenderer playerIcon;
    public float pulseSpeed = 2.0f;
    public float minAlpha; //How transparent the icon gets
    public float maxAlpha; //How solid the player icon gets both of these are when the player is about to place a dino

    // Tower Prefabs
    [Header("Tower Prefabs")]
    public GameObject Tower1; 
    public GameObject Tower2;
    public GameObject Tower3;
    public GameObject Tower4;
    //etc...
    

    // currently active object being placed
    private PlaceObject objectToPlace;

    public DualSenseGamepadHID dualSense;
    [Header("Lightbar Settings For PS Controller")]
    public float colourChangeSpeed = 5f;
    public Color currentColour = Color.blue;
    public Color targetColour = Color.blue;

    //////// Grid Building System ////////
    #region GridBuilding System;

    private void Awake()
    {
        current = this;
            grid = gridSize.gameObject.GetComponent<Grid>();
        controls = new PlayerControls(); //setup building controls
    }
    
    
    //controler setup
        private void OnEnable()
        {
            controls.Enable();
        }
    
        private void OnDisable()
        {
            controls.Disable();
        }

    private void Start()
    {
        dualSense = Gamepad.current as DualSenseGamepadHID;
    }
    private void Update()
    {
        if(dualSense != null)
        {
            PickTargetColour();
            ColouringIn();
        }
        
        //if (objectToPlace.turretType == TurretType.None || objectToPlace == null)
        //{
            //if (dualSense != null)
            //{
                //Clears the light bar when no tower selected
                //ChangeLightBar(Color.blue);
            //}
        //}
        //var dualSense = Gamepad.current as DualSenseGamepadHID;
        //if (dualSense != null)
        //{
            //Set the light bar to blue to test
            //dualSense.SetLightBarColor(Color.blue);
        //}

        if (wheelOpen)
            return;
        
       // // //turret wheel ui intergration// // //
        // Enter build mode 
        //whatever the wheel has set as the new turretID will decide that prefab is placed, basicly the same as the old system but now allows UI wheel
        bool enterBuild = Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame; // //if "B" key was pressed this frame "B" is still for debug

        if (enterBuild)
        {   
            StartBuildModeFromWheel(TurretWheelController.turretID);
        }

        // If not in build mode, skip the rest
        if (objectToPlace == null)
            return;

        if(objectToPlace != null && playerIcon != null)
        {
            float pingPong = Mathf.PingPong(Time.time * pulseSpeed, 1.0f);
            float newAlpha = Mathf.Lerp(minAlpha, maxAlpha, pingPong);
            //Ping pong moves back and forth, in this case making the player icon have a pulse like animation when building

            Color tempColour = playerIcon.color;
            tempColour.a = newAlpha;
            playerIcon.color = tempColour;
            /*
            if (objectToPlace.turretType == TurretType.Turret1)
            {
                if (dualSense != null)
                {
                    //Set the light bar to yellow like the base turret
                    //ChangeLightBar(Color.yellow);
                }
            }
            else if (objectToPlace.turretType == TurretType.Turret2)
            {
                if (dualSense != null)
                {
                    //Set the light bar to red like the AOE turret. Might change the colour later though
                    //ChangeLightBar(Color.red);
                }
            }
            else if (objectToPlace.turretType == TurretType.Turret3)
            {
                if (dualSense != null)
                {
                    //Set the light bar to magenta like the poison turret
                    //ChangeLightBar(Color.magenta);
                }
            }
            else if (objectToPlace.turretType == TurretType.None || objectToPlace == null)
            {
                if (dualSense != null)
                {
                    //Clears the light bar when no tower selected
                    //ChangeLightBar(Color.blue);
                }
            }
            OLD COLOUR CHANGING LIGHT BAR CODE NOT NEEDED ANYMORE
            */
        }
           

        else if (playerIcon != null)
        {
            //Reset the alpha to full when the player isnt trying to place a tower
            //Alpha is transparency
            Color tempColour = playerIcon.color;
            tempColour.a = 1.0f;
            playerIcon.color = tempColour;
        }

        // Keep preview object in front of the player, snapped to grid, change to maybe flash or be transparent
        objectToPlace.transform.position = PlacementWorldPos();

        //controler support
        // controler & keyboard support
        bool place  = controls.Player.Place.triggered; 
        bool cancel = controls.Player.Cancel.triggered;

        if (place)
        {
            if (CanBePlaced(objectToPlace)) //call canbeplaced if it comes back true run:
            {
                objectToPlace.Place();//calls the place method for the object/tower currently being placed as places the onject
                Vector3Int start = grid.WorldToCell(objectToPlace.getStartPos()); //returns the world pos of the object, WorldToCell converts it to grid cell coords
                TakeArea(start, objectToPlace.size);//this checks how many cells wide the object/tower is then "paints" the cells so that we know the cell space is now ocupied 
                objectToPlace = null; // exit build mode as we have placed an objec
                //ChangeLightBar(Color.blue);
            }
            else
            {
                Destroy(objectToPlace.gameObject);
                objectToPlace = null; // exit build mode (failed)
                //ChangeLightBar(Color.blue);
                //print("Changed to blue");
            }
        }
        else if (cancel)
        {
            Destroy(objectToPlace.gameObject);
            objectToPlace = null; // exit build mode
            //ChangeLightBar(Color.blue);
            //print("Changed to blue2");
        }

        }

    /*
    private void ChangeLightBar(Color colourName)
    {
        if (dualSense != null)
        {
            dualSense.SetLightBarColor(colourName);
        }
    }
    */

    private void PickTargetColour()
    {
        if(objectToPlace == null || objectToPlace.turretType == TurretType.None)
        {
            targetColour = Color.blue; //Default because normally a PS controller shows blue
        }
        else if(objectToPlace.turretType == TurretType.Turret1)
        {
            targetColour = Color.yellow; //For the basic tower
        }
        else if (objectToPlace.turretType == TurretType.Turret2)
        {
            targetColour = new Color(215,0,47); //AoE Tower
        }
        else if (objectToPlace.turretType == TurretType.Turret3)
        {
            targetColour = Color.magenta; //Poison guy
        }
        else if (objectToPlace.turretType == TurretType.Turret4)
        {
            targetColour = Color.green; //placeholder colour to test
        }
    }

    private void ColouringIn()
    {
        if(dualSense != null)
        {
            currentColour = Color.Lerp(currentColour, targetColour, Time.unscaledDeltaTime * colourChangeSpeed); //Slowly move to next colour on lightbar
            dualSense.SetLightBarColor(currentColour);
        }
    }
    
    // Position in front of the player, snapped to grid
    private Vector3 PlacementWorldPos()
    {
        Vector3 origin = player.position;
        Vector3 forward = player.forward;
        forward.y = 0f;       // keep it flat on the plane
        forward.Normalize();

        Vector3 forwardPos = origin + forward * placeDistance;
        return SnapCoords(forwardPos);
    }

    public Vector3 SnapCoords(Vector3 position)
    {
        Vector3Int cellPos = grid.WorldToCell(position);
        return grid.GetCellCenterWorld(cellPos);
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }

    // Spawn tower and enter build mode
    public void InitalizeWithObject(GameObject prefab)
    {
        Vector3 position = PlacementWorldPos();
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceObject>();
    }

    private bool CanBePlaced(PlaceObject placeObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridSize.WorldToCell(objectToPlace.getStartPos());
        area.size = placeObject.size;

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap); //check

        // If any tile in area is already greenTile, cannot place
        foreach (var tile in baseArray)
        {
            if (tile == greenTile)
                return false;
        }

        return true;
    }


    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        int maxX = start.x + size.x - 1;
        int maxY = start.y + size.y - 1; //this is the only way i could make the "cube" fit extactly in the cell if anyone wants to try messing with the scaling feel free!

        MainTilemap.BoxFill(
            start,
            greenTile,
            start.x,
            start.y,
            maxX,
            maxY
        );
    }
    #endregion
    
    //////// Ui Wheel Intergration ////////
    #region Ui Wheel Intergration


    public void StartBuildModeFromWheel(int id) //start build mode
    {
        if (objectToPlace != null)
        {
            return;
        }
        GameObject prefab = GetTurretPrefabFromId(id);

        if (prefab != null)
        {
            InitalizeWithObject(prefab);
        }
        else
        {
            Debug.Log("! Debug ! --> No Turret Selected");
        }
    }
    
    public enum TurretType //this allows the ui wheel to ask what turret type is selected
    {
        None = 0,
        Turret1 = 1,
        Turret2 = 2,
        Turret3 = 3,
        Turret4 = 4,
       //etc....
       
    }
    
    public GameObject GetTurretPrefabFromId(int id) //return the prefab for the turret type
    {
        TurretType type = (TurretType)id;

        switch (type)
        {
            case TurretType.Turret1:
                return Tower1;
            case TurretType.Turret2:
                return Tower2;
            case TurretType.Turret3:
                return Tower3;
            case TurretType.Turret4:
                return Tower4;
            //etc..
            default:
                return null;
        }
    }

    public void SetWheelOpen(bool open)
    {
        wheelOpen = open;
    }
    
    
    #endregion
    
}