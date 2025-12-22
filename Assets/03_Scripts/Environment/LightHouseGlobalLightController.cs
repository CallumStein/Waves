using UnityEngine;

public class LightHouseGlobalLightController : MonoBehaviour
{
    [SerializeField] private Light m_globalLight;
    [SerializeField] private float m_minIntensity = 0.3f;
    [SerializeField] private float m_maxIntensity = 1.0f;
    [SerializeField] private float m_speed = 1.5f;

    void Update()
    {
        m_globalLight.intensity =
            Mathf.Lerp(m_minIntensity, m_maxIntensity,
            (Mathf.Sin(Time.time * m_speed) + 1f) / 2f);
    }
}
