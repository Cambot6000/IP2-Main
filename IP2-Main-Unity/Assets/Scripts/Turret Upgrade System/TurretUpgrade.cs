//turret upgrade system


using UnityEngine;


public class TurretUpgrade : MonoBehaviour
{
  //varibles and settings
  [Header("Ui Settings")] 
  
  public string turretName = "Basic Turret";

  [Header("Upgrade Option 1")]  //just a template this can be any upgrade idea, right now its a universal upgrade but we can change it too have specific upgrades for towers
  public string option1Label = "Increase Damage";
  public int option1Cost = 100;
  public int damageIncrease = 10;
  
  [Header("Upgrade Option 2")] 
  public string option2Label = "Increase Range";
  public int option2Cost = 120;
  public float rangeIncrease = 1.5f; 
  
  private TowerFire towerFire;

  void Awake()
  {
    towerFire = GetComponent<TowerFire>(); 
  }

  public void ApplyOption1() //apply the first upgrade option to the tower
  {
    if (MoneyManager.instance == null)
    {
      Debug.LogWarning("No MoneyManager instance found"); //debug and for safty 
      return;
    }

    if (!MoneyManager.instance.SpendGold(option1Cost)) //if there is NOT enough money for upgrade  
    {
      Debug.Log("Not enough gold for upgrade 1"); //debug
      return;
    }
    
    //apply the upgrade
    towerFire.damage += damageIncrease;
    Debug.Log($"{turretName} damge upgraded to {towerFire.damage}");

  }

  public void ApplyOption2() //similar as above
  {
    if (MoneyManager.instance == null)
    {
      Debug.LogWarning("No MoneyManager instance found"); //debug and for safty 
      return;
    }

    if (!MoneyManager.instance.SpendGold(option2Cost)) //if there is NOT enough money for upgrade  
    {
      Debug.Log("Not enough gold for upgrade 2"); //debug
      return;
    }
    
    //apply the upgrade
    towerFire.range += rangeIncrease;
    towerFire.UpdateRangeRing(); //change size of the range ring
    
    Debug.Log($"{turretName} range upgraded to {towerFire.range}");
    
  }
}
