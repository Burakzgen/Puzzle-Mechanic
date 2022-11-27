using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] float ballSpeed = 10f;
    [SerializeField] float rayDistance = 0.6f;
    [SerializeField] LayerMask layer;
    [SerializeField] Color overlappingColor;

    private Rigidbody _rb;
    private int _totalTilesInScene;
    private Vector2 _input;
    private Vector3 _direction = new Vector3(-1, 0, 0);
    private bool _canMove;
    private int _movementCount;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        foreach (var tile in GameObject.FindGameObjectsWithTag("Platform"))
        {
            _totalTilesInScene++;
        }
    }
    private void Update()
    {
        // Get Player Input
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");

        _canMove = Physics.Raycast(transform.position, _direction, rayDistance, layer);

        if (Input.GetButtonDown("Horizontal") && _input.y == 0 && _canMove)
        {
            _direction = Vector3.right * _input.x;
            _movementCount++;
        }
        if (Input.GetButtonDown("Vertical") && _input.x == 0 && _canMove)
        {
            _direction = Vector3.forward * _input.y;
            _movementCount++;
        }
    }
    private void FixedUpdate()
    {
        // Player Movement
        _rb.MovePosition(_rb.position + ballSpeed * Time.fixedDeltaTime * _direction);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Subtract number of tiles, Disable trigger and change color
        if (other.CompareTag("Platform"))
        {
            _totalTilesInScene--;
            other.GetComponent<BoxCollider>().enabled = false;
            other.GetComponent<MeshRenderer>().material.color = overlappingColor;

            if (_totalTilesInScene <= 0)
            {
                Debug.Log("Winner");
            }
        }

    }
}
