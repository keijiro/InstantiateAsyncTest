using UnityEngine;
using Unity.Mathematics;

public sealed class Indicator : MonoBehaviour
{
    [field:SerializeField] float Frequency { get; set; } = 1;

    void Update()
    {
        var t = math.frac(Time.time * Frequency);
        var y = 1 - math.pow(t * 2 - 1, 2);
        var r = math.smoothstep(0, 1, t) * 2 * math.PI;
        transform.localPosition = math.float3(0, y, 0);
        transform.localRotation = quaternion.RotateZ(r);
    }
}
