using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KID.TwoDHorizontal
{
    /// <summary>
    /// 能量系統
    /// </summary>
    public class EnergySystem : MonoBehaviour
    {
        #region 資料
        [Header("能量值")]
        [SerializeField] private float maxEnergy = 100;
        [SerializeField] private float costPerEnergy = 10;
        [SerializeField] private float curePerEnergy = 1;
        [SerializeField] private float cureInterval = 1;

        [Header("介面")]
        [SerializeField] private TextMeshProUGUI textEnergy;
        [SerializeField] private Image imgEnergy;
        [SerializeField, Header("文字前綴")]
        private string stringTextFront = "時光倒流槍能量：";

        private float currentEnergy;
        #endregion

        #region 事件
        private void Awake()
        {
            currentEnergy = maxEnergy;
            CureEnergySwitch(true);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 減少能量
        /// </summary>
        public bool ReduceEnergy()
        {
            if (currentEnergy < costPerEnergy) return false;

            currentEnergy -= costPerEnergy;
            textEnergy.text = stringTextFront + currentEnergy;
            imgEnergy.fillAmount = currentEnergy / maxEnergy;

            return true;
        }

        /// <summary>
        /// 恢復能量轉換
        /// </summary>
        /// <param name="cureActive">是否啟動恢復</param>
        public void CureEnergySwitch(bool cureActive)
        {
            if (cureActive) InvokeRepeating("CureEnergy", 0, cureInterval);
        }

        /// <summary>
        /// 恢復能量
        /// </summary>
        private void CureEnergy()
        {
            currentEnergy += curePerEnergy;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
            textEnergy.text = stringTextFront + currentEnergy;
            imgEnergy.fillAmount = currentEnergy / maxEnergy;
        }

        /// <summary>
        /// 恢復能量
        /// </summary>
        public void CureEnergy(float energy)
        {
            currentEnergy += energy;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
            textEnergy.text = stringTextFront + currentEnergy;
            imgEnergy.fillAmount = currentEnergy / maxEnergy;
        }
        #endregion
    }
}
