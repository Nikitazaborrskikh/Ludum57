using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Enemies.Bosses
{
    public class Encryption : BaseEnemy
    {
        public AudioClip shootSound;
        public AudioClip DieSound;
        public GameObject audioSource;
        [SerializeField] private Slider healthSlider;

        [SerializeField] private SequenceController sequenceController;
        public override float AttackSpeed => config.encryptionStats.attackSpeed;
        public override float DamagePerProjectile => config.encryptionStats.damagePerProjectile;
        public override float MovementSpeed => config.encryptionStats.movementSpeedDivider;
        public override float DistanceToPlayer => config.encryptionStats.distanceToPlayer;

        private void Awake()
        {
            healthSlider.maxValue = config.encryptionStats.health;
            healthSlider.value = config.encryptionStats.health;
            Health = config.encryptionStats.health;
        }

        public override void Attack(Vector3 playerPosition)
        {
            StartCoroutine(sequenceController.RunSequence());
        }
        private IEnumerator StartSound(AudioClip Sound)
        {
            audioSource.GetComponent<AudioSource>().PlayOneShot(Sound);
            yield return null;
        }

        public override void TakeDamage(float damage)
        {
            
            Health -= damage;
            healthSlider.value -= damage;
            if (Health <= 0)
            {
                StartCoroutine(StartSound(DieSound));
                Die();
            }
        }

        public override void Move(Vector3 playerPosition) { }
    }
}
