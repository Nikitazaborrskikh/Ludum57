using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public GameObject managers;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            managers.GetComponent<LevelsManager>().BlinkAndSwitchScene(SceneManager.GetActiveScene().buildIndex + 1, true);
        }
    }
}
