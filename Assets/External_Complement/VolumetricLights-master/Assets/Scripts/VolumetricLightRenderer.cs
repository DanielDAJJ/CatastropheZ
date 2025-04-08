//123
//  Copyright(c) 2016, Michal Skalsky
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification,
//  are permitted provided that the following conditions are met:
//
//  1. Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//
//  2. Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//
//  3. Neither the name of the copyright holder nor the names of its contributors
//     may be used to endorse or promote products derived from this software without
//     specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY
//  EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
//  OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT
//  SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT
//  OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
//  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
//  TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
//  EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using System;

[RequireComponent(typeof(Camera))]
public class VolumetricLightRenderer : MonoBehaviour
{
    public enum VolumtericResolution
    {
        Full,
        Half,
        Quarter
    };

    public static event Action<VolumetricLightRenderer, Matrix4x4> PreRenderEvent;

    private static Mesh _pointLightMesh;
    private static Mesh _spotLightMesh;
    private static Material _lightMaterial;

    private Camera _camera;
    private CommandBuffer _preLightPass;

    private Matrix4x4 _viewProj;
    private Material _blitAddMaterial;
    private Material _bilateralBlurMaterial;

    private RenderTexture _volumeLightTexture;
    private RenderTexture _halfVolumeLightTexture;
    private RenderTexture _quarterVolumeLightTexture;
    private static Texture _defaultSpotCookie;

    private RenderTexture _halfDepthBuffer;
    private RenderTexture _quarterDepthBuffer;
    private VolumtericResolution _currentResolution = VolumtericResolution.Half;
    private Texture2D _ditheringTexture;
    private Texture3D _noiseTexture;

    public VolumtericResolution Resolution = VolumtericResolution.Half;
    public Texture DefaultSpotCookie;

    public CommandBuffer GlobalCommandBuffer { get { return _preLightPass; } }

    /// <summary>
    /// 
    /// </summary>
    public static Material GetLightMaterial()
    {
        return _lightMaterial;
    }

    /// <summary>
    /// 
    /// </summary>
    public static Mesh GetPointLightMesh()
    {
        return _pointLightMesh;
    }

    /// <summary>
    /// 
    /// </summary>
    public static Mesh GetSpotLightMesh()
    {
        return _spotLightMesh;
    }

    /// <summary>
    /// 
    /// </summary>
    public RenderTexture GetVolumeLightBuffer()
    {
        if (Resolution == VolumtericResolution.Quarter)
            return _quarterVolumeLightTexture;
        else if (Resolution == VolumtericResolution.Half)
            return _halfVolumeLightTexture;
        else
            return _volumeLightTexture;
    }

    /// <summary>
    /// 
    /// </summary>
    public RenderTexture GetVolumeLightDepthBuffer()
    {
        if (Resolution == VolumtericResolution.Quarter)
            return _quarterDepthBuffer;
        else if (Resolution == VolumtericResolution.Half)
            return _halfDepthBuffer;
        else
            return null;
    }

    /// <summary>
    /// 
    /// </summary>
    public static Texture GetDefaultSpotCookie()
    {
        return _defaultSpotCookie;
    }

    /// <summary>
    /// Se inicializa en Awake() lo que no depende de la resolución de pantalla.
    /// </summary>
    void Awake()
    {
        _camera = GetComponent<Camera>();
        if (_camera.actualRenderingPath == RenderingPath.Forward)
            _camera.depthTextureMode = DepthTextureMode.Depth;

        _currentResolution = Resolution;

        Shader shader = Shader.Find("Hidden/BlitAdd");
        if (shader == null)
            throw new Exception("Critical Error: \"Hidden/BlitAdd\" shader is missing. Make sure it is included in \"Always Included Shaders\" in ProjectSettings/Graphics.");
        _blitAddMaterial = new Material(shader);

        shader = Shader.Find("Hidden/BilateralBlur");
        if (shader == null)
            throw new Exception("Critical Error: \"Hidden/BilateralBlur\" shader is missing. Make sure it is included in \"Always Included Shaders\" in ProjectSettings/Graphics.");
        _bilateralBlurMaterial = new Material(shader);

        _preLightPass = new CommandBuffer();
        _preLightPass.name = "PreLight";

        // Se quitó la llamada a ChangeResolution() de Awake porque la resolución puede no estar lista.

        if (_pointLightMesh == null)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _pointLightMesh = go.GetComponent<MeshFilter>().sharedMesh;
            Destroy(go);
        }

