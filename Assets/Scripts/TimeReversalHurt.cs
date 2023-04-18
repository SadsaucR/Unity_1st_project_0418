using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace KID.TwoDHorizontal
{
    /// <summary>
    /// 時光倒流受傷效果
    /// </summary>
    public class TimeReversalHurt : MonoBehaviour
    {
        [SerializeField, Header("會造成傷害的物件名稱")]
        private string targetToHurt = "子彈";
        [SerializeField, Header("時光倒流次數"), Range(1, 10)]
        private int timeReversalCount = 3;
        [Header("動畫參數")]
        [SerializeField] private string parameterHurt = "觸發受傷";
        [SerializeField, Header("受傷時間"), Range(0, 2)]
        private float timeHurt = 0.7f;
        [Header("蛋")]
        [SerializeField] private GameObject prefabEgg;
        [SerializeField] private Vector3 offsetEgg;
        [SerializeField, Header("倒流完成事件")]
        private UnityEvent onDead;

        private Animator ani;
        private EnemySystem enemySystem;

        private void Awake()
        {
            ani = GetComponent<Animator>();
            enemySystem = GetComponent<EnemySystem>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.Contains(targetToHurt)) Hurt();
        }

        /// <summary>
        /// 受傷
        /// </summary>
        private void Hurt()
        {
            timeReversalCount--;
            ani.SetTrigger(parameterHurt);
            enemySystem.enabled = false;
            StartCoroutine(SmallEffect());
            SoundSystem.instance.PlaySoundRandomVolume(SoundSystem.instance.soundHurt, 0.8f, 1.2f);
            if (timeReversalCount > 0) Invoke("HurtFinish", timeHurt);
            else Dead();
        }

        private IEnumerator SmallEffect()
        {
            for (int i = 0; i < 10; i++)
            {
                transform.localScale -= Vector3.one * 0.02f;
                yield return null;
            }
        }

        /// <summary>
        /// 受傷結束
        /// </summary>
        private void HurtFinish()
        {
            enemySystem.enabled = true;
        }

        /// <summary>
        /// 死亡
        /// </summary>
        private void Dead()
        {
            Instantiate(prefabEgg, transform.position + offsetEgg, Quaternion.identity);
            onDead?.Invoke();
            Destroy(gameObject);
        }
    }
}
