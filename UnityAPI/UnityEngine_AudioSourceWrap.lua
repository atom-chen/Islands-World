---@class UnityEngine.AudioSource : UnityEngine.AudioBehaviour
---@field public volume System.Single
---@field public pitch System.Single
---@field public time System.Single
---@field public timeSamples System.Int32
---@field public clip UnityEngine.AudioClip
---@field public outputAudioMixerGroup UnityEngine.Audio.AudioMixerGroup
---@field public isPlaying System.Boolean
---@field public isVirtual System.Boolean
---@field public loop System.Boolean
---@field public ignoreListenerVolume System.Boolean
---@field public playOnAwake System.Boolean
---@field public ignoreListenerPause System.Boolean
---@field public velocityUpdateMode UnityEngine.AudioVelocityUpdateMode
---@field public panStereo System.Single
---@field public spatialBlend System.Single
---@field public spatialize System.Boolean
---@field public spatializePostEffects System.Boolean
---@field public reverbZoneMix System.Single
---@field public bypassEffects System.Boolean
---@field public bypassListenerEffects System.Boolean
---@field public bypassReverbZones System.Boolean
---@field public dopplerLevel System.Single
---@field public spread System.Single
---@field public priority System.Int32
---@field public mute System.Boolean
---@field public minDistance System.Single
---@field public maxDistance System.Single
---@field public rolloffMode UnityEngine.AudioRolloffMode
local m = { }
---public AudioSource .ctor()
---@return AudioSource
function m.New() end
---public Void Play()
---public Void Play(UInt64 delay)
---@param UInt64 delay
function m:Play(delay) end
---public Void PlayDelayed(Single delay)
---@param optional Single delay
function m:PlayDelayed(delay) end
---public Void PlayScheduled(Double time)
---@param optional Double time
function m:PlayScheduled(time) end
---public Void PlayOneShot(AudioClip clip)
---public Void PlayOneShot(AudioClip clip, Single volumeScale)
---@param AudioClip clip
---@param optional Single volumeScale
function m:PlayOneShot(clip, volumeScale) end
---public Void SetScheduledStartTime(Double time)
---@param optional Double time
function m:SetScheduledStartTime(time) end
---public Void SetScheduledEndTime(Double time)
---@param optional Double time
function m:SetScheduledEndTime(time) end
---public Void Stop()
function m:Stop() end
---public Void Pause()
function m:Pause() end
---public Void UnPause()
function m:UnPause() end
---public Void PlayClipAtPoint(AudioClip clip, Vector3 position)
---public Void PlayClipAtPoint(AudioClip clip, Vector3 position, Single volume)
---@param AudioClip clip
---@param optional Vector3 position
---@param optional Single volume
function m.PlayClipAtPoint(clip, position, volume) end
---public Void SetCustomCurve(AudioSourceCurveType t, AnimationCurve curve)
---@param optional AudioSourceCurveType t
---@param optional AnimationCurve curve
function m:SetCustomCurve(type, curve) end
---public AnimationCurve GetCustomCurve(AudioSourceCurveType t)
---@return AnimationCurve
---@param optional AudioSourceCurveType t
function m:GetCustomCurve(type) end
---public Void GetOutputData(Single[] samples, Int32 channel)
---@param optional Single[] samples
---@param optional Int32 channel
function m:GetOutputData(samples, channel) end
---public Void GetSpectrumData(Single[] samples, Int32 channel, FFTWindow window)
---@param optional Single[] samples
---@param optional Int32 channel
---@param optional FFTWindow window
function m:GetSpectrumData(samples, channel, window) end
---public Boolean SetSpatializerFloat(Int32 index, Single value)
---@return bool
---@param optional Int32 index
---@param optional Single value
function m:SetSpatializerFloat(index, value) end
---public Boolean GetSpatializerFloat(Int32 index, Single& value)
---@return bool
---@param optional Int32 index
---@param optional Single& value
function m:GetSpatializerFloat(index, value) end
---public Boolean GetAmbisonicDecoderFloat(Int32 index, Single& value)
---@return bool
---@param optional Int32 index
---@param optional Single& value
function m:GetAmbisonicDecoderFloat(index, value) end
---public Boolean SetAmbisonicDecoderFloat(Int32 index, Single value)
---@return bool
---@param optional Int32 index
---@param optional Single value
function m:SetAmbisonicDecoderFloat(index, value) end
UnityEngine.AudioSource = m
return m
