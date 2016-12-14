using System;
using UnityEngine;
using System.Collections;
using UniRx;
using System.Linq;
using System.Collections.Generic;

namespace LineTrace
{
    public class LineManager : MonoBehaviour
    {
        [SerializeField, TooltipAttribute("循環させるか")] private bool cycle = false;

        public List<Transform> waypoints;
        List<Line> lineList = new List<Line>();
        // Use this for initialization
        void Awake()
        {
            waypoints.Select(w => w.position)
                .ToObservable()
                .Buffer(2, 1)
                .Take(waypoints.Count - 1)
                .Subscribe(wp =>
                {
                    var back = wp.First();
                    var front = wp.Last();
                    Line l = new Line(front, back);
                    if (lineList.Count > 0)
                    {
                        lineList.Last().next = l;
                        l.prev = lineList.Last();
                    }
                    lineList.Add(l);
                }, () =>
                {
                    // 循環させる場合、最初と最後を繋ぐ線を追加する
                    if (cycle)
                    {
                        Line cycleLine = new Line(waypoints.First().position, waypoints.Last().position);
                        cycleLine.next = lineList.First();
                        cycleLine.prev = lineList.Last();
                        lineList.First().prev = cycleLine;
                        lineList.Last().next = cycleLine;
                    }
                });
        }

        /// <summary>
        /// 一番近い位置にあるLINEを返す
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Line GetLineAtNearDistance(Vector3 position)
        {
            return lineList.FindMin(l =>
            {
                var center = Vector3.Lerp(l.back, l.front, 0.5F);
                return Mathf.Abs(Vector3.Distance(position, center));
            });
        }

        #region Scene上で線を表示する

        public Color gizmoColor = Color.yellow;

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            waypoints.Select(w => w.position)
                .ToObservable()
                .Buffer(2, 1)
                .Subscribe(wp =>
                    {
                        Gizmos.DrawLine(wp.First(), wp.Last());
                        Gizmos.DrawSphere(wp.First(), 0.3F);
                    }
                    , () =>
                    {
                        if (cycle)
                        {
                            Gizmos.DrawLine(waypoints.Last().position, waypoints.First().position);
                        }
                    });
        }

        #endregion
    }
}