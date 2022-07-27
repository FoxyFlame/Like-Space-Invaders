using UnityEngine;

public class JSONReader : MonoBehaviour
{
    [SerializeField] public TextAsset jsonfile;

    [System.Serializable]
    public class EnemyStats
    {
        public string enemyName;
        public int killingPrice;
        public float missileAttackRate;
        public int killingPriceMin;
        public int killingPriceMax;
    }

    [System.Serializable]
    public class EnemyStatsList
    {
        public EnemyStats[] listOfStatsForEnemy;
    }

    public EnemyStatsList enemyStats = new EnemyStatsList();

    void Start()
    {
        enemyStats = JsonUtility.FromJson<EnemyStatsList>(jsonfile.text);
    }
}
