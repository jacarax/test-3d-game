using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class моб : MonoBehaviour
{
    public Transform player;
    
    public float moveSpeed = 2f;
    public LayerMask obstacleLayer;
    private Rigidbody rb;

    public float patrolChangeInterval = 5f; // Интервал изменения цели патрулирования
    private Vector3 startPosition;
    private bool isChasingPlayer = false;
    private Vector3 currentPatrolTarget;
    private float patrolTimer;
    public float patrolRadius = 10f; // Радиус патрулирования
    public float detectionRadius = 10f; // Радиус обнаружения препятствий
  
    public float fireRate = 1f; // Частота стрельбы (раз в секунду)
    public float shootRange = 10f; // Дистанция, на которой NPC начинает стрелять
    private float fireTimer;
    private NPCAbility abilityManager ;
    public float Damage = 10f; // Урон 

    public float maxHealth = 100f; // Максимальное здоровье
    public float currentHealth; // Текущее здоровье
    public delegate void CloneDestroyedHandler();
    public event CloneDestroyedHandler OnCloneDestroyed;
    public Animator animator;

    public LayerMask obstacleMask; // Маска слоев для преград, через которые лазер не должен проходить
    public LineRenderer laserLine; // Ссылка на LineRenderer для визуализации лазера
    
    
    public void Initialize(float xp, float dm)
    {
        maxHealth = xp;
        Damage = dm;

    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Уменьшаем здоровье
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ограничиваем здоровье
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Логика для уничтожения объекта или проигрывания анимации смерти
        Destroy(gameObject);
        OnCloneDestroyed?.Invoke();
    }
    private void Start()
    {
        
        if (laserLine != null)
        {
            laserLine.enabled = false; // Начально выключаем визуализацию
        }
        if (OnCloneDestroyed==null)
            abilityManager = GetComponent<NPCAbility>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        SetNewPatrolTarget();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth; // Инициализируем текущее здоровье

    }

    private void FixedUpdate()
    {
        fly();
    }
    private void fly() 
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isChasingPlayer = true;
        }

        if (isChasingPlayer)
        {
            MoveTowardsPlayer();
            gan();
        }
        else
        {
            // Патрулируем в пределах радиуса
            Patrol();
        }
        

    }
    private void MoveTowardsPlayer()
    {
        Vector3 targetPosition = player.position + new Vector3(0, shootRange / 4, 0);
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Проверка горизонтального расстояния
        float horizontalDistance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z));
        if (horizontalDistance < shootRange - 1)
        {
            direction = -direction; // Двигаемся в обратном направлении
            direction.y = -direction.y;
        }

        Vector3 moveVelocity = new Vector3(direction.x * moveSpeed, direction.y * moveSpeed, direction.z * moveSpeed);
        rb.velocity = moveVelocity;
        // Опционально: Поворот к цели
        transform.LookAt(targetPosition);
    }
    



    private void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    private void SetNewPatrolTarget()
    {
        // Генерируем случайную позицию в пределах увеличенного радиуса
        Vector3 randomPosition = startPosition + Random.insideUnitSphere * patrolRadius;
        randomPosition.y = startPosition.y; // Устанавливаем высоту на уровне начальной точки
        currentPatrolTarget = randomPosition;
    }
    private void Patrol()
    {
        // Двигаемся к текущей цели патрулирования
        MoveTowards(currentPatrolTarget);

        // Если достигли цели, устанавливаем новую цель через определенный интервал
        if (Vector3.Distance(transform.position, currentPatrolTarget) < 0.1f)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolChangeInterval)
            {
                SetNewPatrolTarget();
                patrolTimer = 0f;
            }
        }
        Vector3 direction = (currentPatrolTarget - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime; // Умножаем на Time.deltaTime

    }


    private void gan()
    {
        // Отслеживаем время для контроля частоты стрельбы
        fireTimer += Time.deltaTime;

        // Вычисляем дистанцию до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Проверяем, в пределах ли игрок находится для стрельбы
        if (distanceToPlayer <= shootRange)
        {
            if (abilityManager != null)
            {
                abilityManager.UseAbility();
            }
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
            targetHealth.TakeDamage(Damage);


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
