using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public int Respawn; 
    public float speed = 2; 
    private GameObject Food; 
    private float segmentSpacing = 1.0f; 
    private List<Transform> _segment; 
    Rigidbody2D rigidbody2;
    [SerializeField] Transform SegmentPrefab; 
    void Start()
    {
        FindNextFood();
        rigidbody2 = GetComponent<Rigidbody2D>();
        _segment = new List<Transform>();
        _segment.Add(this.transform); 
    }

    void Update()
    {
        FindNextFood();
        
        if (Food != null)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, Food.transform.position, speed * Time.deltaTime);
        }
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

        rigidbody2.MovePosition(transform.position);
    }

    private void Grow()
    {
        
        Transform segment = Instantiate(SegmentPrefab);
        segment.position = _segment[_segment.Count - 1].position;
        _segment.Add(segment); 
    }

    void FindNextFood()
    {
        
        if (Food == null)
        {
            Food = GameObject.FindGameObjectWithTag("Food");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "PlayerHead")
        {
            SceneManager.LoadScene(Respawn); 
        }

        
        if (collision.tag == "PlayerBody")
        {
            DestroyEnemyAndSegments(); 
        }

        
        if (collision.tag == "Food")
        {
            Grow(); 
            Destroy(collision.gameObject); 
            Food = null; 
            FindNextFood(); 
        }
    }

    
    private void DestroyEnemyAndSegments()
    {
        
        for (int i = 0; i < _segment.Count; i++)
        {
            Destroy(_segment[i].gameObject); 
        }

        Destroy(this.gameObject);
    }
}


