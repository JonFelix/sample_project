--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")
ai = require("d4AI")

function Attack()
	Speed = Speed + (Acceleration * Time.DeltaTime)
	local dir = core.DirectionToVector(Entity.Direction)
	local appliedSpeed = core.Vector(dir.X * Speed, dir.Y * Speed)

	while #CheckPosition(core.Rect(Entity.Position.X + appliedSpeed.X, Entity.Position.Y, Entity.Texture.Width, Entity.Texture.Height), 0, {Entity.ID}) > 0 do
		explode = true
		if core.Abs(appliedSpeed.X) < 1 then
			appliedSpeed.X = 0
			break
		end
		appliedSpeed.X = appliedSpeed.X - core.Sign(appliedSpeed.X)
	end
	if explode == false then
		Move(core.Vector(appliedSpeed.X, 0))
	end
	while #CheckPosition(core.Rect(Entity.Position.X, Entity.Position.Y + appliedSpeed.Y, Entity.Texture.Width, Entity.Texture.Height), 0, {Entity.ID}) > 0 do
		explode = true
		if core.Abs(appliedSpeed.Y) < 1 then
			appliedSpeed.Y = 0
			break
		end
		appliedSpeed.Y = appliedSpeed.Y - core.Sign(appliedSpeed.Y)
	end
	if explode == false then
		Move(core.Vector(0, appliedSpeed.Y))
	end
	if explode then
		local colls = CheckDistance(core.Vector(Entity.Position.X, Entity.Position.Y), ExplosionDistance, {Entity.ID})
		for i = 1, #colls do
			SendMessage(colls[i], {Event = 'Hit', Weapon = 'Orb', Source = Entity.ID, Damage = 10, Position = core.Vector(Entity.Position.X, Entity.Position.Y)})
		end
	end
end

function Initialize(params)
	texture = {
		'Sprites/orb_0',
		'Sprites/orb_1',
		'Sprites/orb_2',
		'Sprites/orb_3',
		'Sprites/orb_4',
		'Sprites/orb_5',
		'Sprites/orb_6',
		'Sprites/orb_7',
		'Sprites/orb_8',
		'Sprites/orb_9',
		'Sprites/orb_10'
	}
	textureIndex = 1
	textureSpeed = 100
	rotationSpeed = math.pi / 2
	NewTimer(core.Time(0, 0, 0, textureSpeed), {Event = 'ChangeTexture'})
	SetTexture(texture[textureIndex])
	Entity.IsCollidable = true
	Entity.Depth = 0.5
	explode = false
	me = ai.InitiateAI()
	me.Name = 'Orb'
	me.Health = 1

	Acceleration = 20
	Speed = 0
	ExplosionDistance = 128
	
end

function Update()
	if me.Health > 0 then
		if ai.CanISeePlayer(me) then
			me.Target = me.Player
		end
		if ai.IsTargetInLineOfSight(me, me.Target) then
			if explode then
				Entity.IsCollidable = false
			else
				Attack()
			end
		else
			Entity.Direction = Entity.Direction + rotationSpeed * Time.DeltaTime
			if ai.IsTargetInLineOfSight(me, me.Target) then
				Entity.Direction = core.VectorToDirection(core.Vector(Entity.Position.X - me.Target.Position.X, Entity.Position.Y - me.Target.Position.Y))
			end
		end
	else
		me = ai.DeathEvent(me)
	end
end

function OnMessage(message)
	me = ai.CheckForMessages(me, message)
	if message.Event ~= nil then
		if message.Event == 'ChangeTexture' then
			textureIndex = textureIndex + 1
			if textureIndex > #texture then
				textureIndex = #texture
			end
			SetTexture(texture[textureIndex])
			NewTimer(core.Time(0, 0, 0, textureSpeed), {Event = 'ChangeTexture'})
		end
	end
end