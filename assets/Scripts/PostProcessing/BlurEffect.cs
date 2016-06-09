using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BlurEffect : MonoBehaviour {

    [Range(1,30)]
    public float kernelAWidth;
    [Range(1, 30)]
    public float kernelBWidth;
    [Range(0, 1)]
    public float stepThreshold;
    [Range(0, 1)]
    public float cellDifference;
    [Range(0.75f, 5)]
    public float cellSharpness;
    [Range(0, 1)]
    public float edgeIntensity;
    private Material material;

    //Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/Blur"));
    }

	//Postprocess the image
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        if(kernelAWidth == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat("_kernelAWidth", kernelAWidth);
        material.SetFloat("_kernelBWidth", kernelBWidth);
        material.SetFloat("_stepThreshold", stepThreshold);
        material.SetFloat("_cellDifference", cellDifference);
        material.SetFloat("_cellSharpness", cellSharpness);
        material.SetFloat("_edgeIntensity", edgeIntensity);
        Graphics.Blit(source, destination, material);
    }
}
