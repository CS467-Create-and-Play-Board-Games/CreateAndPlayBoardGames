
public class RollDice
{
    Random rnd = new Random();
    public int roll(int sides)
    {
        return rnd.Next(sides)+1
    }
}
