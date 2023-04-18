using UnityEngine;

namespace KID.TwoDHorizontal
{
    /// <summary>
    /// 敵人系統
    /// </summary>
    public class EnemySystem : MonoBehaviour
    {
        #region 資料
        [SerializeField, Header("目標名稱與圖層")]
        private string nameTarget;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField, Header("移動速度"), Range(0, 10)]
        private float speed = 3;
        [Header("遊走")]
        [SerializeField] private Vector3 pointWanderLeft = new Vector3(-1, 0);
        [SerializeField] private Vector3 pointWanderRight = new Vector2(1, 0);
        [Header("追蹤")]
        [SerializeField] private Vector3 trackOffset;
        [SerializeField] private Vector3 trackSize = Vector3.one;
        [Header("攻擊")]
        [SerializeField] private Vector3 attackOffset;
        [SerializeField] private Vector3 attackSize = Vector3.one;
        [SerializeField, Range(0, 5)] private float attackInterval = 2.5f;
        [SerializeField, Range(0, 100)] private float attack = 30;
        [SerializeField, Range(0, 1.5f)] private float attackDelay = 0.3f;

        [Header("動畫參數")]
        [SerializeField] private string parameterRun = "開關跑步";
        [SerializeField] private string parameterHurt = "觸發受傷";
        [SerializeField] private string parameterAttack = "觸發攻擊";

        private Transform transformTarget;
        private Animator ani;
        private Rigidbody2D rig;

        private enum StateEnemy
        {
            Wander, Track, Attack
        }
        [SerializeField] private StateEnemy state;

        /// <summary>
        /// 方向：右邊 +1，左邊 -1
        /// </summary>
        private int direction = -1;

        private bool isAttack;
        private float timerAttack;
        #endregion

        #region 事件
        private void OnDisable()
        {
            rig.velocity = Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.3f, 0.8f, 0.5f, 0.3f);
            Gizmos.DrawSphere(pointWanderLeft, 0.08f);
            Gizmos.DrawSphere(pointWanderRight, 0.08f);

            Gizmos.color = new Color(0.8f, 0.6f, 0.1f, 0.3f);
            Gizmos.DrawCube(
                transform.position +
                transform.TransformDirection(trackOffset),
                trackSize);

            Gizmos.color = new Color(1, 0.6f, 0.5f, 0.3f);
            Gizmos.DrawCube(
                transform.position +
                transform.TransformDirection(attackOffset),
                attackSize);
        }

        private void Awake()
        {
            ani = GetComponent<Animator>();
            rig = GetComponent<Rigidbody2D>();
            transformTarget = GameObject.Find(nameTarget).transform;
            timerAttack = attackInterval;
        }

        private void Update()
        {
            State();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 狀態
        /// </summary>
        private void State()
        {
            switch (state)
            {
                case StateEnemy.Wander:
                    Wander();
                    break;
                case StateEnemy.Track:
                    Track();
                    break;
                case StateEnemy.Attack:
                    Attack();
                    break;
            }
        }

        /// <summary>
        /// 遊走
        /// </summary>
        private void Wander()
        {
            rig.velocity = Vector3.right * speed * direction;
            bool right = direction == 1 && transform.position.x > pointWanderRight.x;
            bool left = direction == -1 && transform.position.x < pointWanderLeft.x;
            if (right || left) ChangeDirection();
            UpdateAnimationRun();

            if (CheckTarget()) state = StateEnemy.Track;
        }

        /// <summary>
        /// 變更方向
        /// </summary>
        private void ChangeDirection()
        {
            direction = direction == -1 ? 1 : -1;
            float angle = transform.eulerAngles.y == 180 ? 0 : 180;
            transform.eulerAngles = Vector3.up * angle;
        }

        /// <summary>
        /// 更新動畫：跑步
        /// </summary>
        private void UpdateAnimationRun()
        {
            ani.SetBool(parameterRun, rig.velocity.x != 0);
        }

        /// <summary>
        /// 檢查目標
        /// </summary>
        private bool CheckTarget()
        {
            Collider2D hit = Physics2D.OverlapBox(
                transform.position +
                transform.TransformDirection(trackOffset),
                trackSize, 0, targetLayer);

            return hit;
        }

        /// <summary>
        /// 檢查目標是否在攻擊區域內
        /// </summary>
        private bool CheckTargetInAttackArea()
        {
            Collider2D hit = Physics2D.OverlapBox(
                transform.position +
                transform.TransformDirection(attackOffset),
                attackSize, 0, targetLayer);

            return hit;
        }

        /// <summary>
        /// 追蹤
        /// </summary>
        private void Track()
        {
            timerAttack = attackInterval;
            rig.velocity = Vector3.right * speed * direction;
            if (direction == -1 && transformTarget.position.x > transform.position.x) ChangeDirection();
            else if (direction == 1 && transformTarget.position.x < transform.position.x) ChangeDirection();

            if (!CheckTarget()) state = StateEnemy.Wander;
            else if (CheckTargetInAttackArea()) state = StateEnemy.Attack;
        }

        /// <summary>
        /// 攻擊
        /// </summary>
        private void Attack()
        {
            if (timerAttack >= attackInterval)
            {
                ani.SetTrigger(parameterAttack);
                timerAttack = 0;

                Invoke("DelaySendDamage", attackDelay);
            }
            else
            {
                timerAttack += Time.deltaTime;
            }

            if (!CheckTargetInAttackArea()) state = StateEnemy.Wander;
        }

        /// <summary>
        /// 延遲傳送傷害
        /// </summary>
        private void DelaySendDamage()
        {
            if (CheckTargetInAttackArea())
            {
                HealthSystem.instance.Hurt(attack);
            }
        }
        #endregion
    }
}
