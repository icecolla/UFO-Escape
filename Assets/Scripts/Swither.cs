using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swither : MonoBehaviour
{
    // Шаблон стен
    private GameObject switchWall;
    // Переменная показывающая статус открытия/закрытия стен
    private bool wallTurndedOff = false;
    // Переменная с компонентом стены
    private Renderer myRenderer;
    // Переменная с компонентом цвета 
    private Color switchColor;

    public AudioSource audios;
    public AudioClip crashSFX;

    void Awake()
    {
        audios = GetComponent<AudioSource>();
        // Для синего переключателя...
        if (gameObject.name == "Building02")
        {
            // Находим синию стену
            switchWall = GameObject.Find("Blue Energy Wall");
            // Запоминаем цвет
            switchColor = Color.blue;
        }
        // Подкючаемся к компоненту MeshRenderer
        myRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (wallTurndedOff)
        {
            // Сделать стену активной в сцене
            switchWall.SetActive(true);
            // Вернуть ей цвет записанный ранее для
            // такого же цвета переключателя (ранее в Awake)
            myRenderer.material.color = switchColor;
            // Статус присвоить ложь (включена)
            wallTurndedOff = false;
        }
        else
        {
            audios.PlayOneShot(crashSFX, 1f);
            // Если стена включена
            // Сделать стену неактивной в сцене
            switchWall.SetActive(false);
            // Сменить цвет переключателя на черный
            myRenderer.material.color = Color.black;
            // Статус присвоить правда (выключена)
            //wallTurndedOff = true;
        }
    }
}
