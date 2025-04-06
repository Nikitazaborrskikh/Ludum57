using UnityEngine;

namespace Enemies.Bosses
{
    public class Encryption : BaseEnemy
    {
        [SerializeField] private SequenceController sequenceController;
        public override float AttackSpeed => config.encryptionStats.attackSpeed;
        public override float DamagePerProjectile => config.encryptionStats.damagePerProjectile;
        public override float MovementSpeed => config.encryptionStats.movementSpeedDivider;
        public override float DistanceToPlayer => config.encryptionStats.distanceToPlayer;

        private void Awake()
        {
            Health = config.encryptionStats.health;
        }

        public override void Attack(Vector3 playerPosition)
        {
            StartCoroutine(sequenceController.RunSequence());
        }

        public override void Move(Vector3 playerPosition) { }
    }
}
