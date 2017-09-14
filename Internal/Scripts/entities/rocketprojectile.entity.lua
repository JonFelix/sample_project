--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function Initialize(params)
	SetTexture("Sprites/rocketprojectile")
	Entity.IsCollidable = false
	Entity.Direction = params.direction
	Entity.Depth = 0.5
	explosionTextures = {
		'Sprites/rocketexplosion_0',
		'Sprites/rocketexplosion_1',
		'Sprites/rocketexplosion_2',
		'Sprites/rocketexplosion_3',
		'Sprites/rocketexplosion_4'
	}
	damage = params.Damage
	textureSpeed = 0.1
	textureIndex = 1
	textureSpeedCount = 0
	exploded = false
	Speed = 500
	SmokeParticleInfo = {
		AmountMinimum = 3,
		AmountMaximum = 6,
		DirectionMinimum = 0,
		DirectionMaximum = math.pi * 2,
		SpeedMinimum = 5,
		SpeedMaximum = 30,
		ScaleMinimum = 0.02,
		ScaleMaximum = 0.06,
		RotationSpeedMaximum = 0,
		RotationSpeedMinimum = 0,
		LifeMinimum = 0.5,
		LifeMaximum = 1,
			Position = {
				X = Entity.Position.X,
				Y = Entity.Position.Y
				},
			Sprites = core.PARTICLE_SMOKE ,
			Colors = {
				core.Color(255, 255, 255, 255),
				core.Color(172, 172, 172, 100),
				core.Color(172, 172, 172, 255),
				core.Color(172, 172, 172, 100),
				core.Color(106, 106, 106, 255),
				core.Color(106, 106, 106, 100)
			},
		Depth = 0.5
		}
	SparkParticleInfo = {
		AmountMinimum = 20,
		AmountMaximum = 50,
		DirectionMinimum = 0,
		DirectionMaximum = math.pi * 2,
		SpeedMinimum = 60,
		SpeedMaximum = 100,
		ScaleMinimum = 0.3,
		ScaleMaximum = 0.43,
		RotationSpeedMaximum = 0,
		RotationSpeedMinimum = 0,
		LifeMinimum = 0.01,
		LifeMaximum = 0.07,
		Position = {
			X = Entity.Position.X, 
			Y = Entity.Position.Y
			},
		Sprites = { core.PARTICLE_CIRCLE },
		Colors = {
			core.Color(255, 128, 0, 255),
			core.Color(255, 128, 0, 100),
			core.Color(255, 167, 79, 255),
			core.Color(255, 167, 79, 100),
			core.Color(210, 210, 25, 255),
			core.Color(210, 210, 25, 100)
		},
		Depth = 0.5
	}
	WaveParticleInfo = {
		AmountMinimum = 150,
		AmountMaximum = 150,
		DirectionMinimum = 0,
		DirectionMaximum = math.pi * 2,
		SpeedMinimum = 600,
		SpeedMaximum = 600,
		ScaleMinimum = 0.3,
		ScaleMaximum = 0.3,
		RotationSpeedMaximum = 0,
		RotationSpeedMinimum = 0,
		LifeMinimum = 0.1,
		LifeMaximum = 0.1,
		Position = {
			X = Entity.Position.X,
			Y = Entity.Position.Y
			},
		Sprites = { core.PARTICLE_SQUARE },
		Colors = { core.Color(255, 255, 255, 100) },
		Depth = 0.5
	}
	NewTimer(core.Time(0, 0, 1, 600), { TTK = true })
	Parent = params.Parent
end

function Update()
	if exploded == false then
		if KeyPressed('detonaterocket') then
			exploded = true
		end
		dir = core.DirectionToVector(Entity.Direction)
		colls = Raycast(core.Vector(Entity.Position.X, Entity.Position.Y), core.Vector(Entity.Position.X + dir.X * Speed * Time.DeltaTime, Entity.Position.Y + dir.Y * Speed * Time.DeltaTime), IgnoreList)
		if #colls > 0 then
			exploded = true
			Entity.Position.X = colls[1].Position.X
			Entity.Position.Y = colls[1].Position.Y
			SendMessage(colls[1].ID, {Event = 'Hit', Weapon = 'RocketLauncher', Source = Parent, Damage = damage, Position = colls[1].Position})
		end
		if exploded == false then
			SmokeParticleInfo.Position.X = Entity.Position.X + ((dir.X * 32) * -1)
			SmokeParticleInfo.Position.Y = Entity.Position.Y + ((dir.Y * 32) * -1)
			SparkParticleInfo.Position.X = Entity.Position.X
			SparkParticleInfo.Position.Y = Entity.Position.Y
			SparkParticleInfo.DirectionMinimum = Entity.Direction - (math.pi / 4)
			SparkParticleInfo.DirectionMaximum = Entity.Direction + (math.pi / 4)
			SpawnParticles(SmokeParticleInfo)
			SpawnParticles(SparkParticleInfo)
			Move(core.Vector(dir.X * Speed * Time.DeltaTime, dir.Y * Speed *Time.DeltaTime))
		else
			WaveParticleInfo.Position.X = Entity.Position.X
			WaveParticleInfo.Position.Y = Entity.Position.Y
			SpawnParticles(WaveParticleInfo)
			Entity.Direction = core.RandomRotation()
			collsDist = CheckDistance(core.Vector(Entity.Position.X, Entity.Position.Y), 92)
			for c = 1, #collsDist do
				SendMessage(collsDist[c], {Event = 'Hit', Weapon = 'RocketBlast', Source = Parent, Damage = damage, Position = core.Vector(Entity.Position.X , Entity.Position.Y)})
			end
		end
	else
		if textureIndex > #explosionTextures then
			textureIndex = #explosionTextures
			SmokeParticleInfo.LifeMaximum = 1.5
			SmokeParticleInfo.LifeMinimum = 1
			SmokeParticleInfo.Position.X = Entity.Position.X
			SmokeParticleInfo.Position.Y = Entity.Position.Y
			SmokeParticleInfo.Depth = 1
			SmokeParticleInfo.AmountMinimum = 20
			SmokeParticleInfo.AmountMaximum = 30
			SmokeParticleInfo.SpeedMaximum = 15
			SpawnParticles(SmokeParticleInfo)
			RemoveEntity(Entity.ID)
		else
			SetTexture(explosionTextures[textureIndex])
			textureSpeedCount = textureSpeedCount + Time.DeltaTime
			if textureSpeedCount > textureSpeed then
				textureIndex = textureIndex + 1
				textureSpeedCount = 0
			end
		end
	end
end

function OnMessage(message)
	if message.BORDERPASSED ~= nil then
		exploded = true
	end
	if message.TTK ~= nil then
		exploded = true
	end
end
--Copyright d4, 2017--