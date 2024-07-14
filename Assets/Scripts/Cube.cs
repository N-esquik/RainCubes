using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer), typeof(Collider))]

public class Cube : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private Coroutine _coroutine;

    private WaitForSecondsRealtime _wait;

    private int _minTimeLife = 2;
    private int _maxTimeLife = 5;
    private int _timeLife;

    private bool _isDeactivated = false;

    public event Action<Cube> CubeDeactivated;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Platform platform) && _isDeactivated == false)
        {
            _isDeactivated = true;
            _renderer.material.color = UnityEngine.Random.ColorHSV();

            _timeLife = GetRandomTimeLife();

            _wait = new WaitForSecondsRealtime(_timeLife);

            RestartWait();
        }
    }

    private void OnEnable()
    {
        _isDeactivated = false;
        _rigidbody.velocity = Vector3.zero;
        _renderer.material.color = Color.white;
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void RestartWait()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(WaitRelease());
    }

    private IEnumerator WaitRelease()
    {
        yield return _wait;

        CubeDeactivated?.Invoke(this);
    }

    private int GetRandomTimeLife()
    {
        return UnityEngine.Random.Range(_minTimeLife, _maxTimeLife + 1);
    }
}
