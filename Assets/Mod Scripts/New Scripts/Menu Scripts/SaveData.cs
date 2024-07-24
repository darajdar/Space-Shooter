using UnityEngine;
//This namespace is used when accessing files
using System.IO;
//Used for compiling data?
using System.Runtime.Serialization.Formatters.Binary;


//Static classes can not be instantiated
public static class SaveData 
{
   public static void SavePlayer(ModGlobalControl data)
    {
        //This will save the data to a folder location under the name 'SaveData'
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.proj";
        //This reads and writes data from a file
        FileStream Stream = new FileStream(path, FileMode.Create);

        
        GameData _Data = new GameData(data);


        formatter.Serialize(Stream, _Data);
        Stream.Close();
    }

    public static GameData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/SaveData.proj";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream Stream = new FileStream(path, FileMode.Open);

            GameData _Data = formatter.Deserialize(Stream) as GameData;

            Stream.Close();
            return _Data;
        } else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
//https://www.youtube.com/watch?v=XOjd_qU2Ido Tutorial