using System.Collections.Generic;
using UnityEngine;

public class SavedData : GenericSingleton<SavedData>
{
    public int Throws;
    public string Name = "---";
    public string Time;

    public void UpdateThrows(int throws)
    {
        Throws = throws;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateTime(string time)
    {
        Time = time;
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    void Save()
    {
        List<Highscore> oldH = Utilities.LoadData<List<Highscore>>("Highscores.json");
        if (oldH == null)
            oldH = new();

        List<Highscore> newH = oldH;
        newH.Add(new(Throws,Name,Time));
        // newH = newH.OrderBy(x => x.Throws).ToList();

        Utilities.SaveData(newH, "Highscores.json");
    }

}
