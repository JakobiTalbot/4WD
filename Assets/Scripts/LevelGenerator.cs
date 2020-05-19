using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_firstTrackPrefab;
    [SerializeField]
    private GameObject[] m_trackPrefabs;
    [SerializeField]
    private float m_trackPrefabLength = 25.25f;
    [SerializeField]
    private int m_levelLength = 10;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        Instantiate(m_firstTrackPrefab, new Vector3(0, -10), Quaternion.identity);

        for (int i = 1; i < m_levelLength; ++i)
        {
            Instantiate(m_trackPrefabs[Random.Range(0, m_trackPrefabs.Length)], new Vector3(m_trackPrefabLength * i, -10), Quaternion.identity);
        }
    }
}