using UnityEngine;

[CreateAssetMenu()]
public class SoundEffectsSO : ScriptableObject
{
    [field: SerializeField] public AudioClip ClickSound { get; private set; }
}
