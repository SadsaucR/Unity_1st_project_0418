using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace KID.TwoDHorizontal
{
    /// <summary>
    /// 吃蛋
    /// </summary>
    public class EatEgg : MonoBehaviour
    {
        [SerializeField, Header("蛋的名稱")]
        private string nameEgg = "蛋";
        [SerializeField, Header("文字蛋")]
        private TextMeshProUGUI textEgg;
        [SerializeField, Header("吃到蛋的事件")]
        private UnityEvent onEat;
        [SerializeField, Header("任務完成")]
        private UnityEvent onMissionComplete;

        private EnergySystem energySystem;
        private int countEgg;
        private int countEggMax = 3;

        private void Awake()
        {
            energySystem = FindObjectOfType<EnergySystem>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains(nameEgg)) Eat(collision.gameObject);
        }

        /// <summary>
        /// 吃蛋
        /// </summary>
        private void Eat(GameObject egg)
        {
            energySystem.CureEnergy(30);

            UpdateEggUI();
            if (countEgg == 1) onEat?.Invoke();

            Destroy(egg);
        }

        /// <summary>
        /// 更新蛋文字介面
        /// </summary>
        private void UpdateEggUI()
        {
            countEgg++;
            textEgg.text = $"外星生物的蛋：{countEgg} / {countEggMax}";

            if (countEgg == countEggMax) onMissionComplete?.Invoke();
        }
    }
}
