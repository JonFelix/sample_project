--Copyright d4, 2017--
--d4 AI Library
core = require("d4core")

local STATE_DORMENT = 0
local STATE_ACTIVE = 1

local function InitiateAI()
return {
	Weapon = nil,
	Range = 256,
	Health = 20,
	Name = 'Unkown Entity',
	Target = nil,
	State = STATE_DORMENT,
	FieldOfView = math.pi / 3,
	PlayerLastSeenPosition = {},
	StartPosition = core.Vector(Entity.Position.X, Entity.Position.Y),
	Speed = 100,
	Player = GetEntitiesByScript('Player.entity.lua')[1],
	MoveDirection = core.Vector(0, 0),
	MoveTimer = 0,
	IsDead = false,
		BloodParticleInfo = {
			AmountMinimum = 5,
			AmountMaximum = 20,
			DirectionMinimum = 0,
			DirectionMaximum = math.pi * 2,
			SpeedMinimum = 200,
			SpeedMaximum = 350,
			ScaleMinimum = 0.1,
			ScaleMaximum = 0.5,
			RotationSpeedMaximum = 0,
			RotationSpeedMinimum = 0,
			LifeMinimum = 0,
			LifeMaximum = 0,
			Position = {
				X = Entity.Position.X,
				Y = Entity.Position.Y
				},
			Sprites = {
				core.PARTICLE_CIRCLE
			},
			Colors = {
				core.Color(255, 0, 0, 255),
			},
			Depth = 0.5,
			DragMaximum = 50,
			DragMinimum = 25,
		}
	}
end

local function IsTargetInLineOfSight(me, target)
	if target == nil then return false end
	local targetDir = core.SubtractVectors(core.Vector(target.Position.X, target.Position.Y), core.Vector(Entity.Position.X, Entity.Position.Y))
	targetDir = core.VectorToDirection(core.NormalizeVector(targetDir))
	targetDir = targetDir - Entity.Direction
	local targetInFOW = false
	local minFOW = -(me.FieldOfView / 2)
	local maxFOW = (me.FieldOfView / 2)
	if targetDir > minFOW and maxFOW > targetDir then
		targetInFOW = true
	end
	if targetInFOW or me.Target == target then
		local targetRay = Raycast(core.Vector(Entity.Position.X, Entity.Position.Y), core.Vector(target.Position.X, target.Position.Y), { Entity.ID, target.ID })
		if #targetRay == 0 then
			return true
		end
	end
	return false
end

local function CanISeePlayer(me)
	if me.Target ~= nil then return false end -- Already has a target. Will focus on him until player damages me.
	if IsTargetInLineOfSight(me, me.Player) then
		return true
	else
		return false
	end
end

local function CheckForMessages(me, message)
	if me.Health <= 0 then return me end
	if message.Event ~= nil then
		if message.Event == 'Hit' then
			me.Target = GetEntity(message.Source)
			me.BloodParticleInfo.Position = message.Position
			me.BloodParticleInfo.DirectionMaximum = core.VectorToDirection(core.Vector(Entity.Position.X - message.Position.X, Entity.Position.Y - message.Position.Y)) + (math.pi / 4)
			me.BloodParticleInfo.DirectionMinimum = core.VectorToDirection(core.Vector(Entity.Position.X - message.Position.X, Entity.Position.Y - message.Position.Y)) - (math.pi / 4)
			SpawnParticles(me.BloodParticleInfo)
			SpawnDecal('Sprites/bloodsplatter_decal_0', core.Vector(Entity.Position.X, Entity.Position.Y), core.Color(150, 0, 0, 255), core.RandomRotation(), math.random(), Entity.Depth - 0.1)
			me.Health = core.Max(0, me.Health - message.Damage)
			if message.Source == me.Player.ID then
				local other = GetEntity(message.Source)
				if core.Distance(core.Vector(Entity.Position.X, Entity.Position.Y), core.Vector(other.Position.X, other.Position.Y)) <= 128 then
					SendMessageToWidget('ScreenBloodsplatter', {Event = 'DisplayBlood'})
				end
			end
		end
		if message.Event == 'IDied' then
			if me.Target ~= nil then
				if message.ID == me.Target.ID then
					me.Target = nil
				end
			end
		end
	end
	return me
