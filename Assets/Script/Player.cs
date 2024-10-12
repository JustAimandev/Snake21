using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int Respawn; 
    Vector3 mousePosition;
    private Transform _transform;
    public float moveSpeed = 0.1f;
    public float segmentSpacing = 1.0f;
    Rigidbody2D rigidbody2;
    Vector2 position = new Vector2(0, 0);
    private List<Transform> _segment;
    public Transform SegmentPrefab;

    private void Start()
    {
        _transform = this.transform;
        rigidbody2 = GetComponent<Rigidbody2D>();
        _segment = new List<Transform>();
        _segment.Add(this.transform); 
    }

    // Update is called once per frame
    void Update()
    {
        SnakeFollowMouse();
        SnakeFollowPosition();
    }

    private void FixedUpdate()
    {
        for (int i = _segment.Count - 1; i > 0; i--)
        {
            float distance = Vector2.Distance(_segment[i].position, _segment[i - 1].position);

            if (distance > segmentSpacing)
            {
                Vector3 newPosition = Vector2.MoveTowards(_segment[i].position, _segment[i - 1].position, distance - segmentSpacing);
                _segment[i].position = newPosition;
            }
        }

        rigidbody2.MovePosition(position);
    }

    private void SnakeFollowPosition()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _transform.rotation = rotation;
    }

    public void SnakeFollowMouse()
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
    }

    private void Grow()
    {
        Transform segment = Instantiate(SegmentPrefab);
        segment.position = _segment[_segment.Count - 1].position;
        _segment.Add(segment);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Grow(); 
            Destroy(collision.gameObject); 
        }

        
        if (collision.tag == "EnemyBody")
        {
            
            SceneManager.LoadScene(Respawn); 
        }
    }
}


