using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class игрок : MonoBehaviour
{
    public Transform playerCamera; // Камера или другой объект, который определяет направление взгляда

    [SerializeField] KeyCode key1;
    [SerializeField] KeyCode key2;
    [SerializeField] KeyCode key3;
    [SerializeField] KeyCode key4;
    [SerializeField] KeyCode key5;
    //[SerializeField] Vector3 movement1;
    //[SerializeField] Vector3 movement2;
    //[SerializeField] Vector3 movement3;
    private Rigidbody rb;
    [SerializeField] public float jumpForce;
    [SerializeField] public float move;
    
    public Transform groundCheck;  // Точка для проверки, находится ли персонаж на земле
   
    public LayerMask groundLayer;  // Слой для проверки земли
    private bool isGrounded;  // Флаг для проверки, находится ли персонаж на земле

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Получаем компонент Rigidbody

        // Проверяем, назначен ли groundCheck
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck не назначен в Inspector");
        }
    }
    private void FixedUpdate()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
        }
        if (isGrounded) { 
            if ( Input.GetKey(key5))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            

            }
            Vector3 moveDirection = Vector3.zero; // Направление движения

            // Определяем направление движения в зависимости от нажатых клавиш
            if (Input.GetKey(key1))
            {
                moveDirection += playerCamera.forward; // Направление вперед
            }
            if (Input.GetKey(key2))
            {
                moveDirection -= playerCamera.forward; // Направление назад
            }
            if (Input.GetKey(key3))
            {
                moveDirection -= playerCamera.right; // Направление влево
            }
            if (Input.GetKey(key4))
            {
                moveDirection += playerCamera.right; // Направление вправо
            }
            // Перемещаем персонажа
            if (moveDirection != Vector3.zero)
            {
                moveDirection.y = 0;
                rb.AddForce(moveDirection.normalized * move, ForceMode.Impulse);
            }
        }
        
    }
}
