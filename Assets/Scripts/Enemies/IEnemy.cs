using UnityEngine;

namespace Enemies
{
    public interface IEnemy
    {
        float Health { get; set; }
        float AttackSpeed { get; }
        float DamagePerProjectile { get; }
        float MovementSpeed { get; }
    
        void Attack(Vector3 playerPosition);
        void Move(Vector3 playerPosition);
        void TakeDamage(float damage);
    }
}
