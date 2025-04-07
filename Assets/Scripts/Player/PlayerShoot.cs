using UnityEngine;

namespace Player
{
    public class PlayerShoot : Shoot
    {
        public GameObject bulletStandartPrefab; // ������ ����
        public GameObject bulletLargePrefab; // ������ ����
        public Vector3 offset;

        public float coolDownStandartAttack = 1f;
        public float coolDownLargeAttack = 1.5f;

        public AudioClip shootSound;
        private AudioSource audioSource;

        private float timeAfterLastShoot;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            timeAfterLastShoot += Time.deltaTime;    
            if (Input.GetButtonDown("Fire1") && timeAfterLastShoot >= coolDownStandartAttack)
            {
                audioSource.PlayOneShot(shootSound);
                BulletManager.Instance.ShootBullet(bulletStandartPrefab, gameObject, transform.position + transform.rotation * offset, transform.rotation);
                timeAfterLastShoot = 0f;
            }
            if (Input.GetButtonDown("Fire2") && timeAfterLastShoot >= coolDownLargeAttack)
            {
                audioSource.PlayOneShot(shootSound);
                BulletManager.Instance.ShootBullet(bulletLargePrefab, gameObject, transform.position + transform.rotation * offset, transform.rotation);
                timeAfterLastShoot = 0f;
            }
        }


    }
}