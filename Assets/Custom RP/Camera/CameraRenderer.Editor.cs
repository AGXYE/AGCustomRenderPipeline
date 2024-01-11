namespace Custom_RP.Camera
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Profiling;
    partial class CameraRenderer
    {

#if UNITY_EDITOR
        private static ShaderTagId[] legacyShaderTagIds =
        {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };

        private string sampleName { get; set; }

        private static Material errormaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        partial void DrawUnsupportedShaders()
        {
            var drawingSetting = new DrawingSettings(
                legacyShaderTagIds[0], new SortingSettings(camera)
            )
            {
                overrideMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"))
            };
            for (int i = 1; i < legacyShaderTagIds.Length; i++)
            {
                drawingSetting.SetShaderPassName(i,legacyShaderTagIds[i]);
            }
            var filteringSetting = FilteringSettings.defaultValue;
            context.DrawRenderers(cullingResults,ref drawingSetting,ref filteringSetting);
        }

        partial void DrawGizmos()
        {
            if (Handles.ShouldRenderGizmos())
            {
                context.DrawGizmos(camera,GizmoSubset.PreImageEffects);
                context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
            }
        }

        partial void PrepareForSceneWindow()
        {
            if (camera.cameraType == CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
            }
        }

        partial void PrepareBuffer()
        {
            Profiler.BeginSample("Editor only");
            buffer.name = sampleName=camera.name;
            Profiler.EndSample();
        }
#endif

        private const string SampleName = bufferName;
    }
}