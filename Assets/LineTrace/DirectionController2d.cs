using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace LineTrace
{
    public class DirectionController2d : MonoBehaviour
    {
        /// <summary>
        /// 正面ベクトル
        /// </summary>
        public Vector3 forward { get; private set; }

        private Direction mDirection;

        /// <summary>
        /// 向いている方向
        /// </summary>
        public Direction direction
        {
            get { return mDirection; }
            set
            {
                mDirection = value;
                var f = (current.GetWayPointByDirection(mDirection) - transform.position);
                f.y = 0F;
                forward = f.normalized;
            }
        }

        private Line current;
        [SerializeField] private float distance = 0.1F;

        [SerializeField] private bool autoRotation = false;

        [SerializeField] private float rotateSpeed = 30F;
        // Use this for initialization
        void Start()
        {
            var lineManager = FindObjectOfType<LineManager>();
            current = lineManager.GetLineAtNearDistance(transform.position);

            this.UpdateAsObservable()
                // 目的のポイントに接近したかどうか
                .Where(
                    _ =>
                        Mathf.Abs(Vector2.Distance(transform.position.XZ(),
                            current.GetWayPointByDirection(mDirection).XZ())) < distance)
                .Subscribe(_ =>
                {
                    var next = current.GetNextLineByDirection(mDirection);
                    current = next ?? current;
                    direction = mDirection;
                });

            // 目的地の方向に自動で回転する
            if (autoRotation)
            {
                this.UpdateAsObservable()
                    .Where(_ => forward != Vector3.zero)
                    .Subscribe(_ =>
                    {
                        var arrivedForward = forward.XZ();
                        var cross = transform.forward.XZ().Cross(arrivedForward);
                        var dot = Vector2.Dot(transform.forward.XZ(), arrivedForward);

                        if (dot < 0.98F)
                        {
                            transform.Rotate(Vector3.down, rotateSpeed * Mathf.Sign(cross) * Time.deltaTime);
                        }
                    });
            }
        }
    }

    public enum Direction
    {
        front,
        back,
        none
    }
}