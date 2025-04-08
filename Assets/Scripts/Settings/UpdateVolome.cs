using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVolome : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Volume", 1f);
    }
}
