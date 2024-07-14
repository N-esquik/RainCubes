using UnityEngine;
using UnityEngine.Pool;

public class SpawnCubes : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private float _minPosition;
    [SerializeField] private float _maxPosition;
    [SerializeField] private float _positionY;

    private ObjectPool<Cube> _pool;

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
        return new Vector3(Random.Range(_minPosition, _maxPosition),_positionY,Random.Range(_minPosition,_maxPosition));
    }
}