end

local function MoveToTarget(me)
	if me.Target == nil then return me end
	
	if #Raycast(core.Vector(Entity.Position.X, Entity.Position.Y), core.Vector(me.Target.Position.X, me.Target.Position.Y), {Entity.ID, me.Target.ID}) == 0 then
		local distToTarget = core.Distance(core.Vector(Entity.Position.X, Entity.Position.Y), core.Vector(me.Target.Position.X, me.Target.Position.Y))
		if distToTarget >= 80 then
			me.MoveDirection = core.NormalizeVector(core.Vector(me.Target.Position.X - Entity.Position.X, me.Target.Position.Y - Entity.Position.Y))
		else
			me.MoveDirection = core.Vector(0, 0)
		end
	else
		if me.MoveTimer == 0 then	
			local newDir = Entity.Direction - 1.3561
			newDir = newDir + Random(0, 2.7123)
			me.MoveDirection = core.DirectionToVector(newDir)
		end
		if me.MoveDirection.X ~= 0 and me.MoveDirection.Y ~= 0 then
			me.MoveTimer = Random(400, 800) * 0.0001
		end
	end
	local movePos = core.Vector(me.MoveDirection.X * me.Speed * Time.DeltaTime, me.MoveDirection.Y * me.Speed * Time.DeltaTime)
	if me.MoveTimer > 0 then
		if #CheckPosition(core.Rect(Entity.Position.X + movePos.X, Entity.Position.Y + movePos.Y, Entity.Texture.Width, Entity.Texture.Height), 0, {Entity.ID}) == 0 then
			Entity.Direction = core.VectorToDirection(me.MoveDirection)
			Move(movePos)
		else
			me.MoveTimer = 0
		end
	end
	return me
end

local function ShootAtTarget(me)
	if me.Target == nil then return false end
	Entity.Direction = (core.VectorToDirection(core.Vector(me.Target.Position.X - Entity.Position.X, me.Target.Position.Y - Entity.Position.Y)))
	me.Weapon.Fire(me.Weapon)
	return true
end

local function HasTarget(me)
	if me.Target == nil then return false end
	return true
end

local function DeathEvent(me)
	if me.Health <= 0 then
		if me.IsDead == false then
			local count = math.random(15)
			me.BloodParticleInfo.Position = core.Vector(Entity.Position.X, Entity.Position.Y)
			me.BloodParticleInfo.AmountMinimum = 10
			me.BloodParticleInfo.AmountMaximum = 15
			for i = 1, count do
				SpawnDecal('Sprites/bloodsplatter_decal_0', core.Vector(Entity.Position.X + Random(-20, 20), Entity.Position.Y + Random(-20, 20)), core.Color(150, 0, 0, 255), core.RandomRotation(), math.random(), Entity.Depth - 0.1)
				SpawnParticles(me.BloodParticleInfo)
			end
			BroadcastMessage({Event = 'IDied', ID = Entity.ID})
			Entity.IsCollidable = false
			me.IsDead = true
		end
	end
	return me
end

local function Update(me)
	me.MoveTimer = core.Max(me.MoveTimer - Time.DeltaTime, 0)
	me.Weapon.Update(me.Weapon)
	return me
end

return {
	CheckForMessages = CheckForMessages,
	InitiateAI = InitiateAI,
	MoveToTarget = MoveToTarget,
	ShootAtTarget = ShootAtTarget,
	IsTargetInLineOfSight = IsTargetInLineOfSight,
	DeathEvent = DeathEvent,
	CanISeePlayer = CanISeePlayer,
	HasTarget = HasTarget,
	Update = Update,
	State = {STATE_ACTIVE, STATE_DORMENT}
}