using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class голова : MonoBehaviour
{
    public Transform playerBody; // Переменная для привязки тела игрока (чтобы вращать его вместе с камерой)
    public float mouseSensitivity = 100f; // Чувствительность мыши
    public float verticalRotationLimit = 80f; // Ограничение угла вертикального вращения

    private float xRotation = 0f; // Текущий угол вертикального вращения

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Блокируем курсор в центре экрана
        Cursor.visible = false; // Прячем курсор
    }

    void Update()
    {
        // Получаем данные о перемещении мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Рассчитываем вертикальное вращение камеры и ограничиваем его
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalRotationLimit, verticalRotationLimit);

        // Применяем вертикальное вращение к камере
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Применяем горизонтальное вращение к телу игрока
        playerBody.Rotate(Vector3.up * mouseX);
        
        //playerBody.Rotate(Vector3.left * mouseY);
    }
}
