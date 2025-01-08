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
}
