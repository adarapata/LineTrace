using UnityEngine;
using System.Collections;

public static class Vector2Extension
{
    /// <summary>
    /// 2次元ベクトルの外積を求める
    /// </summary>
    /// <param name="self"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Cross(this Vector2 self,Vector2 b)
    {
        return self.x * b.y - self.y * b.x;
    }
}
