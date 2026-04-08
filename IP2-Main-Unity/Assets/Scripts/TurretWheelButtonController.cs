using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretWheelButtonController : MonoBehaviour
{
    public int ID;
    private Animator anim;
    public string itemName;
    public string itemDesciption;
    public string itemCost;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI itemDesciptionText;
    public TextMeshProUGUI itemCostText;
    public Image selectedItem;
    private bool selected = false;
    public Sprite icon;
    public Sprite noImage;
    
    
    

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        /*
        if (selected)
        {
            // keep selected button’s icon and name in the UI
            selectedItem.sprite = icon;
            itemText.text = itemName;
            itemDesciptionText.text = itemDesciption;
        }
        */
    }

    public void Selected()
    {
        //this will actually select the button, not deselect it, silly me
        selected = true;                              
        TurretWheelController.turretID = ID;   
        
        if (Building.current != null)
        {
            Building.current.StartBuildModeFromWheel(ID);
        }

        
        TurretWheelController wheel = FindFirstObjectByType<TurretWheelController>(); //idk unity was shouting at me for using FindObjectByType
        if (wheel != null)                                                                  
            wheel.SetOpen(false);
    }


    public void Deselected()
    {
        selected = false;

        
        if (TurretWheelController.turretID == ID)   
            TurretWheelController.turretID = 0;     
    }

    public void HoverEnter()
    {
        anim.SetBool("Hovered", true);
        itemText.text = itemName;
        itemDesciptionText.text = itemDesciption;
        selectedItem.sprite = icon;
        itemCostText.text = itemCost;
    }

    public void HoverExit()
    {
        anim.SetBool("Hovered", false);
        itemText.text = "";
        itemDesciptionText.text = "";
        itemCostText.text = "";
        selectedItem.sprite = noImage;

    }
}
