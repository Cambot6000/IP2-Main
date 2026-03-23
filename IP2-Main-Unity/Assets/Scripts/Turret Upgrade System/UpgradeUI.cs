//Ui for the turret upgrade system

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class UpgradeUI : MonoBehaviour
{
    [Header("Panel 1")]
    public GameObject panel;
    public TextMeshProUGUI titleText;
    
    [Header("Option 1 UI")]
    public Button option1Button;
    public TextMeshProUGUI option1LabelText;
    public TextMeshProUGUI option1CostText;
    
    [Header("Option 2 UI")]
    public Button option2Button;
    public TextMeshProUGUI option2LabelText;
    public TextMeshProUGUI option2CostText;
    
    [Header("Prompt")]
    public GameObject prompt; //upgrade prompt when player is above turret
    
    private TurretUpgrade currentTurret;

    void Awake() 
    {
        if (panel != null)
            panel.SetActive(false);
        
        if (prompt != null)
            prompt.SetActive(false);
        else
        {
            Debug.LogWarning("[UpgradeUI] prompt is NULL"); //debug
        }
    }

    public void ShowPrompt(bool show)
    {
        if (prompt != null)
        {
            prompt.SetActive(show);
            Debug.Log($"[UpgradeUI] ShowPrompt({show})"); //debug
        }
        else
        {
            Debug.LogWarning("[UpgradeUI] ShowPrompt but prompt NULL"); //debug
        }
    }
    
    public void Open(TurretUpgrade turret)
    {
        Debug.Log("[UpgradeUI] Open"); //debug
            
        currentTurret = turret;
        
        if (panel != null)
            panel.SetActive(true);
        
        ShowPrompt(false);
        
        //fill text
        if (titleText != null)
            titleText.text = turret.turretName;
        
        option1LabelText.text = turret.option1Label;
        option1CostText.text = turret.option1Cost.ToString();

        option2LabelText.text = turret.option2Label;
        option2CostText.text = turret.option2Cost.ToString();

        // Wire buttons
        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();

        option1Button.onClick.AddListener(OnOption1);
        option2Button.onClick.AddListener(OnOption2);


        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(option1Button.gameObject);
        }
        else
        {
            Debug.LogWarning("[UpgradeUI] No EventSystem in scene"); 
        }
    }

    public void Close()
    {
        Debug.Log("[UpgradeUI] Close");
        
        if (panel != null)
            panel.SetActive(false);
        
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
        
        currentTurret = null;
    
        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();
    }

    void Update()
    {
        if (panel != null && panel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel")) //if clocked escape or CLOSE button, close UI
            {
                Close();
            }
        }
    }

    private void OnOption1() //aplly upgrade option 1
    {
        Debug.Log("[UpgradeUI] OnOption1 click");
        
        if (currentTurret == null) return;
        
        currentTurret.ApplyOption1();
        Close();
    }
    
    private void OnOption2() //apply upgrade option 2
    {
        Debug.Log("[UpgradeUI] OnOption2 click");
        
        if (currentTurret == null) return;
        
        currentTurret.ApplyOption2();
        Close();
    }
    
}
