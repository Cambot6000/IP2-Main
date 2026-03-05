//last edited 25/02/2026
//documentation and comments needed

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps; //tile map package

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

    // Tower Prefabs
    [Header("Tower Prefabs")]
    public GameObject Tower1; 
    public GameObject Tower2;
    public GameObject Tower3;
    public GameObject Tower4;
    //etc...
    
                  // tower prefab
    //public GameObject SlowingTower;
    //public GameObject poisonTower;

    // currently active object being placed
    private PlaceObject objectToPlace;

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
        
    private void Update()
    {

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
        
        /*
        if (Input.GetKeyDown(KeyCode.M) && objectToPlace == null)
        {
            InitalizeWithObject(poisonTower);
        }
        */

        // If not in build mode, skip the rest
        if (objectToPlace == null)
            return;

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
            }
            else
            {
                Destroy(objectToPlace.gameObject);
                objectToPlace = null; // exit build mode (failed)
            }
        }
        else if (cancel)
        {
            Destroy(objectToPlace.gameObject);
            objectToPlace = null; // exit build mode
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

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        // If any tile in area is already greenTile, cannot place
        
         //debug
        
        
        
        foreach (var b in baseArray)
        {
            if (b == greenTile)
            {
                return false;
            }
            
            int i = 0;
            i++; //debug
            Debug.Log($"CanBePlaced cell {i}: tile={(b != null ? b.name : "null")}, isGreen={(b == greenTile)}"); //debug
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


    public void StartBuildModeFromWheel(int id) //instead of using B for build mode 
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