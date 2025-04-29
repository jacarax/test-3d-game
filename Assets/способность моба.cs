using UnityEngine;

public class NPCAbility : MonoBehaviour
{
    public GameObject cloneTemplate; // Шаблон для клона
    public float abilityCooldown = 10f; // Время перезарядки способности

    public float casttime = 1f; // Время применения способности
    private float casttimeTimer = -1f; // Время применения способности

    public float abilityCooldownTimer = 0f; // Таймер перезарядки способности
    public bool abilityAvailable = true; // Доступность способности

    private int activeClones = 0; // Количество активных клонов

    public float cloneHealth = 50f; // Здоровье клона
    public float cloneDamage = 5f; // Урон клона
    public Animator animator;
    public Vector3 cloneScale = new Vector3(0.5f, 0.5f, 0.5f); // Масштаб для клонов
    
    void Update()
    {
        // Обновление таймера перезарядки
        if (!abilityAvailable)
        {
            abilityCooldownTimer -= Time.deltaTime;
            if (abilityCooldownTimer <= 0f)
            {
                abilityAvailable = true;

            }
        }
    }

    public void UseAbility()
    {
        if (abilityAvailable)
        {
            CreateClones();
        }
    }

    private void CreateClones()
    {
        if (activeClones >= 1) return; // Если уже создано 2 клона, ничего не делать
        
            if(casttimeTimer <= 0f) 
            {
                animator.SetTrigger("AbilityTrigger");
                casttimeTimer = casttime;
            }
            casttimeTimer -= Time.deltaTime;
            if (casttimeTimer <= 0f)
            {
                
                abilityAvailable = false;
                abilityCooldownTimer = abilityCooldown;
                // Позиции для создания клонов
                Vector3 spawnOffset1 = new Vector3(2f, 0, 0); // Смещение для первого клона
                Vector3 spawnOffset2 = new Vector3(-2f, 0, 0); // Смещение для второго клона

                // Создание первого клона
                GameObject clone1 = Instantiate(cloneTemplate, transform.position + spawnOffset1, Quaternion.identity);
                clone1.transform.localScale = cloneScale; // Установка масштаба
                моб clone1Behavior = clone1.GetComponent<моб>();

                if (clone1Behavior != null)
                {
                    clone1Behavior.Initialize(cloneHealth, cloneDamage);
                    clone1Behavior.OnCloneDestroyed += HandleCloneDestroyed;
                    activeClones++;
                }

                // Создание второго клона
                GameObject clone2 = Instantiate(cloneTemplate, transform.position + spawnOffset2, Quaternion.identity);
                clone2.transform.localScale = cloneScale; // Установка масштаба
                моб clone2Behavior = clone2.GetComponent<моб>();
                if (clone2Behavior != null)
                {
                    clone2Behavior.Initialize(cloneHealth, cloneDamage);
                    clone2Behavior.OnCloneDestroyed += HandleCloneDestroyed;
                    activeClones++;
                }
                //break;
            }
        //}
    }

    private void HandleCloneDestroyed()
    {
        activeClones--;
        
    }
}