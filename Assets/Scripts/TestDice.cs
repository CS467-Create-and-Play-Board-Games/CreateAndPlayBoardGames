using System;

public class TestDice
{
    public static void Main()
    {
        // create a new custom dice and test
        string[] custFaces = {"Skip turn", "Swap with player x", "Go back 3 tiles", "Return to start", "Add 3 to next roll"};
        Dice custDice = new Dice(5, custFaces);

        // create rolldice obj
        RollDice rolled = new RollDice();

        Console.WriteLine("Rolling Dice:");
        int rolledNumber = rolled.Roll(custDice.nSides);
        string rolledFace = custDice.GetFace(rolledNumber);
        Console.WriteLine($"You rolled: {rolledFace}");
    }
}
