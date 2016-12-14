using UnityEngine;

namespace LineTrace
{
    public class Line
    {
        public Line next, prev;
        public Vector3 front, back;

        public Line(Vector3 f, Vector3 b)
        {
            front = f;
            back = b;
        }

        /// <summary>
        /// 任意の向きの目的地を返す
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Vector3 GetWayPointByDirection(Direction d)
        {
            return d == Direction.front ? front : back;
        }

        /// <summary>
        /// 任意の向きの次の線を返す
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Line GetNextLineByDirection(Direction d)
        {
            return d == Direction.front ? next : prev;
        }
    }
}