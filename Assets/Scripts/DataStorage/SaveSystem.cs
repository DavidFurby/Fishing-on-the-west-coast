using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{

    public static void SaveGame(Game game)
    {
        try
        {
            BinaryFormatter formatter = new();

            string path = Application.persistentDataPath + "/game.save";
            FileStream stream = new(path, FileMode.Create);

            GameData data = new(game);

            formatter.Serialize(stream, data);
            stream.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("An error occurred while saving: " + e.Message);
        }

    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/game.save";
        if (File.Exists(path))
        {
            try
            {
                BinaryFormatter formatter = new();
                FileStream stream = new(path, FileMode.Open);

                GameData data = formatter.Deserialize(stream) as GameData;
                stream.Close();
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError("An error occurred while loading " + e.Message);
                return null;
            }

        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    public static bool NewGame()
    {
        string path = Application.persistentDataPath + "/game.save";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file has been removed");
            return true;
        }
        else
        {
            Debug.Log("Save file not found");
            return false;
        }
    }
}
