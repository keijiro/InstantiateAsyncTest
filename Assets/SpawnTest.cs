using UnityEngine;
using System;
using System.Linq;

using Random = UnityEngine.Random;

public sealed class SpawnTest : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;

    [field:SerializeField] public int BatchSize { get; set; } = 100;
    [field:SerializeField] public int BatchCount { get; set; } = 30;
    [field:SerializeField] public float TimeSlice { get; set; } = 2;
    [field:SerializeField] public float Interval { get; set; } = 1;

    int TotalCount => BatchSize * BatchCount;

    async Awaitable WaitInterval()
      => await Awaitable.WaitForSecondsAsync(Interval);

    async void Start()
    {
        AsyncInstantiateOperation.SetIntegrationTimeMS(TimeSlice);

        // Position/rotation array
        var range = Enumerable.Range(0, TotalCount);
        var pos = range.Select(x => Random.insideUnitSphere * 10).ToArray();
        var rot = range.Select(x => Random.rotation).ToArray();

        await WaitInterval();

        // Simple spawn test
        var spawned1 = new GameObject[TotalCount];
        for (var i = 0; i < TotalCount; i++)
            spawned1[i] = Instantiate(_prefab, pos[i], rot[i]);

        await WaitInterval();

        // Cleaning up
        foreach (var o in spawned1) Destroy(o);

        await WaitInterval();

        // Batch spawn test
        var spawned2 = new AsyncInstantiateOperation<GameObject>[BatchCount];
        for (var i = 0; i < BatchCount; i++)
            spawned2[i] = InstantiateAsync(_prefab, BatchSize,
              new Span<Vector3>   (pos, i * BatchSize, BatchSize),
              new Span<Quaternion>(rot, i * BatchSize, BatchSize));

        // Completion
        for (var i = 0; i < BatchCount; i++)
            while (!spawned2[i].isDone) await Awaitable.NextFrameAsync();

        await WaitInterval();

        // Cleaning up
        for (var i = 0; i < BatchCount; i++)
            foreach (var o in spawned2[i].Result) Destroy(o);

        await WaitInterval();

        Application.Quit();
    }
}
