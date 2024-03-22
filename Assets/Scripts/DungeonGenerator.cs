using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private LayerMask _segmentLayer;
    [SerializeField] private List<DungeonSegment> _segments = new List<DungeonSegment>();
    [SerializeField] private DungeonSegment _seedSegment;
    [SerializeField] private DungeonSegment _terminalSegment;
    [SerializeField] private int _maxSegments = 10;

    private List<Transform> _openConnections = new List<Transform>();

    private void Start()
    {
        DungeonSegment root = Instantiate(_seedSegment, transform);
        root.name = "Root";
        foreach (Transform connection in root.Connections)
        {
            _openConnections.Add(connection);
        }
        for (int i = 0; i < _maxSegments; i++)
        {
            GenerateSegment(_openConnections[Random.Range(0, _openConnections.Count)]);
        }
        for (int i = 0; i < _openConnections.Count; i++)
        {
            Instantiate(_terminalSegment, _openConnections[i].position, _openConnections[i].rotation, transform);
        }
    }

    private void GenerateSegment(Transform parentConnection)
    {
        int attempts = 0;
        while (attempts++ < 10)
        {
            DungeonSegment newSegment = Instantiate(_segments[Random.Range(0, _segments.Count)], transform);
            Transform childConnection = newSegment.Connections[Random.Range(0, newSegment.Connections.Count)];

            // Align the new segment with the parent connection
            float angleDifference = Mathf.Round(Vector3.SignedAngle(childConnection.forward, -parentConnection.forward, Vector3.up) / 90) * 90;
            newSegment.transform.rotation *= Quaternion.Euler(0, angleDifference, 0);
            newSegment.transform.position += parentConnection.position - childConnection.position;
            Physics.SyncTransforms();

            bool valid = true;
            foreach (BoxCollider collider in newSegment.Colliders)
            {
                if (Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity, _segmentLayer, QueryTriggerInteraction.Collide).Length > 1)
                {
                    valid = false;
                    Destroy(newSegment.gameObject);
                    break;
                }
            }

            if (valid)
            {
                foreach (Transform newConnection in newSegment.Connections)
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
                return;
            }
        }
    }
}
