using UnityEngine;
using System.Collections;

public static class Vector3Extension {
    /// <summary>
    /// XとZでVector2を生成する
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static Vector2 XZ(this Vector3 self)
    {
        return new Vector2(self.x, self.z);
    }
}
