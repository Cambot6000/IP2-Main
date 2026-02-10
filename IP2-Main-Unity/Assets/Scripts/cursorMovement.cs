using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;

public class cursorMovement : MonoBehaviour
{
    [Header("References")]
    public BuildGrid gridManager;

    [Header("Movement Variables")]
    public float moveSpeed;
    public float gridSize; //Will become the width of the prefabs used in the grid if I've done everything right
    private bool isMoving = false;
    private Vector2 inputDirection;

    private float maxX;
    private float minX;
    private float minZ;
    private float maxZ;

    private void Start()
    {
        if (gridManager != null)
        {
            //Sync gridSize to the width of the prefabs used in the grid
            gridSize = gridManager.width;

            //Gets the boundaries based on the dimensions of th grid
            minX = gridManager.transform.position.x;
            maxX = minX + (gridManager.gridWidth - 1) * gridSize;

            minZ = gridManager.transform.position.z;
            maxZ = minZ + (gridManager.gridHeight - 1) * gridSize;

            //Should hopefully snap the starting position of the cursor right to the grid origin
            transform.position = new Vector3(minX, transform.position.y + 1173.6f, minZ);
        }
    }
    //This method is called by the "Player Input" component from the new Unity inpit system thing
    public void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
    }

    void Update()
    {
        if (!isMoving && inputDirection != Vector2.zero)
        {
            Vector3 moveTarget = Vector3.zero;

            //Prioritize horizontal movement so that the cursor will move sideyways if more than one input direction is going on at once
            if (Mathf.Abs(inputDirection.x) > 0.5f)
            {
                moveTarget = new Vector3(Mathf.Sign(inputDirection.x) * gridSize, 0, 0);
            }
            else if (Mathf.Abs(inputDirection.y) > 0.5f)
            {
                moveTarget = new Vector3(0, 0, Mathf.Sign(inputDirection.y) * gridSize);
            }

            if (moveTarget != Vector3.zero)
            {
                Vector3 potentialPosition = transform.position + moveTarget;

                //Keeps the cursor within the generated grid bounds from the other scripot - BuilfGrid.cs
                if (potentialPosition.x <= maxX + 0.1f && potentialPosition.x >= minX - 0.1f &&
                    potentialPosition.z <= maxZ + 0.1f && potentialPosition.z >= minZ - 0.1f)
                {
                    StartCoroutine(MovePlayer(moveTarget));
                }
            }
        }
    }


    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        Vector3 targetPosition = transform.position + direction;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}
