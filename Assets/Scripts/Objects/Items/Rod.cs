using System;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Rod", menuName = "ScriptableObjects/Rod", order = 1)]
public class Rod : Item
{
    public string rodId;
    public string RodId => rodId;
    public int reelInSpeed;
    public int throwRange;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(rodId))
        {
            rodId = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }

    public void AddRodToInstance()
    {
        MainManager.Instance.Inventory.FoundRods = MainManager.Instance.Inventory.FoundRods.Append(this).ToList();
    }
}
