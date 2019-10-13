---@class UnityEngine.ParticleSystem : UnityEngine.Component
---@field public isPlaying System.Boolean
---@field public isEmitting System.Boolean
---@field public isStopped System.Boolean
---@field public isPaused System.Boolean
---@field public particleCount System.Int32
---@field public time System.Single
---@field public randomSeed System.UInt32
---@field public useAutoRandomSeed System.Boolean
---@field public proceduralSimulationSupported System.Boolean
---@field public main UnityEngine.ParticleSystem.MainModule
---@field public emission UnityEngine.ParticleSystem.EmissionModule
---@field public shape UnityEngine.ParticleSystem.ShapeModule
---@field public velocityOverLifetime UnityEngine.ParticleSystem.VelocityOverLifetimeModule
---@field public limitVelocityOverLifetime UnityEngine.ParticleSystem.LimitVelocityOverLifetimeModule
---@field public inheritVelocity UnityEngine.ParticleSystem.InheritVelocityModule
---@field public forceOverLifetime UnityEngine.ParticleSystem.ForceOverLifetimeModule
---@field public colorOverLifetime UnityEngine.ParticleSystem.ColorOverLifetimeModule
---@field public colorBySpeed UnityEngine.ParticleSystem.ColorBySpeedModule
---@field public sizeOverLifetime UnityEngine.ParticleSystem.SizeOverLifetimeModule
---@field public sizeBySpeed UnityEngine.ParticleSystem.SizeBySpeedModule
---@field public rotationOverLifetime UnityEngine.ParticleSystem.RotationOverLifetimeModule
---@field public rotationBySpeed UnityEngine.ParticleSystem.RotationBySpeedModule
---@field public externalForces UnityEngine.ParticleSystem.ExternalForcesModule
---@field public noise UnityEngine.ParticleSystem.NoiseModule
---@field public collision UnityEngine.ParticleSystem.CollisionModule
---@field public trigger UnityEngine.ParticleSystem.TriggerModule
---@field public subEmitters UnityEngine.ParticleSystem.SubEmittersModule
---@field public textureSheetAnimation UnityEngine.ParticleSystem.TextureSheetAnimationModule
---@field public lights UnityEngine.ParticleSystem.LightsModule
---@field public trails UnityEngine.ParticleSystem.TrailModule
---@field public customData UnityEngine.ParticleSystem.CustomDataModule
local m = { }
---public ParticleSystem .ctor()
---@return ParticleSystem
function m.New() end
---public Void SetParticles(Particle[] particles)
---public Void SetParticles(Particle[] particles, Int32 size)
---public Void SetParticles(Particle[] particles, Int32 size, Int32 offset)
---@param Particle[] particles
---@param Int32 size
---@param optional Int32 offset
function m:SetParticles(particles, size, offset) end
---public Int32 GetParticles(Particle[] particles)
---public Int32 GetParticles(Particle[] particles, Int32 size)
---public Int32 GetParticles(Particle[] particles, Int32 size, Int32 offset)
---@return number
---@param Particle[] particles
---@param Int32 size
---@param optional Int32 offset
function m:GetParticles(particles, size, offset) end
---public Void SetCustomParticleData(List`1 customData, ParticleSystemCustomData streamIndex)
---@param optional List`1 customData
---@param optional ParticleSystemCustomData streamIndex
function m:SetCustomParticleData(customData, streamIndex) end
---public Int32 GetCustomParticleData(List`1 customData, ParticleSystemCustomData streamIndex)
---@return number
---@param optional List`1 customData
---@param optional ParticleSystemCustomData streamIndex
function m:GetCustomParticleData(customData, streamIndex) end
---public Void Simulate(Single t)
---public Void Simulate(Single t, Boolean withChildren)
---public Void Simulate(Single t, Boolean withChildren, Boolean restart)
---public Void Simulate(Single t, Boolean withChildren, Boolean restart, Boolean fixedTimeStep)
---@param Single t
---@param Boolean withChildren
---@param Boolean restart
---@param optional Boolean fixedTimeStep
function m:Simulate(t, withChildren, restart, fixedTimeStep) end
---public Void Play()
---public Void Play(Boolean withChildren)
---@param Boolean withChildren
function m:Play(withChildren) end
---public Void Pause()
---public Void Pause(Boolean withChildren)
---@param Boolean withChildren
function m:Pause(withChildren) end
---public Void Stop()
---public Void Stop(Boolean withChildren)
---public Void Stop(Boolean withChildren, ParticleSystemStopBehavior stopBehavior)
---@param Boolean withChildren
---@param ParticleSystemStopBehavior stopBehavior
function m:Stop(withChildren, stopBehavior) end
---public Void Clear()
---public Void Clear(Boolean withChildren)
---@param Boolean withChildren
function m:Clear(withChildren) end
---public Boolean IsAlive()
---public Boolean IsAlive(Boolean withChildren)
---@return bool
---@param Boolean withChildren
function m:IsAlive(withChildren) end
---public Void Emit(Int32 count)
---public Void Emit(EmitParams emitParams, Int32 count)
---@param EmitParams emitParams
---@param optional Int32 count
function m:Emit(emitParams, count) end
---public Void TriggerSubEmitter(Int32 subEmitterIndex)
---public Void TriggerSubEmitter(Int32 subEmitterIndex, Particle& particle)
---public Void TriggerSubEmitter(Int32 subEmitterIndex, List`1 particles)
---@param Int32 subEmitterIndex
---@param optional List`1 particles
function m:TriggerSubEmitter(subEmitterIndex, particles) end
---public Void ResetPreMappedBufferMemory()
function m.ResetPreMappedBufferMemory() end
---public Void ClearJob()
function m:ClearJob() end
UnityEngine.ParticleSystem = m
return m
