using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUpgradeInteractor : MonoBehaviour
{
    
    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;  //keyboard key
    public string interactButton = "Submit"; //controller

    [Header("References")]
    public UpgradeUI upgradeUI;
    private TurretUpgrade currentTurret;

    
    private void OnTriggerEnter(Collider other)
    {
        TurretUpgrade turret = other.GetComponentInParent<TurretUpgrade>();
        if (turret != null)
        {
            currentTurret = turret;
            Debug.Log($"[Interactor] Enter turret {turret.name}");
            upgradeUI.ShowPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TurretUpgrade turret = other.GetComponentInParent<TurretUpgrade>();
        if (turret != null && turret == currentTurret)
        {
            Debug.Log($"[Interactor] Exit turret {turret.name}");
            currentTurret = null;
            upgradeUI.ShowPrompt(false);
            upgradeUI.Close();
        }
    }

    private void Update()
    {
        if (currentTurret == null)
            return;

        if (Input.GetKeyDown(interactKey) || Input.GetButtonDown(interactButton))
        {
            Debug.Log("[Interactor] Open UI");      
            upgradeUI.Open(currentTurret);
        }
    }
}
