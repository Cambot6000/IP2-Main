using UnityEngine;
using UnityEngine.UI;

public class TurretWheelController : MonoBehaviour
{
    //public Animator anim;
    //private bool turretWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int turretID;

    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            turretWheelSelected = !turretWheelSelected;
        }

        if (turretWheelSelected)
        {
            anim.SetBool("OpenTurretWheel", true);
        }
        else
        {
            anim.SetBool("OpenTurretWheel", false);
        }
        */

        switch (turretID)
        {
            case 0:   // case0 = nothing selected
                selectedItem.sprite = noImage;
                break;
            case 1:
                // do something
                Debug.Log("Turret 1 selected");
                break;
            case 2:
                // do something
                Debug.Log("Turret 2 selected");
                break;
            case 3:
                // do something
                Debug.Log("Turret 3 selected");
                break;
            case 4:
                // do something
                Debug.Log("Turret 4 selected");
                break;
            case 5:
                // do something
                Debug.Log("Turret 5 selected");
                break;
            case 6:
                // do something
                Debug.Log("Turret 6 selected");
                break;
            case 7:
                // do something
                Debug.Log("Turret 7 selected");
                break;
            case 8:
                // do something
                Debug.Log("Turret  selected");
                break;
        }

    }
}
