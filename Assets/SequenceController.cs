using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SequenceController : MonoBehaviour
{
    public SectionController[] sections;
    public float warningTime = 1f;
    public float activeTime = 1f;
    [SerializeField] private int activeSectionsCount = 2;

    void Start()
    {
        if (sections == null || sections.Length == 0)
        {
            Debug.LogError("Sections array is empty! Please assign sections in the Inspector.");
            return;
        }
        if (activeSectionsCount > sections.Length)
        {
            Debug.LogWarning($"activeSectionsCount ({activeSectionsCount}) exceeds sections length ({sections.Length}). Setting to max available.");
            activeSectionsCount = sections.Length;
        }
        StartCoroutine(RunSequence());
    }

    public IEnumerator RunSequence()
    {
        while (true)
        {
          
            List<int> sequence = new List<int>();
            for (int i = 0; i < sections.Length; i++)
                sequence.Add(i);

           
            sequence = sequence.OrderBy(x => Random.value).Take(activeSectionsCount).ToList();

           
            foreach (int index in sequence)
            {
                sections[index].SetWarning(true);
            }
            yield return new WaitForSeconds(warningTime);

          
            foreach (int index in sequence)
            {
                sections[index].SetWarning(false);
                sections[index].Activate();
            }
            yield return new WaitForSeconds(activeTime);
        }
    }
}