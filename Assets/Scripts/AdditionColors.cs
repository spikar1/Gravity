using UnityEngine;
using System.Collections;

static public class AdditionColors{

	public static Color GreenDoor()
    {
        return new Vector4(0 / 255f, 75 /255f, 50 / 255f, 1);
    }
    public static Color GreenKey(this Color color)
    {
        return new Vector4(0, 150, 100, 1);
    }
}
