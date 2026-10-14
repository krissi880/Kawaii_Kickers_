using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform startPieceEndPoint;
    [SerializeField] private GameObject[] piecePrefabs;
    [SerializeField] private float spawnAheadDistance = 10f;
    [SerializeField] private float deleteBelowDistance = 50f;

    private Vector3 lastEndPointPosition;
    private List<GameObject> spawnedPieces = new List<GameObject>();

    void Start()
    {
        lastEndPointPosition = startPieceEndPoint.position;
    }

    void Update()
    {
        if (player.position.y >= lastEndPointPosition.y - spawnAheadDistance)
        {
            Debug.Log("ГЕНЕРАЦИЯ ЗАПУСТИЛАСЬ");
            GeneratePiece();
        }

        RemoveOldPieces();
    }

    void GeneratePiece()
    {
        GameObject prefab = piecePrefabs[
            Random.Range(0, piecePrefabs.Length)
        ];

        GameObject newPiece = Instantiate(prefab);

        Transform startPoint = FindPoint(newPiece.transform, "StartPoint");
        Transform endPoint = FindPoint(newPiece.transform, "EndPoint");

        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("У куска не найдены Start Point или End Point!");
            Destroy(newPiece);
            return;
        }

        newPiece.transform.position +=
            lastEndPointPosition - startPoint.position;

        spawnedPieces.Add(newPiece);
        lastEndPointPosition = endPoint.position;
    }

    Transform FindPoint(Transform parent, string pointName)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == pointName)
            {
                return child;
            }
        }

        return null;
    }

    void RemoveOldPieces()
    {
        for (int i = spawnedPieces.Count - 1; i >= 0; i--)
        {
            GameObject piece = spawnedPieces[i];

            if (piece == null)
            {
                spawnedPieces.RemoveAt(i);
                continue;
            }

            Transform endPoint = FindPoint(piece.transform, "EndPoint");

            if (endPoint == null)
            {
                continue;
            }

            if (player.position.y - endPoint.position.y > deleteBelowDistance)
            {
                Debug.Log($"Удаляется кусок: {piece.name}. " +
              $"Игрок Y: {player.position.y}, " +
              $"EndPoint Y: {endPoint.position.y}, " +
              $"Расстояние: {player.position.y - endPoint.position.y}");

                Destroy(piece);
                spawnedPieces.RemoveAt(i);
            }
        }
    }
}