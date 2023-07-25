public class PlayerLevel
{
    public int Level { get; set; }
    public int Exp { get; set; }

    public int baseExp = 100;

    public int requiredExp = 0;

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
            LevelChanged();
        }
    }
    public void LevelChanged()
    {
        Level++;
        Exp = 0;
        CalculateExpRequired(Level);
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