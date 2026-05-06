using UnityEngine;
public static class GameData
{
     public static int level = 0;
    public static int currentExp = 0;
    public static int expToNextLevel = 70;

    public static int skillPoints = 10;

    public static int hpLevel = 0;
    public static int damageLevel = 0;
    public static int speedLevel = 0;
    public static int attackSpeedLevel = 0;
    public static int autoHealLevel = 0;

    public static void Reset()
    {
        level = 0;
        currentExp = 0;
        expToNextLevel = 70;

        skillPoints = 10;
        hpLevel = 0;
        damageLevel = 0;
        speedLevel = 0;
        attackSpeedLevel = 0;
        autoHealLevel = 0;
    }
}

