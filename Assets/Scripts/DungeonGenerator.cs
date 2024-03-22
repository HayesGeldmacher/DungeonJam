using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private LayerMask _segmentLayer;
    [SerializeField] private List<DungeonSegment> _segments = new List<DungeonSegment>();
    [SerializeField] private DungeonSegment _startSegment;
    [SerializeField] private DungeonSegment _goalSegment;
    [SerializeField] private DungeonSegment _terminalSegment;
    [SerializeField] private int _maxSegments = 25;

    private List<Transform> _openConnections = new List<Transform>();
    private List<DungeonSegment> _generatedSegments = new List<DungeonSegment>();

    private void Start()
    {
        GenerateDungeon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateDungeon();
        }
    }

    public void GenerateDungeon()
    {
        // Clear the previous dungeon
        for (int i = 0; i < _generatedSegments.Count; i++)
        {
            foreach (Collider collider in _generatedSegments[i].Colliders)
            {
                collider.enabled = false;
            }
            Destroy(_generatedSegments[i].gameObject);
        }
        _generatedSegments.Clear();
        _openConnections.Clear();

        // Create the first segment
        DungeonSegment root = Instantiate(_startSegment, transform);
        _generatedSegments.Add(root);
        root.name = "Root";
        foreach (Transform connection in root.Connections)
        {
            _openConnections.Add(connection);
        }

        // Generate the dungeon
        for (int i = 0; i < _maxSegments; i++)
        {
            for (int _ = 0; _ < 100; _++)
            {
                Transform childConnection = _openConnections[Random.Range(0, _openConnections.Count)];
                DungeonSegment segment = _segments[Random.Range(0, _segments.Count)];
                int connectionIndex = Random.Range(0, segment.Connections.Count);
                if (TryGenerateSegment(childConnection, segment, connectionIndex, out DungeonSegment generatedSegment))
                {
                    _generatedSegments.Add(generatedSegment);
                    break;
                }
            }
        }

        // Plug the remaining open connections
        for (int i = 0; i < _openConnections.Count; i++)
        {
            Transform parentConnection = _openConnections[i];
            DungeonSegment newSegment = Instantiate(_terminalSegment, transform);
            Transform childConnection = newSegment.Connections[0];

            // Align the new segment with the parent connection
            float angleDifference = Mathf.Round(Vector3.SignedAngle(childConnection.forward, -parentConnection.forward, Vector3.up) / 90) * 90;
            newSegment.transform.rotation *= Quaternion.Euler(0, angleDifference, 0);
            newSegment.transform.position += parentConnection.position - childConnection.position;
            _generatedSegments.Add(newSegment);
        }
    }

    private bool TryGenerateSegment(Transform parentConnection, DungeonSegment segment, int connectionIndex, out DungeonSegment generatedSegment)
    {
        generatedSegment = Instantiate(segment, transform);
        Transform childConnection = generatedSegment.Connections[connectionIndex];

        // Align the new segment with the parent connection
        float angleDifference = Mathf.Round(Vector3.SignedAngle(childConnection.forward, -parentConnection.forward, Vector3.up) / 90) * 90;
        generatedSegment.transform.rotation *= Quaternion.Euler(0, angleDifference, 0);
        generatedSegment.transform.position += parentConnection.position - childConnection.position;
        Physics.SyncTransforms();

        // Check for collisions
        foreach (BoxCollider collider in generatedSegment.Colliders)
        {
            if (Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity, _segmentLayer, QueryTriggerInteraction.Collide).Length > 1)
            {
                Destroy(generatedSegment.gameObject);
                generatedSegment = null;
                return false;
            }
        }

        // Update the open connections list
        foreach (Transform newConnection in generatedSegment.Connections)
        {
            Transform duplicate = null;
            foreach (Transform openConnection in _openConnections)
            {
                if (newConnection.position == openConnection.position)
                {
                    duplicate = openConnection;
                }
            }
            if (duplicate)
            {
                _openConnections.Remove(duplicate);
            }
            else
            {
                _openConnections.Add(newConnection);
            }
        }
        return true;
    }
}
