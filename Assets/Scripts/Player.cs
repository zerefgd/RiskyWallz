using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveTime, _maxOffsetY;

    private float currentMovePosition;
    private float moveSpeed;
    private Vector3 startPos, endPos;

    private bool canMove;
    private bool canShoot;

    [SerializeField]
    private AudioClip _moveClip, _pointClip, _scoreClip, _loseClip;

    [SerializeField]
    private GameObject _explosionPrefab;
    private void Awake()
    {
        currentMovePosition = 0.5f;
        canShoot = false;
        canMove = false;
        moveSpeed = 1 / _moveTime;

        Vector3 temp = transform.position;
        temp.y = -_maxOffsetY;
        startPos = temp;
        temp.y = _maxOffsetY;
        endPos = temp;
    }

    private void OnEnable()
    {
        GameManager.Instance.GameStarted += GameStarted;
    }

    private void OnDisable()
    {
        GameManager.Instance.GameStarted -= GameStarted;
    }

    private void GameStarted()
    {
        canMove = true;
        canShoot = true;
    }

    private void Update()
    {
        if (canShoot && Input.GetMouseButtonDown(0))
        {
            moveSpeed *= -1f;
            AudioManager.Instance.PlaySound(_moveClip);
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        currentMovePosition += moveSpeed * Time.fixedDeltaTime;


        if (currentMovePosition < 0f || currentMovePosition > 1f)
        {
            moveSpeed *= -1f;
        }

        transform.position = startPos + currentMovePosition * (endPos - startPos);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.Tags.SCORE))
        {
            GameManager.Instance.UpdateScore();
            AudioManager.Instance.PlaySound(_scoreClip);
            Destroy(collision.gameObject);
        }

        if(collision.CompareTag(Constants.Tags.OBSTACLE))
        {
            Destroy(Instantiate(_explosionPrefab,transform.position,Quaternion.identity), 3f);
            AudioManager.Instance.PlaySound(_loseClip);
            GameManager.Instance.EndGame();
            Destroy(gameObject);
        }
    }
}