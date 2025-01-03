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

}
