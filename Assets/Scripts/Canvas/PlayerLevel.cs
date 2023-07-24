public class PLayerLevel {
    public int level;
    public int exp;

    public int baseExp = 100;

    public int requiredExp = 0;

    public PLayerLevel(int level, int exp) {
        this.level = level;
        this.exp = exp;
        CalculateExpRequired(level);
    }
    public void CalculateExpRequired(int level) {
       requiredExp = level * baseExp;
    }

    public void LevelChanged(int level) {
        this.level++;
        exp = 0;
        CalculateExpRequired(level);
    }
}