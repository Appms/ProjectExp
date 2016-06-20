using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class DOFEffect : MonoBehaviour
    {

        public Transform focusObject;
        public float focalDistance = 10;

        [Range(0,2)]
        public float focalSize = 0.05f;
        [Range(0, 1)]
        public float aperture = 0.5f;
        [Range(0.1f, 2)]
        public float maxBlurSize = 0.5f;

        private float internalBlurWidth = 1.0f;

        private Material material;
        private Camera camera;

        //Creates a private material used to the effect
        void Awake()
        {
            //material = new Material(Shader.Find("Hidden/Dof/DepthOfFieldHdr"));
            material = new Material(Shader.Find("Hidden/DOF"));
            camera = GetComponent<Camera>();
        }

        // Performs one blur iteration.
        public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
        {
            float off = 0.5f + iteration * 1f;
            Graphics.BlitMultiTap(source, dest, material,
                                   new Vector2(-off, -off),
                                   new Vector2(-off, off),
                                   new Vector2(off, off),
                                   new Vector2(off, -off)
                );
        }

        // Downsamples the texture to a quarter resolution.
        private void DownSample4x(RenderTexture source, RenderTexture dest)
        {
            float off = 1.0f;
            Graphics.BlitMultiTap(source, dest, material,
                                   new Vector2(-off, -off),
                                   new Vector2(-off, off),
                                   new Vector2(off, off),
                                   new Vector2(off, -off)
                );
        }

        float FocalDistance01(float worldDist)
        {
            return camera.WorldToViewportPoint((worldDist - camera.nearClipPlane) * camera.transform.forward + camera.transform.position).z / (camera.farClipPlane - camera.nearClipPlane);
        }

        // Called by the camera to apply the image effect
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if(focusObject != null)
            {
                Vector3 cameraToObject = focusObject.transform.position - camera.transform.position;
                float angleToObject = Vector3.Angle(cameraToObject, camera.transform.forward);

                float objectDepth = Mathf.Clamp01(Mathf.Cos(Mathf.Deg2Rad * angleToObject) * cameraToObject.magnitude / camera.farClipPlane);

                Debug.Log(objectDepth);

                material.SetFloat("_FocusDepth", objectDepth);
            }

            else material.SetFloat("_FocusDepth", 0);

            int rtW = source.width;
            int rtH = source.height;
            RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);

            // Copy source to the 4x4 smaller texture.
            DownSample4x(source, buffer);

            // Blur the small texture
            for (int i = 0; i < 5; i++)
            {
                RenderTexture buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
                FourTapCone(buffer, buffer2, i);
                RenderTexture.ReleaseTemporary(buffer);
                buffer = buffer2;
            }
            Graphics.Blit(buffer, destination);

            RenderTexture.ReleaseTemporary(buffer);

            /*internalBlurWidth = Mathf.Max(maxBlurSize, 0.0f);

            // focal & coc calculations

            float focalDistance01 = (focusObject) ? (Camera.main.WorldToViewportPoint (focusObject.position)).z / (Camera.main.farClipPlane) : FocalDistance01 (focalDistance);
            material.SetVector("_CurveParams", new Vector4(1.0f, focalSize, (1.0f / (1.0f - aperture) - 1.0f), focalDistance01));

            // possible render texture helpers

            RenderTexture rtLow = null;
            RenderTexture rtLow2 = null;

            source.filterMode = FilterMode.Bilinear;

            source.MarkRestoreExpected(); // only touching alpha channel, RT restore expected
            Graphics.Blit(source, source, material, 0);

            rtLow = RenderTexture.GetTemporary (source.width >> 1, source.height >> 1, 0, source.format);
            rtLow2 = RenderTexture.GetTemporary (source.width >> 1, source.height >> 1, 0, source.format);

            material.SetVector ("_Offsets", new Vector4 (0.0f, internalBlurWidth, 0.1f, internalBlurWidth));

            // blur
            Graphics.Blit (source, rtLow, material, 6);
            Graphics.Blit (rtLow, rtLow2, material, 17);

            // cheaper blur in high resolution, upsample and combine
            material.SetTexture("_LowRez", rtLow2);
            material.SetTexture("_FgOverlap", null);
            material.SetVector ("_Offsets",  Vector4.one * ((1.0f*source.width)/(1.0f*rtLow2.width)) * internalBlurWidth);
            Graphics.Blit (source, destination, material, 18);

            if (rtLow) RenderTexture.ReleaseTemporary(rtLow);
            if (rtLow2) RenderTexture.ReleaseTemporary(rtLow2);*/
            
        }
    }
}
