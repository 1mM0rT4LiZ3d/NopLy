using System.Collections;
using UnityEngine;

public class EnemyFollowingScript : MonoBehaviour
{
    private bool IsMoveDistance => _distance < 20f;
    private bool IsAttackDistance => _distance < 1.5f;

    [SerializeField] private Animator _animator;
    [SerializeField] private int _damage;
    [SerializeField] public float _speed;
    [SerializeField] private float _attackTime;
    private WaitForSeconds _halfAttackTime;
    private Transform _playerTransform;
    private float _distance;
    private bool _isAttack;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _halfAttackTime = new(_attackTime / 2);
    }

    private void FixedUpdate()
    {
        if (_isAttack) return;

        _distance = Vector3.Distance(transform.position, _playerTransform.position);
        _animator.SetBool("IsMove", IsMoveDistance && !IsAttackDistance);
        _animator.SetBool("IsAttack", IsAttackDistance);

        if (IsAttackDistance)
            StartCoroutine(WaitAttack());
        else if (IsMoveDistance)
        {
            transform.LookAt(_playerTransform.position);
            transform.Translate(Vector3.forward * _speed);
        }
    }

    IEnumerator WaitAttack()
    {
        _isAttack = true;
        yield return _halfAttackTime;

        if (Vector3.Distance(transform.position, _playerTransform.position) < 1.5f)
            _playerTransform.gameObject.GetComponent<PlayerHealthScript>().TakeDamage(_damage);

        yield return _halfAttackTime;
        _isAttack = false;
    }
}
