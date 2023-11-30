/*
using UnityEngine;
using System;
using System.IO;
using System.Text;
/*
public class FileManager : MonoBehaviour
{
    private static FileManager instance;
    public static FileManager Command { get { return instance; } }

    [SerializeField] private string saveFileName = "GameSaveFile";

    [Serializable]
    public struct DataToStore
    {
        //TODO : Struct cant return null, If class can return Null.
        public string Name;
        public Vector3 Position;

        public DataToStore(string Name, Vector3 Position)
        {
            this.Name = Name;
            this.Position = Position;
        }
    }

    private void Awake()
    {
        //Sigelton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    //SAVE FILE////////////////////////////////////////////////////////////////////////////////////////
    public void SaveData(Transform player)
    {
        string storedData = StoreThisData(player);
        SaveToHDD(saveFileName, storedData);
    }

    //Prosses What should be saved to Json format
    private string StoreThisData(Transform player)
    {
        //Create holder object
        DataToStore storedData = new DataToStore(player.name, player.position);

        //Turn class into json string
        string jsonString = JsonUtility.ToJson(storedData);
        return jsonString;
    }

    //Store on HDD
    public void SaveToHDD(string fileName, string jsonString)
    {
        // Open a file in write mode. This will create the file if it's missing.
        // It is assumed that the path already exists.
        // The "using" statement will automatically close the stream after we leave
        // the scope - this is VERY important
        using (FileStream stream = File.OpenWrite(Application.persistentDataPath + fileName))
        {
            // Truncate the file if it exists (we want to overwrite the file)
            stream.SetLength(0);

            // Convert the string into bytes. Assume that the character-encoding is UTF8.
            Byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

            // Write the bytes to the hard-drive
            stream.Write(bytes, 0, bytes.Length);
        }
    }

    //LOAD FILE////////////////////////////////////////////////////////////////////////////////////////
    public void LoadData(Transform player)
    {
        string jsonString = LoadFromHDD(saveFileName);

        //Gate if empty
        if (string.IsNullOrEmpty(jsonString))
        {
            Debug.LogError("<color=red>Nothing to load</color>");
            return;
        }

        SetDataToObject(jsonString, player);
    }

    private string LoadFromHDD(string fileName)
    {
        //Check if file exists / gate
        if (!File.Exists(Application.persistentDataPath + fileName))
            return null;

        // Open a stream for the supplied file name as a text file
        using (StreamReader stream = File.OpenText(Application.persistentDataPath + fileName))
        {

            // Read the entire file and return the result. 
            // This assumes that we've written the file in UTF-8
            return stream.ReadToEnd();
        }
    }

    //Set loaded data to scene
    private void SetDataToObject(string jsonString, Transform player)
    {
        DataToStore storedData = JsonUtility.FromJson<DataToStore>(jsonString);
        player.transform.name = storedData.Name;
        player.transform.position = storedData.Position;
    }
}
*/