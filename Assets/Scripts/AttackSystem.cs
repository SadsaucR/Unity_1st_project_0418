using UnityEngine;

namespace KID.TwoDHorizontal
{
    /// <summary>
    /// 攻擊系統
    /// </summary>
    public class AttackSystem : MonoBehaviour
    {
        #region 資料
        [SerializeField, Header("子彈生成位置")]
        private Vector3 pointSpawnBullet;
        [SerializeField, Header("子彈預製物")]
        private GameObject prefabBullet;
        [SerializeField, Header("子彈速度"), Range(0, 5000)]
        private float speedBullet = 800;
        [SerializeField, Header("動畫參數")]
        private string parameterAttack = "觸發開槍";
        
        private Animator ani;
        private EnergySystem energySystem;
        #endregion

        #region 事件
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0.5f, 0.3f, 0.3f);
            Gizmos.DrawSphere(
                transform.position +
                transform.TransformDirection(pointSpawnBullet),
                0.05f);
        }

        private void Awake()
        {
            ani = GetComponent<Animator>();
            energySystem = FindObjectOfType<EnergySystem>();
        }

        private void Update()
        {
            Fire();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 開槍
        /// </summary>
        private void Fire()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!energySystem.ReduceEnergy()) return;

                SoundSystem.instance.PlaySoundRandomVolume(SoundSystem.instance.soundFire, 0.8f, 1.2f);
                UpdateAnimationAttack();
                GameObject tempBullet = Instantiate(
                    prefabBullet,
                    transform.position +
                    transform.TransformDirection(pointSpawnBullet),
                    Quaternion.Euler(0, transform.eulerAngles.y, 0));
                ShootBullet(tempBullet);
            }
        }

        /// <summary>
        /// 更新動畫：攻擊
        /// </summary>
        private void UpdateAnimationAttack()
        {
            ani.SetTrigger(parameterAttack);
        }

        /// <summary>
        /// 發射子彈
        /// </summary>
        private void ShootBullet(GameObject bullet)
        {
            Rigidbody2D rigBullet = bullet.GetComponent<Rigidbody2D>();
            rigBullet.AddForce(transform.right * speedBullet);
        } 
        #endregion
    }
}
