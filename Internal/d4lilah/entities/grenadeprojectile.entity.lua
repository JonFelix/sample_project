--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")

function Initialize(params)
	SetTexture("Sprites/rocketprojectile")
	font = "Fonts/Unispace_regular_16"
	Entity.IsCollidable = false
	Entity.Direction = params.direction
	parentID = params.Parent
	Entity.Depth = 0.5
	explosionTextures = {
		'Sprites/rocketexplosion_0',
		'Sprites/rocketexplosion_1',
		'Sprites/rocketexplosion_2',
		'Sprites/rocketexplosion_3',
		'Sprites/rocketexplosion_4'
	}
	damage = params.Damage

	textureSpeedCount = 0
	exploded = false
	speed = 700
	velocity = core.Vector(0, 0)
	deacceleration = 1
	gravitationalPull = 1050
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
	NewTimer(core.Time(0, 0, 2, 500), { TTK = true })
	Parent = params.Parent
	velocity = core.MultiplyVector(core.DirectionToVector(Entity.Direction), speed)
	points = {}
	lineColor = core.Color(255, 0, 0, 255)
	points[#points + 1] = core.Vector(Entity.Position.X, Entity.Position.Y)
	timer = 2.5
	timerOffset = core.Vector(-32, -32)
	timerColor = core.Color(255, 255, 255, 255)
	initialExplosion = false
	explosionTextures = {
		'Sprites/rocketexplosion_0',
		'Sprites/rocketexplosion_1',
		'Sprites/rocketexplosion_2',
		'Sprites/rocketexplosion_3',
		'Sprites/rocketexplosion_4'
	}
	textureSpeed = 0.1
	textureIndex = 1
	textureSpeedCount = 0
end

function Update()
	if exploded == false then
		if timer > 0 then
			timer = timer - Time.DeltaTime
			local colls = CheckPosition(core.Rect(Entity.Position.X + (velocity.X * Time.DeltaTime), Entity.Position.Y, Entity.Texture.Width, Entity.Texture.Height), 0, {Entity.ID, parentID})
			if #colls > 0 then
				velocity.X = velocity.X * -0.5
				velocity.Y = velocity.Y * 0.5
			end

			colls = CheckPosition(core.Rect(Entity.Position.X, Entity.Position.Y + (velocity.Y * Time.DeltaTime), Entity.Texture.Width, Entity.Texture.Height), 0, {Entity.ID, parentID})
			if #colls > 0 then
				velocity.Y = velocity.Y * -0.5
				velocity.X = velocity.X * 0.5
			end
			if #CheckPosition(core.Rect(Entity.Position.X, Entity.Position.Y + 1, Entity.Texture.Width, Entity.Texture.Height), 0, {Entity.ID}) == 0 then
				velocity.Y = velocity.Y + (gravitationalPull * Time.DeltaTime)
			end
			newPos = core.JoinVectors(Entity.Position, core.MultiplyVector(velocity, Time.DeltaTime))
			Entity.Position.X = newPos.X
			Entity.Position.Y = newPos.Y
			points[#points + 1] = core.Vector(Entity.Position.X, Entity.Position.Y)
		else
			exploded = true
		end
	else
		if initialExplosion == false then
			WaveParticleInfo.Position.X = Entity.Position.X
			WaveParticleInfo.Position.Y = Entity.Position.Y
			SpawnParticles(WaveParticleInfo)
			Entity.Direction = core.RandomRotation()
			collsDist = CheckDistance(core.Vector(Entity.Position.X, Entity.Position.Y), 92)
			for c = 1, #collsDist do
				local other = GetEntity(collsDist[c])
				local ratio = core.Distance(Entity.Position, other.Position) / 92
				SendMessage(collsDist[c], {Event = 'Hit', Weapon = 'RocketBlast', Source = Parent, Damage = damage, DamageRatio = ratio, Position = core.Vector(Entity.Position.X , Entity.Position.Y)})
			end
			initialExplosion = true
		end
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

function Draw()
	for i = 2, #points do
		DrawLine(points[i - 1], points[i], lineColor, 1)
	end
	if timer > 0 then
		DrawText(font, string.sub(timer, 1, 4), core.Vector(Entity.Position.X + timerOffset.X, Entity.Position.Y + timerOffset.Y), timerColor)
	end
end
--Copyright d4, 2017--