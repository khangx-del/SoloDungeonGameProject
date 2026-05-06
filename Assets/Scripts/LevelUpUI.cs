using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    public PlayerUpgrade upgrade;

    public void UpgradeHP()
    {
        upgrade.UpgradeHP();
       
    }

    public void UpgradeDamage()
    {
        upgrade.UpgradeDamage();
      
    }

    public void UpgradeSpeed()
    {
        upgrade.UpgradeSpeed();
        
    }
    public void UpgradeAttackSpeed()
    {
        upgrade.UpgradeAttackSpeed();
        
    }
    public void UpgradeAutoHeal()
    {
        upgrade.UpgradeAutoHeal();
    }
}