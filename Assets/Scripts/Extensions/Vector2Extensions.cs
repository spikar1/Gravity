using UnityEngine;
using System.Collections;

public static class Vector2Extensions
{

	public static Vector3 toVector3(this Vector2 vec2, float zVar = 0)
    {
        return new Vector3(vec2.x, vec2.y, zVar);
    }
}
    