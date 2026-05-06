using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerExperience : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip levelup;
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    public PlayerHealth playerHealth;
    public Player playerMove;
    public PlayerAttack playerAttack;
    public LevelUpUI levelUpUI;
    public EXPbar expBar;
    public TMP_Text levelUpText;
    [SerializeField] private TMP_Text SkillpointText;

    public void Awake()
    {
       levelUpText.enabled = false; 
    }
    void Start()
    {
        
    level = GameData.level;
    currentExp = GameData.currentExp;
    expToNextLevel = GameData.expToNextLevel;

    expBar.setMaxExp(expToNextLevel);
    expBar.setExp(currentExp);
    
    }
    void Update()
    {
        SkillpointText.text = "Skill Points: " + GameData.skillPoints;
    }

    public void GainExperience(int amount)
    {
        currentExp += amount;

        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        } 

        GameData.currentExp = currentExp;
        expBar.setExp(currentExp);
    }

    void LevelUp()
    {
    if (audioSource != null && levelup != null) {
        audioSource.PlayOneShot(levelup); 
    }

    currentExp -= expToNextLevel;
    
    GameData.level++;
    level = GameData.level;

    int skillPointsGained = 5 + level; // Số điểm kỹ năng nhận được mỗi khi lên cấp
    GameData.skillPoints += skillPointsGained;
    

    expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);

    // sync vào GameData
    
    GameData.currentExp = currentExp;
    GameData.expToNextLevel = expToNextLevel;

    expBar.setMaxExp(expToNextLevel);
    expBar.setExp(currentExp);

    StartCoroutine(ShowLevelUpText());
    }   

    IEnumerator ShowLevelUpText()
    {
        levelUpText.enabled = true; // Show the level up text
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        levelUpText.enabled = false; // Hide the level up text after 2 seconds
    }
}