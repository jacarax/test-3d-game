using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public Transform player; // Ссылка на игрока
    public float fireRate = 1f; // Частота стрельбы
    public float shootRange = 100f; // Дальность стрельбы
    public LayerMask obstacleMask; // Маска слоев для преград, через которые лазер не должен проходить
    public LineRenderer laserLine; // Ссылка на LineRenderer для визуализации лазера
    public float damage = 10f; // Урон, который будет наносить лазер
    private float fireTimer = 0f; // Таймер для контроля частоты стрельбы
    private Animator animator; // Аниматор для запуска анимации атаки

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (laserLine != null)
        {
            laserLine.enabled = false; // Начально выключаем визуализацию
        }
    }

    private void Update()
    {
        // Отслеживаем время для контроля частоты стрельбы
        fireTimer += Time.deltaTime;

        // Вычисляем дистанцию до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Проверяем, в пределах ли игрок находится для стрельбы
        if (distanceToPlayer <= shootRange)
        {
            // Если время стрельбы достигло интервала, стреляем
            if (fireTimer >= 1f / fireRate)
            {
                FireLaser();
                fireTimer = 0f; // Сбрасываем таймер
            }
        }
    }

    private void FireLaser()
    {
        // Запускаем анимацию атаки
        if (animator != null)
        {
            animator.SetTrigger("AttackTrigger");
        }

        // Определяем точку начала и направление лазера
        Vector3 startPosition = transform.position;
        Vector3 direction = (player.position - startPosition).normalized;
        RaycastHit hit;

        // Выполняем Raycast для создания лазера
        if (Physics.Raycast(startPosition, direction, out hit, shootRange, ~obstacleMask))
        {
            if (laserLine != null)
            {
                laserLine.enabled = true;
                laserLine.SetPosition(0, startPosition);
                laserLine.SetPosition(1, hit.point);
            }
            хп targetHealth = player.GetComponent<хп>();
            // Наносим урон объекту
            targetHealth.TakeDamage(damage);

            
            Debug.Log("Hit: " + hit.collider.name);
        }
        else
        {
            if (laserLine != null)
            {
                laserLine.enabled = true;
                laserLine.SetPosition(0, startPosition);
                laserLine.SetPosition(1, startPosition + direction * shootRange);
            }
            Debug.Log("Laser shot missed.");
        }

        // Отключаем LineRenderer через несколько секунд
        if (laserLine != null)
        {
            Invoke("HideLaser", 0.1f);
        }
    }

    private void HideLaser()
    {
        if (laserLine != null)
        {
            laserLine.enabled = false;
        }
    }
    
}