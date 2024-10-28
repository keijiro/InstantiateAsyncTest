using UnityEngine;

public sealed class SpawnTest : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;

    [field:SerializeField] public int SpawnCount { get; set; } = 100;
    [field:SerializeField] public float TimeSlice { get; set; } = 30;

    async Awaitable Interval()
    {
        for (var i = 0; i < 10; i++)
            await Awaitable.NextFrameAsync();
    }

    async void Start()
    {
        var spawned = new GameObject[SpawnCount];
        var pos = new Vector3[SpawnCount];
        var rot = new Quaternion[SpawnCount];

        for (var i = 0; i < SpawnCount; i++)
        {
            pos[i] = Random.insideUnitSphere * 5;
            rot[i] = Random.rotation;
        }

        await Interval();

        for (var i = 0; i < SpawnCount; i++)
            spawned[i] = Instantiate(_prefab, pos[i], rot[i]);

        await Interval();

        for (var i = 0; i < SpawnCount; i++) Destroy(spawned[i]);

        await Interval();

        Debug.Log(AsyncInstantiateOperation.GetIntegrationTimeMS());
        AsyncInstantiateOperation.SetIntegrationTimeMS(TimeSlice);

        var spawned2 = Object.InstantiateAsync(_prefab, SpawnCount, pos, rot);

        await spawned2;

        await Interval();

        for (var i = 0; i < SpawnCount; i++) Destroy(spawned2.Result[i]);

        await Interval();

        Application.Quit();
    }
}
