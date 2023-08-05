using System;
using System.Linq;

[Serializable]
public class PayoutLineModel
{
    public int Line;
    public string Coordinates;

    public int[] GetCoordinatesArray()
    {
        var coordinates = Coordinates.Split(',').Select(int.Parse).ToArray();
        var coordinateString = "";
        for (int i = 0; i < coordinates.Length; i++)
        {
            coordinateString += coordinates[i] + "/";
        }
        return coordinates;
    }
}
