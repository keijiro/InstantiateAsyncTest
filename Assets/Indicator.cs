using UnityEngine;
using Unity.Mathematics;

public sealed class Indicator : MonoBehaviour
{
    [field:SerializeField] float3 Frequency { get; set; }
    [field:SerializeField] float3 Amplitude { get; set; }

    void Update()
    {
        var v = math.sin(Frequency * Time.time) * Amplitude;
        transform.localPosition = math.float3(v.xy, 0);
        transform.localRotation = quaternion.RotateZ(v.z);
    }
}
