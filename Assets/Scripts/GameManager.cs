using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float score = 0;
    public float multiplicator = 1.0f;

    [SerializeField] GameObject[] scoreManager = new GameObject[6];

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateCount();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < scoreManager.Length; i++)
        {
            scoreManager[i].transform.position += new Vector3(0, Mathf.Cos((i + Time.time)) * 0.01f, 0);
        }
        UpdateCount();
    }

    public void AddScore(int points)
    {
        score += points * multiplicator;
    }

    private void UpdateCount()
    {
        string nbr = Mathf.CeilToInt(score).ToString();
        for (int i = 0; i < nbr.Length; i++)
        {
            scoreManager[i].GetComponent<TextMeshProUGUI>().text = nbr[nbr.Length - 1 -i].ToString();
        }
    }
}
