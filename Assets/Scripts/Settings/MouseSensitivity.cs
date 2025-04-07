using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    public Slider sensitivitySlider; // ������ �� Slider � UI
    public float DefaultSensitivity = 2.0f; // �������� �� ���������

    void Start()
    {
        // ��������� ���������� �������� ����������������
        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", DefaultSensitivity);

        // ��������� ��������� ��� ��������� ���������������� ��� ����������� ��������
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    void OnDisable()
    {
        // ���������� �������� ���������������� ��� ������
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySlider.value);
    }

    // �����, ���������� ���������������� ����
    public void OnSensitivityChanged(float value)
    {
        // ���������� ������ �������� ����������������
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
    }
}