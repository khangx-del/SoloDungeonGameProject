using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Maxmin : MonoBehaviour
{
    public TMP_Text hptext;
    public TMP_Text damagetext;
    public TMP_Text speedtext;
    public TMP_Text attackSpeedtext;
    public TMP_Text autoHealtext;
    public PlayerUpgrade playerUpgrade;
    public TMP_Text levelText;
    public PlayerExperience playerExperience;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
{
    if (playerUpgrade == null)
        playerUpgrade = FindAnyObjectByType<PlayerUpgrade>();

    if (playerExperience == null)
        playerExperience = FindAnyObjectByType<PlayerExperience>();
}
void Update()
{
    RefreshUI();
}

    // Update is called once per frame
    public void RefreshUI()
{
    if (playerUpgrade == null || playerExperience == null) return;

    hptext.text = $"Health: {GameData.hpLevel}/{playerUpgrade.maxHpLevel}";
    damagetext.text = $"Damage: {GameData.damageLevel}/{playerUpgrade.maxDamageLevel}";
    speedtext.text = $"Speed: {GameData.speedLevel}/{playerUpgrade.maxSpeedLevel}";
    attackSpeedtext.text = $"Attack Speed: {GameData.attackSpeedLevel}/{playerUpgrade.maxAttackSpeedLevel}";
    autoHealtext.text = $"Auto Heal: {GameData.autoHealLevel}/{playerUpgrade.maxAutoHealLevel}";
    levelText.text = $"Level: {GameData.level}";
}
}
