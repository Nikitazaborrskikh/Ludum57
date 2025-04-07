using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectContextHolder : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("ProjectContext marked as DontDestroyOnLoad");
    }
}