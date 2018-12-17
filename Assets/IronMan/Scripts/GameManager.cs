using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject ammoPrefab;
    public GameObject blastPrefab;
    public GameObject exhaustPrefab;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
