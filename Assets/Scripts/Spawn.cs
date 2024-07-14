using UnityEngine;
using UnityEngine.Pool;

public class Spawn : MonoBehaviour
{
    [SerializeField] private Cube _cube;

    private ObjectPool<Cube> _pool;

    private float _minPositionX = -10f;
    private float _maxPositionX = 18f;
    private float _minPositionZ = -11f;
    private float _maxPositionZ = 17f;
    private float _positionY = 25f;

    private int _countCubeInPool = 5;
    private int _maxSize = 5;

    private float _startTime = 0.1f;
    private float _periodDrop = 0.2f;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>
            (
                createFunc: () => Instantiate(_cube),
                actionOnGet: (cube) => OnGet(cube),
                actionOnRelease: (cube) => OnRelease(cube),
                actionOnDestroy: (cube) => Destroy(cube),
                collectionCheck: true,
                defaultCapacity: _countCubeInPool,
                maxSize: _maxSize
            );
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), _startTime, _periodDrop);
    }

    private void Release(Cube cube)
    {
        _pool.Release(cube);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void OnGet(Cube cube)
    {
        cube.transform.position = GetSpawnPosition();
        cube.gameObject.SetActive(true);
        cube.CubeDeactivated += Release;
    }

    private void OnRelease(Cube cube)
    {
        cube.CubeDeactivated -= Release;
        cube.gameObject.SetActive(false);
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition;
        spawnPosition.x = Random.Range(_minPositionX, _maxPositionX + 1);
        spawnPosition.z = Random.Range(_minPositionZ, _maxPositionZ + 1);
        spawnPosition.y = _positionY;
        return spawnPosition;
    }
}
