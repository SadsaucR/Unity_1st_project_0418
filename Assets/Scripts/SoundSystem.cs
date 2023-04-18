using UnityEngine;

namespace KID
{
    /// <summary>
    /// 音效系統
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundSystem : MonoBehaviour
    {
        public static SoundSystem instance;

        [Header("音效")]
        public AudioClip soundFire;
        public AudioClip soundHurt;
        public AudioClip soundDead;

        private AudioSource aud;

        private void Awake()
        {
            instance = this;
            aud = GetComponent<AudioSource>();
        }

        /// <summary>
        /// 播放音效隨機音量
        /// </summary>
        /// <param name="sound">音效</param>
        /// <param name="min">最小音量</param>
        /// <param name="max">最大音量</param>
        public void PlaySoundRandomVolume(AudioClip sound, float min, float max)
        {
            float volume = Random.Range(min, max);
            aud.PlayOneShot(sound, volume);
        }
    }
}
