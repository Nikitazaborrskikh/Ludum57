using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
    private Material material;
    private bool isActive = false;
    private bool isWarning = false;
    private bool hasDamaged = false; // Флаг, чтобы отслеживать, был ли уже нанесен урон
    
    void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            Debug.LogError($"MeshRenderer not found on {gameObject.name}!");
            return;
        }
        material = renderer.material;
        material.color = Color.white;
        if (material == null)
        {
            Debug.LogError($"Material not assigned on {gameObject.name}!");
        }
    }

    public void SetWarning(bool state)
    {
        isWarning = state;
        if (material != null)
        {
            material.color = state ? Color.yellow : Color.white;
        }
        else
        {
            Debug.LogWarning($"Cannot set warning color on {gameObject.name}: material is null");
        }
    }

    public void Activate()
    {
        isActive = true;
        hasDamaged = false; // Сбрасываем флаг при активации
        if (material != null)
        {
            material.color = Color.red;
            StartCoroutine(DeactivateAfterTime());
        }
    }

    IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(1f);
        isActive = false;
        if (material != null)
        {
            material.color = Color.white;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (isActive && !hasDamaged && other.gameObject.layer == LayerMask.NameToLayer("Player") /*Player.layer*/) // Проверяем, не нанесли ли уже урон
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Player is damaged");
                //playerHealth.TakeDamage(1);
                hasDamaged = true; // Устанавливаем флаг, чтобы урон больше не наносился
            }
        }
    }
}