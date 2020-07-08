using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Tracks")]
    [SerializeField]
    private GameObject m_firstTrackPrefab;
    [SerializeField]
    private GameObject[] m_trackPrefabs;
    [SerializeField]
    private float m_trackPrefabLength = 25.25f;
    [SerializeField]
    private int m_levelLength = 10;
    [Header("Animals")]
    [SerializeField]
    private GameObject m_animalPrefab;
    [SerializeField]
    private int m_animalsCount = 2;
    [Header("Fuel")]
    [SerializeField]
    private GameObject m_fuelPickupPrefab;
    [SerializeField]
    private int m_fuelPickupCount = 2;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        // spawn tracks
        Instantiate(m_firstTrackPrefab, new Vector3(0, -10), Quaternion.identity);
        for (int i = 1; i < m_levelLength; ++i)
        {
            Instantiate(m_trackPrefabs[Random.Range(0, m_trackPrefabs.Length)], new Vector3(m_trackPrefabLength * i, -10), Quaternion.identity);
        }

        // spawn animals
        for (int i = 0; i < m_animalsCount; ++i)
        {
            // get animal X
            float x = m_levelLength * m_trackPrefabLength / m_animalsCount;
            float lowerX = x * i;
            float upperX = x * (i + 1);
            x = Random.Range(lowerX, upperX);

            // raycast for Y
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(x, 50f), Vector3.down);
            Physics.Raycast(ray, out hit, 50f);
            float y = hit.point.y + 1f;

            // spawn animal
            Instantiate(m_animalPrefab, new Vector3(x, y), Quaternion.identity);
        }

        // spawn fuel pickups
        for (int i = 0; i < m_fuelPickupCount; ++i)
        {
            // get fuel pickup X
            float x = m_levelLength * m_trackPrefabLength / m_fuelPickupCount;
            float lowerX = x * i;
            float upperX = x * (i + 1);
            x = Random.Range(lowerX, upperX);

            // raycast for Y
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(x, 50f), Vector3.down);
            Physics.Raycast(ray, out hit, 50f);
            float y = hit.point.y + 1f;

            // spawn fuel pickup
            Instantiate(m_fuelPickupPrefab, new Vector3(x, y), Quaternion.identity);
        }
    }
}