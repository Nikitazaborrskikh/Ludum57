using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SequenceController : MonoBehaviour
{
    public SectionController[] sections; // Массив секций, который вы заполните в инспекторе
    public float warningTime = 1f;       // Время предупреждения
    public float activeTime = 1f;        // Время активности

    void Start()
    {
        // Проверяем, что все секции назначены
        if (sections == null || sections.Length == 0)
        {
            Debug.LogError("Sections array is empty! Please assign sections in the Inspector.");
            return;
        }
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        while (true)
        {
            // Случайная последовательность
            List<int> sequence = new List<int>();
            for (int i = 0; i < sections.Length; i++)
                sequence.Add(i);
            sequence = sequence.OrderBy(x => Random.value).ToList();

            foreach (int index in sequence)
            {
                // Предупреждение
                sections[index].SetWarning(true);
                yield return new WaitForSeconds(warningTime);
                
                // Активация
                sections[index].SetWarning(false);
                sections[index].Activate();
                yield return new WaitForSeconds(activeTime);
            }
        }
    }
}
