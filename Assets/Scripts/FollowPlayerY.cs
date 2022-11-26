using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerY : MonoBehaviour {
    private Transform _player;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float minPlayerY;
    [SerializeField] private float maxPlayerY;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private int delay;
    private readonly Queue<float> _positions = new();

    private void Start() {
        startPos = transform.position;
    }

    private void Update() {
        if (_player == null) {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) {
                _player = go.GetComponent<Transform>();
            }
        }

        if (_player == null) {
            return;
        }

        _positions.Enqueue(_player.position.y);

        if (_positions.Count <= delay) {
            return;
        }

        var oldY = _positions.Dequeue() - 1.5f;
        var oldRange = (maxPlayerY - minPlayerY);
        var newRange = (maxY - minY);
        var newY = (((oldY - minPlayerY) * newRange) / oldRange) + minY;

        transform.position = new Vector3(
            startPos.x,
            newY,
            startPos.z);
    }
}