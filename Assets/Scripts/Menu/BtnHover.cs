using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;
    public AudioClip sound;
    private AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("isHover", true);
        audioSource.PlayOneShot(sound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("isHover", false);
    }
}
