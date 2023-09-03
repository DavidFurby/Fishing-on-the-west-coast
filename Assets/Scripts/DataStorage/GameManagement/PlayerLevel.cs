using System;

public class PlayerLevel
{
    public int Level { get; set; }
    public int Exp { get; set; }

    public int baseExp = 100;

    public int requiredExp = 0;

        public static event Action OnLevelUp;


    public void SetPlayerLevel(int level, int exp)
    {
        this.Level = level;
        this.Exp = exp;
        CalculateExpRequired(level);
    }
    public void CalculateExpRequired(int level)
    {
        requiredExp = level * baseExp;
    }
    public void AddExp(int exp)
    {
        Exp += exp;
        if (Exp >= requiredExp)
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        Level++;
        Exp = 0;
        CalculateExpRequired(Level);
        OnLevelUp.Invoke();
    }

    public float ThrowRangeModifier()
    {
        return 1 + (float)Level / 100;
    }

    public float ReelingSpeedModifier()
    {
        return 1 + (float)Level * 2 / 100;
    }
}