        if (_spotLightMesh == null)
        {
            _spotLightMesh = CreateSpotLightMesh();
        }

        if (_lightMaterial == null)
        {
            shader = Shader.Find("Sandbox/VolumetricLight");
            if (shader == null)
                throw new Exception("Critical Error: \"Sandbox/VolumetricLight\" shader is missing. Make sure it is included in \"Always Included Shaders\" in ProjectSettings/Graphics.");
            _lightMaterial = new Material(shader);
        }

        if (_defaultSpotCookie == null)
        {
            _defaultSpotCookie = DefaultSpotCookie;
        }

        LoadNoise3dTexture();
        GenerateDitherTexture();
    }

    /// <summary>
    /// Se llama en Start() para que la resolución ya esté definida.
    /// </summary>
    void Start()
    {
        ChangeResolution();
    }

    /// <summary>
    /// Se suscribe el CommandBuffer al cámara.
    /// </summary>
    void OnEnable()
    {
        if (_camera.actualRenderingPath == RenderingPath.Forward)
            _camera.AddCommandBuffer(CameraEvent.AfterDepthTexture, _preLightPass);
        else
            _camera.AddCommandBuffer(CameraEvent.BeforeLighting, _preLightPass);
    }

    /// <summary>
    /// Se quita el CommandBuffer de la cámara.
    /// </summary>
    void OnDisable()
    {
        if (_camera.actualRenderingPath == RenderingPath.Forward)
            _camera.RemoveCommandBuffer(CameraEvent.AfterDepthTexture, _preLightPass);
        else
            _camera.RemoveCommandBuffer(CameraEvent.BeforeLighting, _preLightPass);
    }

    /// <summary>
    /// Cambia la resolución y recrea las RenderTextures según la resolución de la cámara.
    /// Se añade una verificación para que ancho y alto sean válidos.
    /// </summary>
    void ChangeResolution()
    {
        int width = _camera.pixelWidth;
        int height = _camera.pixelHeight;

        if (width <= 0 || height <= 0)
        {
            Debug.LogWarning("VolumetricLightRenderer: Resolución de la cámara inválida (" + width + "x" + height + "). Se salta la creación de RenderTextures.");
            return;
        }

        if (_volumeLightTexture != null)
            Destroy(_volumeLightTexture);

        _volumeLightTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf);
        _volumeLightTexture.name = "VolumeLightBuffer";
        _volumeLightTexture.filterMode = FilterMode.Bilinear;

        if (_halfDepthBuffer != null)
            Destroy(_halfDepthBuffer);
        if (_halfVolumeLightTexture != null)
            Destroy(_halfVolumeLightTexture);

        if (Resolution == VolumtericResolution.Half || Resolution == VolumtericResolution.Quarter)
        {
            _halfVolumeLightTexture = new RenderTexture(width / 2, height / 2, 0, RenderTextureFormat.ARGBHalf);
            _halfVolumeLightTexture.name = "VolumeLightBufferHalf";
            _halfVolumeLightTexture.filterMode = FilterMode.Bilinear;

            _halfDepthBuffer = new RenderTexture(width / 2, height / 2, 0, RenderTextureFormat.RFloat);
            _halfDepthBuffer.name = "VolumeLightHalfDepth";
            _halfDepthBuffer.Create();
            _halfDepthBuffer.filterMode = FilterMode.Point;
        }

        if (_quarterVolumeLightTexture != null)
            Destroy(_quarterVolumeLightTexture);
        if (_quarterDepthBuffer != null)
            Destroy(_quarterDepthBuffer);

        if (Resolution == VolumtericResolution.Quarter)
        {
            _quarterVolumeLightTexture = new RenderTexture(width / 4, height / 4, 0, RenderTextureFormat.ARGBHalf);
            _quarterVolumeLightTexture.name = "VolumeLightBufferQuarter";
            _quarterVolumeLightTexture.filterMode = FilterMode.Bilinear;

            _quarterDepthBuffer = new RenderTexture(width / 4, height / 4, 0, RenderTextureFormat.RFloat);
            _quarterDepthBuffer.name = "VolumeLightQuarterDepth";
            _quarterDepthBuffer.Create();
            _quarterDepthBuffer.filterMode = FilterMode.Point;
        }
    }

    /// <summary>
    /// Prepara el CommandBuffer para renderizar las luces volumétricas.
    /// </summary>
    public void OnPreRender()
    {
        // Se usa un near clip plane muy bajo para simplificar la intersección cono/frustum
        Matrix4x4 proj = Matrix4x4.Perspective(_camera.fieldOfView, _camera.aspect, 0.01f, _camera.farClipPlane);

#if UNITY_2017_2_OR_NEWER
        if (UnityEngine.XR.XRSettings.enabled)
        {
            // En VR se utiliza la matriz de proyección de la cámara actual.
            proj = Camera.current.projectionMatrix;
        }
#endif

        proj = GL.GetGPUProjectionMatrix(proj, true);
        _viewProj = proj * _camera.worldToCameraMatrix;

        _preLightPass.Clear();

        bool dx11 = SystemInfo.graphicsShaderLevel > 40;

        if (Resolution == VolumtericResolution.Quarter)
        {
            Texture nullTexture = null;
            // Se reduce la resolución del depth a media
            _preLightPass.Blit(nullTexture, _halfDepthBuffer, _bilateralBlurMaterial, dx11 ? 4 : 10);
            // Se reduce la resolución del depth a cuarto
            _preLightPass.Blit(nullTexture, _quarterDepthBuffer, _bilateralBlurMaterial, dx11 ? 6 : 11);

            _preLightPass.SetRenderTarget(_quarterVolumeLightTexture);
        }
        else if (Resolution == VolumtericResolution.Half)
        {
            Texture nullTexture = null;
            // Se reduce la resolución del depth a media
            _preLightPass.Blit(nullTexture, _halfDepthBuffer, _bilateralBlurMaterial, dx11 ? 4 : 10);

            _preLightPass.SetRenderTarget(_halfVolumeLightTexture);
        }
        else
        {
            _preLightPass.SetRenderTarget(_volumeLightTexture);
        }

        _preLightPass.ClearRenderTarget(false, true, new Color(0, 0, 0, 1));

        UpdateMaterialParameters();

        if (PreRenderEvent != null)
            PreRenderEvent(this, _viewProj);
    }

    [ImageEffectOpaque]
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (Resolution == VolumtericResolution.Quarter)
        {
            RenderTexture temp = RenderTexture.GetTemporary(_quarterDepthBuffer.width, _quarterDepthBuffer.height, 0, RenderTextureFormat.ARGBHalf);
            temp.filterMode = FilterMode.Bilinear;

            // Blur bilateral horizontal a cuarto de resolución
            Graphics.Blit(_quarterVolumeLightTexture, temp, _bilateralBlurMaterial, 8);
            // Blur bilateral vertical a cuarto de resolución
            Graphics.Blit(temp, _quarterVolumeLightTexture, _bilateralBlurMaterial, 9);

            // Se escala de vuelta a resolución completa
            Graphics.Blit(_quarterVolumeLightTexture, _volumeLightTexture, _bilateralBlurMaterial, 7);

            RenderTexture.ReleaseTemporary(temp);
        }
        else if (Resolution == VolumtericResolution.Half)
        {
            RenderTexture temp = RenderTexture.GetTemporary(_halfVolumeLightTexture.width, _halfVolumeLightTexture.height, 0, RenderTextureFormat.ARGBHalf);
            temp.filterMode = FilterMode.Bilinear;

            // Blur bilateral horizontal a media resolución
            Graphics.Blit(_halfVolumeLightTexture, temp, _bilateralBlurMaterial, 2);

            // Blur bilateral vertical a media resolución
            Graphics.Blit(temp, _halfVolumeLightTexture, _bilateralBlurMaterial, 3);

            // Se escala a resolución completa
            Graphics.Blit(_halfVolumeLightTexture, _volumeLightTexture, _bilateralBlurMaterial, 5);
            RenderTexture.ReleaseTemporary(temp);
        }
        else
        {
            RenderTexture temp = RenderTexture.GetTemporary(_volumeLightTexture.width, _volumeLightTexture.height, 0, RenderTextureFormat.ARGBHalf);
            temp.filterMode = FilterMode.Bilinear;

            // Blur bilateral horizontal a resolución completa
            Graphics.Blit(_volumeLightTexture, temp, _bilateralBlurMaterial, 0);
            // Blur bilateral vertical a resolución completa
            Graphics.Blit(temp, _volumeLightTexture, _bilateralBlurMaterial, 1);
            RenderTexture.ReleaseTemporary(temp);
        }

        // Se añade el buffer de luz volumétrica a la escena renderizada.
        _blitAddMaterial.SetTexture("_Source", source);
        Graphics.Blit(_volumeLightTexture, destination, _blitAddMaterial, 0);
    }

    private void UpdateMaterialParameters()
    {
        _bilateralBlurMaterial.SetTexture("_HalfResDepthBuffer", _halfDepthBuffer);
        _bilateralBlurMaterial.SetTexture("_HalfResColor", _halfVolumeLightTexture);
        _bilateralBlurMaterial.SetTexture("_QuarterResDepthBuffer", _quarterDepthBuffer);
        _bilateralBlurMaterial.SetTexture("_QuarterResColor", _quarterVolumeLightTexture);

        Shader.SetGlobalTexture("_DitherTexture", _ditheringTexture);
        Shader.SetGlobalTexture("_NoiseTexture", _noiseTexture);
    }

    /// <summary>
    /// Se comprueba si se ha modificado la resolución o si la resolución actual difiere de la de la cámara.
    /// </summary>
    void Update()
    {
        if (_currentResolution != Resolution)
        {
            _currentResolution = Resolution;
            ChangeResolution();
        }

        if ((_volumeLightTexture.width != _camera.pixelWidth || _volumeLightTexture.height != _camera.pixelHeight))
            ChangeResolution();
    }

    /// <summary>
    /// Carga la textura de ruido 3D.
    /// </summary>
    void LoadNoise3dTexture()
    {
        // Cargador básico de DDS para textura 3D - ¡no es muy robusto!
        TextAsset data = Resources.Load("NoiseVolume") as TextAsset;
        byte[] bytes = data.bytes;

        uint height = BitConverter.ToUInt32(data.bytes, 12);
        uint width = BitConverter.ToUInt32(data.bytes, 16);
        uint pitch = BitConverter.ToUInt32(data.bytes, 20);
        uint depth = BitConverter.ToUInt32(data.bytes, 24);
        uint formatFlags = BitConverter.ToUInt32(data.bytes, 20 * 4);
        uint bitdepth = BitConverter.ToUInt32(data.bytes, 22 * 4);
        if (bitdepth == 0)
            bitdepth = pitch / width * 8;

        // No funciona con TextureFormat.Alpha8 por alguna razón
        _noiseTexture = new Texture3D((int)width, (int)height, (int)depth, TextureFormat.RGBA32, false);
        _noiseTexture.name = "3D Noise";

        Color[] c = new Color[width * height * depth];

        uint index = 128;
        if (data.bytes[21 * 4] == 'D' && data.bytes[21 * 4 + 1] == 'X' && data.bytes[21 * 4 + 2] == '1' && data.bytes[21 * 4 + 3] == '0' &&
            (formatFlags & 0x4) != 0)
        {
            uint format = BitConverter.ToUInt32(data.bytes, (int)index);
            if (format >= 60 && format <= 65)
                bitdepth = 8;
            else if (format >= 48 && format <= 52)
                bitdepth = 16;
            else if (format >= 27 && format <= 32)
                bitdepth = 32;
            index += 20;
        }

        uint byteDepth = bitdepth / 8;
        pitch = (width * bitdepth + 7) / 8;

        for (int d = 0; d < depth; ++d)
        {
            for (int h = 0; h < height; ++h)
            {
                for (int w = 0; w < width; ++w)
                {
                    float v = (bytes[index + w * byteDepth] / 255.0f);
                    c[w + h * width + d * width * height] = new Color(v, v, v, v);
                }
                index += pitch;
            }
        }

        _noiseTexture.SetPixels(c);
        _noiseTexture.Apply();
    }

    /// <summary>
    /// Genera la textura de dithering.
    /// </summary>
    private void GenerateDitherTexture()
    {
        if (_ditheringTexture != null)
        {
            return;
        }

        int size = 8;
#if DITHER_4_4
        size = 4;
#endif
        _ditheringTexture = new Texture2D(size, size, TextureFormat.Alpha8, false, true);
        _ditheringTexture.filterMode = FilterMode.Point;
        Color32[] c = new Color32[size * size];

        byte b;
#if DITHER_4_4
        b = (byte)(0.0f / 16.0f * 255); c[0] = new Color32(b, b, b, b);
        b = (byte)(8.0f / 16.0f * 255); c[1] = new Color32(b, b, b, b);
        b = (byte)(2.0f / 16.0f * 255); c[2] = new Color32(b, b, b, b);
        b = (byte)(10.0f / 16.0f * 255); c[3] = new Color32(b, b, b, b);
        b = (byte)(12.0f / 16.0f * 255); c[4] = new Color32(b, b, b, b);
        b = (byte)(4.0f / 16.0f * 255); c[5] = new Color32(b, b, b, b);
        b = (byte)(14.0f / 16.0f * 255); c[6] = new Color32(b, b, b, b);
        b = (byte)(6.0f / 16.0f * 255); c[7] = new Color32(b, b, b, b);
        b = (byte)(3.0f / 16.0f * 255); c[8] = new Color32(b, b, b, b);
        b = (byte)(11.0f / 16.0f * 255); c[9] = new Color32(b, b, b, b);
        b = (byte)(1.0f / 16.0f * 255); c[10] = new Color32(b, b, b, b);
        b = (byte)(9.0f / 16.0f * 255); c[11] = new Color32(b, b, b, b);
        b = (byte)(15.0f / 16.0f * 255); c[12] = new Color32(b, b, b, b);
        b = (byte)(7.0f / 16.0f * 255); c[13] = new Color32(b, b, b, b);
        b = (byte)(13.0f / 16.0f * 255); c[14] = new Color32(b, b, b, b);
        b = (byte)(5.0f / 16.0f * 255); c[15] = new Color32(b, b, b, b);
#else
        int i = 0;
        b = (byte)(1.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(49.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(13.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(61.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(4.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(52.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(16.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(64.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(33.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(17.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(45.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(29.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(36.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(20.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(48.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(32.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(9.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(57.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(5.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(53.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(12.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(60.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(8.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(56.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(41.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(25.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(37.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(21.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(44.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(28.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(40.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(24.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(3.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(51.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(15.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(63.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(2.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(50.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(14.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(62.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(35.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(19.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(47.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(31.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(34.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(18.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(46.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(30.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(11.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(59.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(7.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(55.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(10.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(58.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(6.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(54.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(43.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(27.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(39.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(23.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(42.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(26.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(38.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
        b = (byte)(22.0f / 65.0f * 255); c[i++] = new Color32(b, b, b, b);
#endif

        _ditheringTexture.SetPixels32(c);
        _ditheringTexture.Apply();
    }

    /// <summary>
    /// Crea la malla para la luz puntual.
    /// </summary>
    private Mesh CreateSpotLightMesh()
    {
        // Geometría compleja copiada de otro proyecto, se recomienda simplificarla.
        Mesh mesh = new Mesh();

        const int segmentCount = 16;
        Vector3[] vertices = new Vector3[2 + segmentCount * 3];
        Color32[] colors = new Color32[2 + segmentCount * 3];

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 0, 1);

        float angle = 0;
        float step = Mathf.PI * 2.0f / segmentCount;
        float ratio = 0.9f;

        for (int i = 0; i < segmentCount; ++i)
        {
            vertices[i + 2] = new Vector3(-Mathf.Cos(angle) * ratio, Mathf.Sin(angle) * ratio, ratio);
            colors[i + 2] = new Color32(255, 255, 255, 255);
            vertices[i + 2 + segmentCount] = new Vector3(-Mathf.Cos(angle), Mathf.Sin(angle), 1);
            colors[i + 2 + segmentCount] = new Color32(255, 255, 255, 0);
            vertices[i + 2 + segmentCount * 2] = new Vector3(-Mathf.Cos(angle) * ratio, Mathf.Sin(angle) * ratio, 1);
            colors[i + 2 + segmentCount * 2] = new Color32(255, 255, 255, 255);
            angle += step;
        }

        mesh.vertices = vertices;
        mesh.colors32 = colors;

        int[] indices = new int[segmentCount * 3 * 2 + segmentCount * 6 * 2];
        int index = 0;

        for (int i = 2; i < segmentCount + 1; ++i)
        {
            indices[index++] = 0;
            indices[index++] = i;
            indices[index++] = i + 1;
        }

        indices[index++] = 0;
        indices[index++] = segmentCount + 1;
        indices[index++] = 2;

        for (int i = 2; i < segmentCount + 1; ++i)
        {
            indices[index++] = i;
            indices[index++] = i + segmentCount;
            indices[index++] = i + 1;

            indices[index++] = i + 1;
            indices[index++] = i + segmentCount;
            indices[index++] = i + segmentCount + 1;
        }

        indices[index++] = 2;
        indices[index++] = 1 + segmentCount;
        indices[index++] = 2 + segmentCount;

        indices[index++] = 2 + segmentCount;
        indices[index++] = 1 + segmentCount;
        indices[index++] = 1 + segmentCount + segmentCount;

        for (int i = 2 + segmentCount; i < segmentCount + 1 + segmentCount; ++i)
        {
            indices[index++] = i;
            indices[index++] = i + segmentCount;
            indices[index++] = i + 1;

            indices[index++] = i + 1;
            indices[index++] = i + segmentCount;
            indices[index++] = i + segmentCount + 1;
        }

        indices[index++] = 2 + segmentCount;
        indices[index++] = 1 + segmentCount * 2;
        indices[index++] = 2 + segmentCount * 2;

        indices[index++] = 2 + segmentCount * 2;
        indices[index++] = 1 + segmentCount * 2;
        indices[index++] = 1 + segmentCount * 3;

        for (int i = 2 + segmentCount * 2; i < segmentCount * 3 + 1; ++i)
        {
            indices[index++] = 1;
            indices[index++] = i + 1;
            indices[index++] = i;
        }

        indices[index++] = 1;
        indices[index++] = 2 + segmentCount * 2;
        indices[index++] = segmentCount * 3 + 1;

        mesh.triangles = indices;
        mesh.RecalculateBounds();

        return mesh;
    }
}