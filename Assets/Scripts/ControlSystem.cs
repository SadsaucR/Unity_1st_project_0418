using UnityEngine;

namespace KID.TwoDHorizontal
{
    /// <summary>
    /// 控制系統:橫向卷軸移動與跳躍
    /// </summary>
    public class ControlSystem : MonoBehaviour
    {
        #region 資料
        [SerializeField, Header("移動速度"), Range(0, 100)]
        private float speedWalk;
        [SerializeField, Header("跑步倍速"), Range(1, 10)]
        private float speedRun = 1;
        [SerializeField, Header("跑步按鍵")]
        private KeyCode keyRun = KeyCode.LeftShift;
        
        [Header("跳躍高度、半徑、位移與圖層")]
        [SerializeField, Range(0, 1000)] private float jump;
        [SerializeField] private float groundRadius = 0.1f;
        [SerializeField] private Vector3 groundOffset;
        [SerializeField] private LayerMask layerGround;

        [Header("動畫參數")]
        [SerializeField] private string parameterWalk = "開關走路";
        [SerializeField] private string parameterRun = "開關跑步";
        [SerializeField] private string parameterJump = "觸發跳躍"; 

        private Rigidbody2D rig;
        private Animator ani;
        private bool isRun;
        private float speed;
        private bool isGround;
        #endregion

        #region 事件
        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
        }

        private void Update()
        {
            Move();
            Run();
            CheckGround();
            Jump();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(transform.position + groundOffset, groundRadius);
        }

        private void OnDisable()
        {
            rig.velocity = Vector3.zero;
            ani.SetBool(parameterWalk, false);
            ani.SetBool(parameterRun, false);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 移動
        /// </summary>
        private void Move()
        {
            float h = Input.GetAxis("Horizontal");
            rig.velocity = new Vector2(h * speed, rig.velocity.y);
            UpdateAnimationWalk(h);
            Flip(h);
        }

        /// <summary>
        /// 跑步
        /// </summary>
        private void Run()
        {
            isRun = Input.GetKey(keyRun);
            speed = isRun ? speedWalk * speedRun : speedWalk;
            UpdateAnimationRun();
        }

        /// <summary>
        /// 更新動畫：走路
        /// </summary>
        /// <param name="walkValue">走路的值</param>
        private void UpdateAnimationWalk(float walkValue)
        {
            ani.SetBool(parameterWalk, walkValue != 0);
        }

        /// <summary>
        /// 更新動畫：跑步
        /// </summary>
        private void UpdateAnimationRun()
        {
            ani.SetBool(parameterRun, isRun && ani.GetBool(parameterWalk));
        }

        /// <summary>
        /// 翻面
        /// </summary>
        /// <param name="walkValue">走路的值</param>
        private void Flip(float walkValue)
        {
            if (Mathf.Abs(walkValue) < 0.1f) return;
            float angle = walkValue >= 0 ? 0 : 180;
            transform.eulerAngles = Vector3.up * angle;
        }

        /// <summary>
        /// 檢查地板
        /// </summary>
        private void CheckGround()
        {
            isGround = Physics2D.OverlapCircle(transform.position + groundOffset, groundRadius, layerGround);
        }

        /// <summary>
        /// 跳躍
        /// </summary>
        private void Jump()
        {
            if (!isGround) return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rig.AddForce(Vector3.up * jump);
                UpdateAnimationJump();
            }
        }

        /// <summary>
        /// 更新動畫：跳躍
        /// </summary>
        private void UpdateAnimationJump()
        {
            ani.SetTrigger(parameterJump);
        } 
        #endregion
    }
}
