using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KID.TwoDHorizontal
{
    /// <summary>
    /// 血量系統：玩家
    /// </summary>
    public class HealthSystem : MonoBehaviour
    {
        public static HealthSystem instance;

        [Header("動畫參數")]
        [SerializeField] private string parameterHurt = "觸發受傷";
        [SerializeField] private string parameterDead = "開關死亡";
        [SerializeField, Header("介面圖片")]
        private Image imgHp;
        [SerializeField, Header("死亡事件")]
        private UnityEvent onDead;

        private Animator ani;
        private float hp = 100, hpMax;
        private ControlSystem controlSystem;

        private void Awake()
        {
            instance = this;
            hpMax = hp;
            ani = GetComponent<Animator>();
            controlSystem = FindObjectOfType<ControlSystem>();
        }

        /// <summary>
        /// 受傷
        /// </summary>
        /// <param name="damage">收到的傷害值</param>
        public void Hurt(float damage)
        {
            if (ani.GetBool(parameterDead)) return;
            
            SoundSystem.instance.PlaySoundRandomVolume(SoundSystem.instance.soundHurt, 0.8f, 1.2f);
            hp -= damage;
            imgHp.fillAmount = hp / hpMax;
            ani.SetTrigger(parameterHurt);
            if (hp <= 0) Dead();
        }

        /// <summary>
        /// 死亡
        /// </summary>
        private void Dead()
        {
            onDead?.Invoke();
            controlSystem.enabled = false;
            ani.SetBool(parameterDead, true);
            SoundSystem.instance.PlaySoundRandomVolume(SoundSystem.instance.soundDead, 0.8f, 1.2f);
        }
    }
}
