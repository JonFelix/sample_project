--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")

function Initialize(params)
	Entity.Direction = params.direction
	Entity.Depth = 0.5
	Distance = 300
	texture = {
		'Sprites/laser_explosion_0',
		'Sprites/laser_explosion_1',
		'Sprites/laser_explosion_2',
		'Sprites/laser_explosion_3',
		'Sprites/laser_explosion_4',
		'Sprites/laser_explosion_5',
		'Sprites/laser_explosion_6'
	}
	textureSpeed = 50
	textureIndex = 1
	texturePosition = core.Vector(Entity.Position.X, Entity.Position.Y)
	textureDirection = core.RandomRotation()
	NewTimer(core.Time(0, 0, 0, textureSpeed), {Event = 'ChangeTexture'})
	dir = core.DirectionToVector(params.direction)
	ParticleInfo = {
		AmountMinimum = 10,
		AmountMaximum = 20,
		DirectionMinimum = Entity.Direction - (math.pi / 8),
		DirectionMaximum = Entity.Direction + (math.pi / 8),
		SpeedMinimum = 50,
		SpeedMaximum = 100,
		ScaleMinimum = 0.1,
		ScaleMaximum = 0.5,
		RotationSpeedMaximum = 0,
		RotationSpeedMinimum = 0,
		LifeMinimum = 0,
		LifeMaximum = 0.5,
		Position = {
			X = Entity.Position.X,
			Y = Entity.Position.Y
			},
		Sprites = {
			core.PARTICLE_SQUARE
			},
		Colors = {
			params.Color
			},
		Depth = 0.5
	}
	SpawnParticles(ParticleInfo)
	ParticleInfo.DirectionMinimum = 0
	ParticleInfo.DirectionMaximum = math.pi * 2
	colls = Raycast(core.Vector(Entity.Position.X, Entity.Position.Y), core.Vector(Entity.Position.X + (dir.X * Distance), Entity.Position.Y + (dir.Y * Distance)), IgnoreList)
	local dest = core.Vector(Entity.Position.X + (dir.X * Distance), Entity.Position.Y + (dir.Y * Distance))
	if #colls > 0 then
		dest = colls[1].Position
	end
	ParticleInfo.Position.X = dest.X
	ParticleInfo.Position.Y = dest.Y
	texturePosition = core.Vector(dest.X, dest.Y)
	SpawnParticles(ParticleInfo)
	col = params.Color
	colls = CheckDistance(dest, 94)
	for i = 1, #colls do
		local other = GetEntity(colls[i])
		local ratio = core.Distance(Entity.Position, other.Position) / 92
		SendMessage(colls[i], {Event = 'Hit', Weapon = 'Blaster', Source = params.Parent, Damage = params.Damage, DamageRatio = ratio, Position = dest })
	end
end


function Draw()
	DrawTexture(texture[textureIndex], texturePosition, textureDirection, col, Entity.Depth)
end

function End()
end

function OnMessage(message)
	if message.Event ~= nil then
		if message.Event == 'ChangeTexture' then
			textureIndex = textureIndex + 1
			if #texture < textureIndex then
				RemoveEntity(Entity.ID)
			else
				NewTimer(core.Time(0, 0, 0, textureSpeed), {Event = 'ChangeTexture'})
			end
		end
	end
end
--Copyright d4, 2017--
