using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed,_maxOffset;

    [SerializeField]
    private float _minSize, _maxSize;

    [SerializeField]
    private SpriteRenderer _sr;

    [SerializeField]
    private BoxCollider2D _col;

    [SerializeField]
    private Transform _scoreTransform;

    private void Start()
    {
        float offset = _maxSize - _minSize;
        float currentSize = _minSize + Random.Range(0, 5) * 0.25f * offset;
        _scoreTransform.localPosition = (currentSize + 1.6f) * Vector3.up;

        _sr.size = new Vector2(_sr.size.x, currentSize);
        _col.size = _sr.size;
        _col.offset = new Vector2(0, currentSize / 2f);
    }

    private void FixedUpdate()
    {
        transform.position += _moveSpeed * Time.fixedDeltaTime * Vector3.left;
        if(transform.position.x < _maxOffset)
        {
            Destroy(gameObject);
        }
    }
}
