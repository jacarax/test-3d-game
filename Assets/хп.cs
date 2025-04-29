using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class хп : MonoBehaviour
{
    public float maxHealth = 100f; // Максимальное здоровье
    public float currentHealth; // Текущее здоровье
    public Image healthBar; // Ссылка на UI элемент HP-бара
    public float damage =50;

    public GameObject projectilePrefab;  // Префаб снаряда
    public float projectileSpeed = 10f;  // Скорость снаряда
    public Transform gunTransform;       // Точка выстрела (например, ствол оружия)


    void Start()
    {
        currentHealth = maxHealth; // Инициализируем текущее здоровье
        UpdateHealthBar(); // Обновляем HP-бар при старте
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Проверка нажатия кнопки стрельбы (например, левая кнопка мыши или Ctrl)
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        // Устанавливаем начальную позицию лазера немного перед игроком
        //Vector3 startPosition = player.position + new Vector3(0, 1, 0);

        // Создаем лазер
        GameObject laser = Instantiate(projectilePrefab, gunTransform.position, Quaternion.identity);

        // Получаем скрипт лазера и инициализируем его
        лазер laserScript = laser.GetComponent<лазер>();
        if (laserScript != null)
        {
            laserScript.Initialize(damage);

            // Устанавливаем направление лазера в сторону цели
            Vector3 direction = (gunTransform.forward ).normalized;
            laserScript.SetDirection(direction);
        }
        //if (projectilePrefab != null && gunTransform != null)
        //{
        //    // Создание снаряда

        //    GameObject projectile = Instantiate(projectilePrefab, gunTransform.position, gunTransform.rotation);

        //    // Получение Rigidbody для применения силы
        //    Rigidbody rb = projectile.GetComponent<Rigidbody>();
        //    if (rb != null)
        //    {
        //        // Запуск снаряда в направлении взгляда игрока
        //        rb.velocity = gunTransform.forward * projectileSpeed;
        //    }
        //}
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Уменьшаем здоровье
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ограничиваем здоровье
        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthBar(); // Обновляем HP-бар
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            
           healthBar.fillAmount = currentHealth / maxHealth; // Обновляем заполненность HP-бара
        }
    }
    private void Die()
    {
        // Логика для уничтожения объекта или проигрывания анимации смерти
        Destroy(gameObject);
    }
}
