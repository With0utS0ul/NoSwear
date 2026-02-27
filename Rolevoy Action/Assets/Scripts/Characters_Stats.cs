using UnityEngine;

public class Characters_Stats : MonoBehaviour
{
    public class PlayerStats
    {
        public float maxHP = 100f;
        [HideInInspector] public float currentHP = 100f;
        public float physicalDamage = 10f;
        public float magicDamage = 25f;
        public string mk_Tag = "MK";  //MainKarkusha
        public string mk_spear_Tag = "MK_spear";  

        public void ResetStats()
        {
            currentHP = maxHP;
        }
    }
    public class Spider 
    {
        public float maxHP = 50f;
        [HideInInspector] public float currentHP = 50f;
        public float physicalDamage = 10f;
        public string spider_Tag = "Spider";

        public void ResetStats()
        {
            currentHP = maxHP;
        }
    }


    public class Maga
    {
        public float maxHP = 150f;
        [HideInInspector] public float currentHP = 150f;
        public float magicDamage = 25f;
        public string maga_Tag = "Maga";

        public void ResetStats()
        {
            currentHP = maxHP;
        }
    }
}
