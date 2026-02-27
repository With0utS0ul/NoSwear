using UnityEngine;
using UnityEngine.UI;

public class Health_Damage_System : MonoBehaviour
{
    
    public Characters_Stats.Spider spider_Stats;
    public Characters_Stats.Maga maga_Stats;
    public Characters_Stats.PlayerStats mk_Stats;

    [Header("UI")]
    [SerializeField] private Transform stats_Canvas;
    [SerializeField] private Slider hp_Slider;

    private Transform cam;
    private int role = 0;
    GameObject character;



    void Awake()
    {
        // Инициализируем объекты, если они не назначены
        if (mk_Stats == null) mk_Stats = new Characters_Stats.PlayerStats();
        if (spider_Stats == null) spider_Stats = new Characters_Stats.Spider();
        if (maga_Stats == null) maga_Stats = new Characters_Stats.Maga();
    }
    void Start()
    {
        character = this.gameObject;
        cam = Camera.main?.transform;

        if (stats_Canvas != null)
        {
            Canvas canvas = stats_Canvas.GetComponent<Canvas>();
            if (canvas != null)
            {
                //  Canvas в режиме World Space
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.worldCamera = Camera.main;

                
            }
        }
        
        hp_Slider.minValue = 0;
        if (character.tag == mk_Stats.mk_Tag)
        {
            role = 1;
            hp_Slider.maxValue = mk_Stats.maxHP;
            hp_Slider.value = mk_Stats.currentHP;
        }
        else if (character.tag == spider_Stats.spider_Tag)
        {
            role = 2;
            hp_Slider.maxValue = spider_Stats.maxHP;
            hp_Slider.value = spider_Stats.currentHP;
        }
        else {
            
            hp_Slider.maxValue = maga_Stats.maxHP;
            hp_Slider.value = maga_Stats.currentHP;
        }
        
    }


    
    private void OnTriggerEnter(Collider collision)
    {

        if (role == 1)
        {
            if (collision.gameObject.CompareTag(spider_Stats.spider_Tag))
            {
                
                mk_Stats.currentHP -= spider_Stats.physicalDamage;
                hp_Slider.value = mk_Stats.currentHP;
            }
            else if (collision.gameObject.CompareTag(maga_Stats.maga_Tag))
            {
                mk_Stats.currentHP -= maga_Stats.magicDamage;
                hp_Slider.value -= maga_Stats.currentHP;
            }
        }

        if (role == 2)
        {
            if (collision.gameObject.CompareTag(mk_Stats.mk_spear_Tag))
            {
                
                spider_Stats.currentHP -= mk_Stats.physicalDamage;
                hp_Slider.value = spider_Stats.currentHP;
                if (hp_Slider.value == 0)
                {
                    character.SetActive(false);
                }
            }
        }

        else
        {
            if (collision.gameObject.CompareTag(mk_Stats.mk_spear_Tag))
            {
                
                maga_Stats.currentHP -= mk_Stats.physicalDamage;
                hp_Slider.value = maga_Stats.currentHP;
                if (hp_Slider.value == 0)
                {
                    character.SetActive(false);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (stats_Canvas == null || cam == null) return;

        //  канвас смотрит на камеру
        stats_Canvas.rotation = Quaternion.LookRotation(cam.position - stats_Canvas.position);

    }
}