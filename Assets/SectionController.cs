using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SectionController : MonoBehaviour
{
    [Inject] private PlayerStats playerStats;
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
        Debug.Log(other.gameObject.name);
        if (isActive && !hasDamaged && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player is collided");
            if (playerStats != null)
            {
                Debug.Log("Player is damaged");
                playerStats.TakeDamage(1f);
                hasDamaged = true; 
            }
        }
    }
}