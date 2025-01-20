using System.Collections.Generic;
using UnityEngine;

// runs in empty game object.
// singleton to save current progress of player.
public class SavedData : GenericSingleton<SavedData>
{
    public int Throws;
    public string Name = "---";
    public string Time;

    // updates in AimDart
    public void UpdateThrows(int throws)
    {
        Throws = throws;
    }

    // updates in GameOverUI
    public void UpdateName(string name)
    {
        Name = name;
    }

    // updates in Score
    public void UpdateTime(string time)
    {
        Time = time;
    }

    // saves data to .json file
    public void Save()
    {
        // gets a list of Highscores as defined in Highscores from .json
        // .json is located in C:\Users\User\AppData\LocalLow\DefaultCompany\Darts Game\Saved Data
        List<Highscore> oldH = Utilities.LoadData<List<Highscore>>("Highscores.json");
        if (oldH == null)
            oldH = new();

        // add new highscore entry from singleton
        List<Highscore> newH = oldH;
        newH.Add(new(Throws,Name,Time));

        // save the updated list to the .json file
        Utilities.SaveData(newH, "Highscores.json");
    }

}
