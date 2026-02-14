using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    [Header("Building Script Settings")]
    public static Building current;

    public Transform player;
    public PlayerControls controls;
    public float placeDistance = 1f;         // how far in front of player to place preview
    public GridLayout gridSize;             // reference to the grid layout
    private Grid grid;                      // cached grid component

    // main tile map
    public Tilemap MainTilemap;             // tracks which cells are occupied
    public TileBase greenTile;              // tile used to mark taken cells

    // towers (later: array)
    
    public GameObject Tower1;               // tower prefab

    // currently active object being placed
    private PlaceObject objectToPlace;

    private void Awake()
    {
        current = this;
            grid = gridSize.gameObject.GetComponent<Grid>();
        controls = new PlayerControls(); //setup building controls
    }
    
    
    //controls 
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
        
        //set to "B" but for testing
        // Enter build mode with Tower1
        if (Input.GetKeyDown(KeyCode.B) && objectToPlace == null)
        {
            InitalizeWithObject(Tower1);
        }

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
        foreach (var b in baseArray)
        {
            if (b == greenTile)
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        int maxX = start.x + size.x - 1;
        int maxY = start.y + size.y - 1;

        MainTilemap.BoxFill(
            start,
            greenTile,
            start.x,
            start.y,
            maxX,
            maxY
        );
    }
}
