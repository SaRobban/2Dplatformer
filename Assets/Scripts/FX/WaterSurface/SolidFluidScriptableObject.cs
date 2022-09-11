using UnityEngine;

[CreateAssetMenu(fileName = "FluidFX", menuName = "FX/SpringFluidSettings", order = 1)]
public class SolidFluidScriptableObject : ScriptableObject
{
    [Header("SpriteSettings")]
    public bool getFromParent = true;
    public Material material;
    public int layer;
    public bool destroyOriginalRenderer=true;
    [Header("SpriteMask")]
    public bool createMask = false;
    public Material maskMaterial;
    public int maskLayer;

    [Header("Springs")]
    public int springsPerMeter = 4;

    [Header("Spring")]
    public float tension = 1f;
    public float drag = 1f;

    [Header("WaterSpred")]
    [Range(0, 1)] public float spread = 0.25f;
    public float impactMultiplyer = 0.25f;
    public float smoothValue = 0.5f;

    [Header("Update")]
    public float timeBeforeSleep = 1f;
    public float sleepDelta = 0.1f;
}
