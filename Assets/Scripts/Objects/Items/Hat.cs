using System;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Hat", menuName = "ScriptableObjects/Hat", order = 1)]
public class Hat : Item
{
    public string hatId;
    public string HatId => hatId;

        private void OnValidate()
    {
        if (string.IsNullOrEmpty(hatId))
        {
            hatId = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }


    public void AddHatToInstance()
    {
        MainManager.Instance.Inventory.FoundHats =
            MainManager.Instance.Inventory.FoundHats.Append(this).ToList();
    }

}