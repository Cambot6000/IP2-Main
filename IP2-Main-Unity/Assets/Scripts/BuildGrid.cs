using System.Collections.Generic;
using UnityEngine;

public class BuildGrid : MonoBehaviour
{

    [Header("Grid")]
 
    public int gridWidth;
    public int gridHeight;

    [Header("GridPrefabs")]

    public GameObject prefab;
    public float width;
    public float height;

     [Header("EnemiesPath")]

    public List<Vector3> pathWaypoint = new List<Vector3>();
    public List<(int x, int y)> path = new List<(int x, int y)> { (1, 0), (1, 1), (2, 1), (3, 1), (3, 2), (3, 3), (3, 4), (3, 5) };

    private void Start()
    {
        
        Renderer prefabR = prefab.GetComponent<Renderer>();
        width = prefabR.bounds.size.x;
        height = prefabR.bounds.size.y;

        // creating the grid loop
        for (int x = 0; x < gridWidth; x++)
        {
            
            for (int y = 0; y < gridHeight; y++)
            {
                

                Vector3 pos = new Vector3(transform.position.x + (width * x), transform.position.y, transform.position.z + (height * y));
                GameObject gridBox = Instantiate(prefab, pos, Quaternion.identity);
                Renderer gridBoxR = gridBox.GetComponent<Renderer>();// temporary
                if (path.Contains((x, y)))// finds path for enemies 
                {
                  
                    gridBoxR.material.color = Color.red;// temporary 
                    pathWaypoint.Add(gridBox.transform.position);

                }
                else
                {
                    gridBoxR.material.color = Color.green;// temporary
                }



            }


        }

        for (int i = 0; i < pathWaypoint.Count; i++)//temporary 
        {


            Debug.Log($"{pathWaypoint[i]}");
        }
    }
}
