namespace Custom_RP.Camera
{
    using UnityEngine;
    using UnityEngine.Rendering;
    
    public class CameraRenderer
    {
        private ScriptableRenderContext context;

        private Camera mainCamera;

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this.context = context;
            this.mainCamera = camera;
            DrawVisibleGeometry();
            Submit();

        }

        void DrawVisibleGeometry()
        {
            context.DrawSkybox(mainCamera);
        }

        void Submit()
        {
            context.Submit();
        }
    }
}