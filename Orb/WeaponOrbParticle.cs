using UnityEngine;

namespace Swift_Blade
{
    public class WeaponOrbParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ps;
        [SerializeField] private ParticleSystem ps1;
        [SerializeField] private ParticleSystem ps2;

        private ParticleSystem ps_model;

        public void SetWeapon(WeaponSO weapon)
        {
            Color color = ColorUtils.GetColorRGBUnity(weapon.ColorType);

            SetParticleColor(ps, color);
            SetParticleColor(ps1, color);
            SetParticleColor(ps2, color);

            if (ps_model != null)
            {
                Destroy(ps_model.gameObject);
            }
            ps_model = Instantiate(weapon.PreviewMeshParticle, transform);
            ps_model.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            Mesh previewMesh = weapon.PreviewMesh;
            if (previewMesh != null)
            {
                ParticleSystemRenderer renderer = ps_model.GetComponent<ParticleSystemRenderer>();
                Debug.Assert(renderer != null, "fail to get renderer");
                renderer.mesh = previewMesh;
            }
        }
        private void SetParticleColor(ParticleSystem particleSystem, Color color)
        {
            particleSystem.Clear();
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startColor = color;
        }
    }
}
