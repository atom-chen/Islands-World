---@class UnityEngine.SystemInfo
---@field public unsupportedIdentifier System.String
---@field public batteryLevel System.Single
---@field public batteryStatus UnityEngine.BatteryStatus
---@field public operatingSystem System.String
---@field public operatingSystemFamily UnityEngine.OperatingSystemFamily
---@field public processorType System.String
---@field public processorFrequency System.Int32
---@field public processorCount System.Int32
---@field public systemMemorySize System.Int32
---@field public deviceUniqueIdentifier System.String
---@field public deviceName System.String
---@field public deviceModel System.String
---@field public supportsAccelerometer System.Boolean
---@field public supportsGyroscope System.Boolean
---@field public supportsLocationService System.Boolean
---@field public supportsVibration System.Boolean
---@field public supportsAudio System.Boolean
---@field public deviceType UnityEngine.DeviceType
---@field public graphicsMemorySize System.Int32
---@field public graphicsDeviceName System.String
---@field public graphicsDeviceVendor System.String
---@field public graphicsDeviceID System.Int32
---@field public graphicsDeviceVendorID System.Int32
---@field public graphicsDeviceType UnityEngine.Rendering.GraphicsDeviceType
---@field public graphicsUVStartsAtTop System.Boolean
---@field public graphicsDeviceVersion System.String
---@field public graphicsShaderLevel System.Int32
---@field public graphicsMultiThreaded System.Boolean
---@field public hasHiddenSurfaceRemovalOnGPU System.Boolean
---@field public hasDynamicUniformArrayIndexingInFragmentShaders System.Boolean
---@field public supportsShadows System.Boolean
---@field public supportsRawShadowDepthSampling System.Boolean
---@field public supportsMotionVectors System.Boolean
---@field public supports3DTextures System.Boolean
---@field public supports2DArrayTextures System.Boolean
---@field public supports3DRenderTextures System.Boolean
---@field public supportsCubemapArrayTextures System.Boolean
---@field public copyTextureSupport UnityEngine.Rendering.CopyTextureSupport
---@field public supportsComputeShaders System.Boolean
---@field public supportsInstancing System.Boolean
---@field public supportsHardwareQuadTopology System.Boolean
---@field public supports32bitsIndexBuffer System.Boolean
---@field public supportsSparseTextures System.Boolean
---@field public supportedRenderTargetCount System.Int32
---@field public supportsSeparatedRenderTargetsBlend System.Boolean
---@field public supportsMultisampledTextures System.Int32
---@field public supportsMultisampleAutoResolve System.Boolean
---@field public supportsTextureWrapMirrorOnce System.Int32
---@field public usesReversedZBuffer System.Boolean
---@field public npotSupport UnityEngine.NPOTSupport
---@field public maxTextureSize System.Int32
---@field public maxCubemapSize System.Int32
---@field public supportsAsyncCompute System.Boolean
---@field public supportsGraphicsFence System.Boolean
---@field public supportsAsyncGPUReadback System.Boolean
---@field public supportsSetConstantBuffer System.Boolean
---@field public minConstantBufferOffsetAlignment System.Boolean
---@field public hasMipMaxLevel System.Boolean
---@field public supportsMipStreaming System.Boolean
local m = { }
---public SystemInfo .ctor()
---@return SystemInfo
function m.New() end
---public Boolean SupportsRenderTextureFormat(RenderTextureFormat format)
---@return bool
---@param optional RenderTextureFormat format
function m.SupportsRenderTextureFormat(format) end
---public Boolean SupportsBlendingOnRenderTextureFormat(RenderTextureFormat format)
---@return bool
---@param optional RenderTextureFormat format
function m.SupportsBlendingOnRenderTextureFormat(format) end
---public Boolean SupportsTextureFormat(TextureFormat format)
---@return bool
---@param optional TextureFormat format
function m.SupportsTextureFormat(format) end
---public Boolean IsFormatSupported(GraphicsFormat format, FormatUsage usage)
---@return bool
---@param optional GraphicsFormat format
---@param optional FormatUsage usage
function m.IsFormatSupported(format, usage) end
---public GraphicsFormat GetCompatibleFormat(GraphicsFormat format, FormatUsage usage)
---@return number
---@param optional GraphicsFormat format
---@param optional FormatUsage usage
function m.GetCompatibleFormat(format, usage) end
---public GraphicsFormat GetGraphicsFormat(DefaultFormat format)
---@return number
---@param optional DefaultFormat format
function m.GetGraphicsFormat(format) end
UnityEngine.SystemInfo = m
return m
