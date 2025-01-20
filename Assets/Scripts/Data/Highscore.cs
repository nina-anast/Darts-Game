// class to determine the format the highscore elements should have.
// also determines the format the elements should have in when displayed.
public class Highscore 
{
    public float Throws;
    public string Name;
    public string Time;

    public Highscore() { }

    public Highscore(float throws, string name, string time)
    {
        Throws = throws;
        Name = name;
        Time = time;
    }

    public override string ToString()
    {
        return $"{Name} -> {Throws}";
    }
}
