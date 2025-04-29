using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class лазер : MonoBehaviour
{
    public float damage = 10f; // Урон, который будет наносить лазер
    public float speed = 20f; // Скорость лазера
    private Vector3 direction;
    private float lifetime = 5f; // Время жизни лазера
    
    public delegate void AbilityFunction();
    public AbilityFunctionType selectedFunction;
    
    private лазер abilityFunctions;
    public enum AbilityFunctionType
    {
        HandleCollision,
        HandleCollision2
    }
    
    public void Initialize(float dm)
    {
        damage = dm;
        
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
        Destroy(gameObject, lifetime); // Удаляем лазер через 5 секунд
    }

    private void Update()
    {
        // Перемещаем лазер в заданном направлении
        transform.Translate(direction * speed * Time.deltaTime);
    }
  
    private void OnTriggerEnter(Collider other)
    {
        switch (selectedFunction)
        {
            case AbilityFunctionType.HandleCollision:
                HandleCollision(other.gameObject);
                break;
            case AbilityFunctionType.HandleCollision2:
                HandleCollision2(other.gameObject);
                break;

        }

    }

    private void HandleCollision(GameObject target)
    {
        // Проверяем, если объект, с которым произошел контакт, имеет компонент Health
        хп targetHealth = target.GetComponent<хп>();
        if (targetHealth != null)
        {
            
            
            // Наносим урон объекту
            targetHealth.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
    private void HandleCollision2(GameObject target)
    {
        // Проверяем, если объект, с которым произошел контакт, имеет компонент Health
        моб targetHealth = target.GetComponent<моб>();
        if (targetHealth != null)
        {
            
            // Наносим урон объекту
            targetHealth.TakeDamage(damage);
            Destroy(gameObject);
        }

    }

    //private void HandleCollision(GameObject target)
    //{
    //    // Проверяем, если объект, с которым произошел контакт, имеет компонент Health
    //    хп targetHealth = target.GetComponent<хп>();

    //    if (targetHealth != null)
    //    {
    //        // Наносим урон объекту
    //        targetHealth.TakeDamage(damage);
    //        Destroy(gameObject);
    //    }

    //}



    //public float damage = 10f; // Урон, который будет наносить лазер
    //public float speed = 20f; // Скорость лазера
    //private Vector3 direction;
    //public void SetDirection(Vector3 newDirection)
    //{
    //    direction = newDirection;
    //    Destroy(gameObject, 5f); // Удаляем лазер через 5 секунд
    //}

    //private void Update()
    //{
    //    // Перемещаем лазер в заданном направлении
    //    transform.Translate(direction * speed * Time.deltaTime);
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // Проверяем, если объект, с которым произошел контакт, имеет компонент Health
    //    хп targetHealth = collision.gameObject.GetComponent<хп>();

    //    if (targetHealth != null)
    //    {
    //        // Наносим урон объекту
    //        targetHealth.TakeDamage(damage);
    //    }

    //    // Удаляем лазер из сцены
    //    Destroy(gameObject);
    //}
    //public float damage = 10f; // Урон, который будет наносить лазер
    //public Transform laserOrigin; // Начало лазера
    //public float laserLength = 100f; // Длина лазера
    //public LayerMask targetLayer; // Слой целиы

    //private LineRenderer lineRenderer;

    //private void Start()
    //{
    //    lineRenderer = GetComponent<LineRenderer>();
    //}

    //private void Update()
    //{
    //    // Обновляем направление и длину лазера
    //    RaycastHit hit;
    //    direction = laserOrigin.forward;

    //    if (Physics.Raycast(laserOrigin.position, direction, out hit, laserLength, targetLayer))
    //    {
    //        // Лазер сталкивается с целью
    //        lineRenderer.SetPosition(0, laserOrigin.position);
    //        lineRenderer.SetPosition(1, hit.point);

    //        // Дополнительные действия при попадании
    //        HandleHit(hit);
    //    }
    //    else
    //    {
    //        // Лазер не попадает никуда
    //        lineRenderer.SetPosition(0, laserOrigin.position);
    //        lineRenderer.SetPosition(1, laserOrigin.position + direction * laserLength);
    //    }
    //}

    //private void HandleHit(RaycastHit hit)
    //{
    //    // Обработка попадания лазера
    //    Debug.Log("Hit " + hit.collider.name);
    //}
}
