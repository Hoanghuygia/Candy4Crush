using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData {
    public bool[] ActiveLevel;
    public int[] highScore;
    public int[] stars;
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;//no need to create reference to call it in other class
    public SaveData saveData;

    private void Awake() {
        if(gameData == null) {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else {
            Destroy(this.gameObject);//E.g when back from the main screen, it checks whether the splash scene exist or not
        }
        Load();

    }
    private void Start() {

    }
    public void Save() {
        //Create a binary formatter which can read binary files
        BinaryFormatter formatter = new BinaryFormatter();

        //Create a route from the program to the file
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create);

        SaveData data = new SaveData();
        data = saveData;
        //actualy save the data
        formatter.Serialize(file, data);
        file.Close();
        Debug.Log("Saved");
    }
    public void Load() {
        //Check whether the save game file exists
        if(File.Exists(Application.persistentDataPath + "/player.dat")) {
            //Create a Binary formatter
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("Loaded");
        }
    }
    private void OnDisable() {
        Save();
    }
}
