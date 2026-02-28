using UnityEngine;
using UnityEngine.UI;

public class Health_Damage_System : MonoBehaviour
{
    public Characters_Stats.Spider spider_Stats;
    public Characters_Stats.Maga maga_Stats;
    public Characters_Stats.PlayerStats mk_Stats;
    public Animator animator;

    [Header("UI")]
    [SerializeField] private Transform stats_Canvas;
    [SerializeField] private Slider hp_Slider;

    [Header("Ссылки на компоненты")]
    [SerializeField] private Movement movement; // 🎯 Ссылка на скрипт движения/анимации

    private Transform cam;
    private int role = 0;
    private GameObject character;

    // 🎯 Событие для обновления UI извне (опционально)
    public System.Action<float, float> OnHealthChanged;

    void Awake()
    {
        if (mk_Stats == null) mk_Stats = new Characters_Stats.PlayerStats();
        if (spider_Stats == null) spider_Stats = new Characters_Stats.Spider();
        if (maga_Stats == null) maga_Stats = new Characters_Stats.Maga();

        // 🎯 Автоматически находим Movement, если не назначен в инспекторе
        if (movement == null)
            movement = GetComponent<Movement>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        character = this.gameObject;
        cam = Camera.main?.transform;

        if (stats_Canvas != null)
        {
            Canvas canvas = stats_Canvas.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.worldCamera = Camera.main;
            }
        }

        hp_Slider.minValue = 0;

        // 🎯 Инициализация HP и роли
        if (character.CompareTag(mk_Stats.mk_Tag))
        {
            role = 1;
            hp_Slider.maxValue = mk_Stats.maxHP;
            hp_Slider.value = mk_Stats.currentHP;
        }
        else if (character.CompareTag(spider_Stats.spider_Tag))
        {
            role = 2;
            hp_Slider.maxValue = spider_Stats.maxHP;
            hp_Slider.value = spider_Stats.currentHP;
        }
        else
        {
            role = 3;
            hp_Slider.maxValue = maga_Stats.maxHP;
            hp_Slider.value = maga_Stats.currentHP;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        float damage = 0f;
        string attackerTag = collision.gameObject.tag;

        // Определяем урон в зависимости от роли и атакующего
        if (role == 1) // Игрок получает урон
        {
            if (attackerTag == spider_Stats.spider_Tag)
            {
                damage = spider_Stats.physicalDamage;
                hp_Slider.value-=damage;
            }

            else if (attackerTag == maga_Stats.maga_Tag)
            { 
                damage = maga_Stats.magicDamage;
                hp_Slider.value -= damage;
            }

            if (damage > 0)
            {
                movement.TakeDamage(damage);
                
            }
        }
        else if (role == 2) // Паук получает урон
        {
            if (attackerTag == mk_Stats.mk_spear_Tag)
            {
                spider_Stats.currentHP -= mk_Stats.physicalDamage;
                hp_Slider.value = spider_Stats.currentHP;
                if (hp_Slider.value == 0)
                {
                    animator.SetTrigger("isDead");
                    character.SetActive(false);
                }


            }
            else if (attackerTag == mk_Stats.mk_magic_Tag)
            {
                spider_Stats.currentHP -= mk_Stats.magicDamage;
                hp_Slider.value = spider_Stats.currentHP;
                if (hp_Slider.value == 0)
                {
                    animator.SetTrigger("isDead");
                    character.SetActive(false);
                }


            }
        }
        else if (role == 3) // Маг получает урон
        {
            if (attackerTag == mk_Stats.mk_spear_Tag)
            {
                animator.SetTrigger("6");
                maga_Stats.currentHP -= mk_Stats.physicalDamage;
                hp_Slider.value = maga_Stats.currentHP;
                if (hp_Slider.value == 0)
                {
                    animator.SetTrigger("7");
                    character.SetActive(false);
                }
                

            }
            else if (attackerTag == mk_Stats.mk_magic_Tag) 
            {
                animator.SetTrigger("6");
                maga_Stats.currentHP -= mk_Stats.magicDamage;
                hp_Slider.value = maga_Stats.currentHP;
                if (hp_Slider.value == 0)
                {
                    animator.SetTrigger("7");
                    character.SetActive(false);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (stats_Canvas == null || cam == null) return;
        stats_Canvas.rotation = Quaternion.LookRotation(cam.position - stats_Canvas.position);
    }
}