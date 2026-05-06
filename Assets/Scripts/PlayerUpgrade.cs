using Unity.VisualScripting;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public PlayerExperience exp;
    public PlayerHealth health;
    public Player playerMove;
    public PlayerAttack attack;
   
    public int maxHpLevel = 5;
   
    public int maxDamageLevel = 5;
    
    public int maxSpeedLevel = 5;
    
    public int maxAttackSpeedLevel = 5;
    
    public int maxAutoHealLevel = 5;



    void Start()
    {
        if (exp == null) exp = GetComponent<PlayerExperience>();
        if (health == null) health = GetComponent<PlayerHealth>();
        if (playerMove == null) playerMove = GetComponent<Player>();
        if (attack == null) attack = GetComponent<PlayerAttack>();
    }

    public void UpgradeHP()
    {
        if (GameData.skillPoints <= 0 || GameData.hpLevel >= maxHpLevel) return;
        
        
        health.maxHealth += 10;
        health.currentHealth += 10;

        health.UpdateHealthBar(); 

        GameData.hpLevel++;
        GameData.skillPoints--;
        
        Debug.Log("HP Up!");
    }

    public void UpgradeDamage()
    {
        if (GameData.skillPoints <= 0 || GameData.damageLevel >= maxDamageLevel) return;

        attack.attackDamage += 2;
        GameData.damageLevel++;

        GameData.skillPoints--;
        Debug.Log("Damage Up!");
    }

    public void UpgradeSpeed()
    {
        if (GameData.skillPoints <= 0 || GameData.speedLevel >= maxSpeedLevel) return;

        playerMove.speed += 0.1f;
        GameData.speedLevel++;

        GameData.skillPoints--;
        Debug.Log("Speed Up!");
    }
    public void UpgradeAttackSpeed()
    {
        if (GameData.skillPoints <= 0 || GameData.attackSpeedLevel >= maxAttackSpeedLevel) return;

        attack.attackRate += 0.1f;
        GameData.attackSpeedLevel++;

        GameData.skillPoints--;
        Debug.Log("Attack Speed Up!");
    }
    public void UpgradeAutoHeal()
    {
        if (GameData.skillPoints <= 0 || GameData.autoHealLevel >= maxAutoHealLevel) return;

        health.healAmount += 1f;
        GameData.autoHealLevel++;

        GameData.skillPoints--;
        Debug.Log("Health Regen Up!");
    }
}