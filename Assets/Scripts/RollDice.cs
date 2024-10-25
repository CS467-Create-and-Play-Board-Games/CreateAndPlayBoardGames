using System;

public class RollDice
{
    private Random rnd = new Random();
    public int Roll(int sides)
    {
        return rnd.Next(sides)+1;
    }
}
