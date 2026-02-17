using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretWheelButtonController : MonoBehaviour
{
    public int ID;
    private Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    public Image selectedItem;
    private bool selected = false;
    public Sprite icon;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (selected)
        {
            selectedItem.sprite = icon;
            itemText.text = itemName;
        }
    }

    public void Selected()
    {
        selected = true;
        TurretWheelController.turretID = ID;
    }

    public void Deselected()
    {
        selected = false;
        TurretWheelController.turretID = 0;
    }

    public void HoverEnter()
    {
        anim.SetBool("Hovered", true);
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hovered", false);
        itemText.text = "";
    }
}
