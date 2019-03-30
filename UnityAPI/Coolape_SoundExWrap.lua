---@class Coolape.SoundEx : UnityEngine.MonoBehaviour
---@field public self Coolape.SoundEx
---@field public mainAudio UnityEngine.AudioSource
---@field public singletonAudio UnityEngine.AudioSource
---@field public mainClip UnityEngine.AudioClip
---@field public soundEffectSwitch System.Boolean
---@field public musicBgSwitch System.Boolean

local m = { }
---public SoundEx .ctor()
---@return SoundEx
function m.New() end
---public Void PlaySoundWithCallback(AudioClip clip, Single volume, Object callback)
---@param optional AudioClip clip
---@param optional Single volume
---@param optional Object callback
function m.PlaySoundWithCallback(clip, volume, callback) end
---public Void playSound(AudioClip clip, Single volume, Int32 maxTimes)
---public Void playSound(String name, Single volume, Int32 maxTimes)
---@param optional String name
---@param optional Single volume
---@param optional Int32 maxTimes
function m.playSound(name, volume, maxTimes) end
---public Void doPlaySound(AudioClip clip, Single volume, Int32 maxTimes)
---@param optional AudioClip clip
---@param optional Single volume
---@param optional Int32 maxTimes
function m.doPlaySound(clip, volume, maxTimes) end
---public Void onFinishSetAudio(Object[] args)
---@param optional Object[] args
function m.onFinishSetAudio(args) end
---public Void playSoundSingleton(String name, Single volume)
---@param optional String name
---@param optional Single volume
function m.playSoundSingleton(name, volume) end
---public Void onFinishSetAudio4Singleton(Object[] args)
---@param optional Object[] args
function m.onFinishSetAudio4Singleton(args) end
---public Void playSound2(String clipName, Single volume)
---@param optional String clipName
---@param optional Single volume
function m.playSound2(clipName, volume) end
---public Void onGetMainMusic(Object[] args)
---@param optional Object[] args
function m.onGetMainMusic(args) end
---public Void stopMainMusic()
function m.stopMainMusic() end
---public Void doPlayMainMusic(AudioClip clip)
---@param optional AudioClip clip
function m.doPlayMainMusic(clip) end
---public Void playMainMusic(String soundName)
---@param optional String soundName
function m.playMainMusic(soundName) end
Coolape.SoundEx = m
return m
