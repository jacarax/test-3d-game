using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ����� : MonoBehaviour
{
    public Transform playerCamera; // ������ ��� ������ ������, ������� ���������� ����������� �������

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
    
    public Transform groundCheck;  // ����� ��� ��������, ��������� �� �������� �� �����
   
    public LayerMask groundLayer;  // ���� ��� �������� �����
    private bool isGrounded;  // ���� ��� ��������, ��������� �� �������� �� �����

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();  // �������� ��������� Rigidbody

        // ���������, �������� �� groundCheck
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck �� �������� � Inspector");
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
            Vector3 moveDirection = Vector3.zero; // ����������� ��������

            // ���������� ����������� �������� � ����������� �� ������� ������
            if (Input.GetKey(key1))
            {
                moveDirection += playerCamera.forward; // ����������� ������
            }
            if (Input.GetKey(key2))
            {
                moveDirection -= playerCamera.forward; // ����������� �����
            }
            if (Input.GetKey(key3))
            {
                moveDirection -= playerCamera.right; // ����������� �����
            }
            if (Input.GetKey(key4))
            {
                moveDirection += playerCamera.right; // ����������� ������
            }
            // ���������� ���������
            if (moveDirection != Vector3.zero)
            {
                moveDirection.y = 0;
                rb.AddForce(moveDirection.normalized * move, ForceMode.Impulse);
            }
        }
        
    }
}
