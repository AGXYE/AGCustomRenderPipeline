using System.Collections;
using System.Collections.Generic;
namespace Custom_RP.Camera
{
    using UnityEngine ;
    using UnityEngine.Rendering ;

    [CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
    public class CustomRenderPipelineAssest : RenderPipelineAsset
    {
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline();
    }
}}



