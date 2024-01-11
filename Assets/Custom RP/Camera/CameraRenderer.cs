namespace Custom_RP.Camera
{
    using UnityEngine;
    using UnityEngine.Rendering;

    public partial class CameraRenderer
    {
        //this class declares how a camera is rendered.


        private static ShaderTagId unlitShaderTageID = new ShaderTagId("SRPDefaultUnlit");


        //this context will be given by unity
        private ScriptableRenderContext context;
        //the camera we described here.
        private Camera camera;
        private CullingResults cullingResults;

        private const string bufferName = "AGMainCamera";

        //commandBuffer can set various render settings,like set uniform,create renderTexture.Eventually,the context will draw geometry.
        private CommandBuffer buffer = new CommandBuffer()
        {
            name =bufferName
        };

        //this method is to draw all geometry its camera can see.
        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this.context = context;
            this.camera = camera;
            
            PrepareBuffer();
            PrepareForSceneWindow();
            if(!Cull())
                return;

            Setup();
            DrawVisibleGeometry();
            DrawUnsupportedShaders();
            DrawGizmos();
            EndAndSubmit();
        }

        void Setup()
        {
            //this function setup the basic variables like view,projection and clipping plane
            context.SetupCameraProperties(camera);

            CameraClearFlags flags = camera.clearFlags;
            buffer.ClearRenderTarget(
                flags <= CameraClearFlags.Depth,
                flags == CameraClearFlags.Color,
                flags == CameraClearFlags.Color ?
                    camera.backgroundColor.linear : Color.clear
            );
            buffer.BeginSample(sampleName);
            //before you draw something via context,you should execute the command buffer.
            //buffer.ClearRenderTarget(true,true,Color.clear);
            ExecuteBuffer();
            
        }

        void DrawVisibleGeometry()
        {
            var sortSetting = new SortingSettings(camera);
            var drawingSetting = new DrawingSettings(unlitShaderTageID,sortSetting);
            var filterSetting = new FilteringSettings(RenderQueueRange.opaque);
            
            //draw opaque object from cullingResult
            context.DrawRenderers(cullingResults,ref drawingSetting,ref filterSetting);
            // draw skybox
            context.DrawSkybox(camera);
            //draw transparent object
            sortSetting.criteria = SortingCriteria.CommonTransparent;
            drawingSetting.sortingSettings = sortSetting;
            filterSetting.renderQueueRange = RenderQueueRange.transparent;
            context.DrawRenderers(cullingResults,ref drawingSetting,ref filterSetting);

        }

        void EndAndSubmit()
        {
            buffer.EndSample(sampleName);
            ExecuteBuffer();
            context.Submit();
        }

        void ExecuteBuffer()
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }

        bool Cull()
        {
            if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
            {
                cullingResults = context.Cull(ref p);
                return true;
            }

            return false;
        }

        partial void DrawUnsupportedShaders();
        partial void DrawGizmos();
        partial void PrepareForSceneWindow();
        partial void PrepareBuffer();


    }
}