namespace Custom_RP.RunTime
{
    using Custom_RP.Camera;
    using UnityEngine ;
    using UnityEngine.Rendering ;

    public class CustomRenderPipeline : RenderPipeline
    {
        private CameraRenderer Renderer = new CameraRenderer();
        
        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var cam in cameras)
            {
                Renderer.Render(context,cam);
            }
        }
    }
}
