using UnityEngine;

public sealed class PrintAwakeTime : MonoBehaviour
{
    int _awakeTime;

    void Awake()
      => _awakeTime = Time.frameCount;

    void Start()
      => Debug.Log(_awakeTime);
}
