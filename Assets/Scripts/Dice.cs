using System;

public class Dice
{
    public int nSides;
    public string[] faces;

    public Dice(int sides, string[] custDice)
    {
        nSides = sides;
        faces = new string[nSides];
        
        for (int i = 0; i < nSides; i++)
        {
            faces[i] = custDice[i];
        }
    }

    public Dice(int sides)
    {
        nSides = sides;
        faces = new string [nSides];

        for (int i = 0; i < nSides; i++)
        {
            faces[i] = i.ToString();
        }
    }

    public string GetFace(int rolledNumber)
    {
        return faces[rolledNumber - 1];
    }
}
