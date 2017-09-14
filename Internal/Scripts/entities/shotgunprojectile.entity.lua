--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function Initialize(params)
	Entity.Direction = params.direction
	Entity.Depth = 0.48
	maxCount = 12
	spread = 0.30
	randomDirAmount = 0.05
	rayColor = core.Color(46, 51, 54, 255)
	SmokeParticleInfo = {
		AmountMinimum = 20,
		AmountMaximum = 30,
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
				X = Entity.Position.X + (core.DirectionToVector(Entity.Direction).X * 16),
				Y = Entity.Position.Y+ (core.DirectionToVector(Entity.Direction).Y * 16)
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
		Depth = 0.51
	}
	SmokeParticleInfo.DirectionMinimum = SmokeParticleInfo.DirectionMinimum - spread / 2
	SmokeParticleInfo.DirectionMaximum = SmokeParticleInfo.DirectionMaximum + spread / 2
	Entity.Direction = Entity.Direction - (spread / 2)
	Distance = 500
	damage = params.Damage
	shells = params.Shell
	dirIncrease = spread / maxCount
	IgnoreList = {params.Parent}
	rays = {}
	for i = 1, maxCount do
		dir = core.DirectionToVector(Entity.Direction + -Random(0.00, randomDirAmount))
		Entity.Direction = Entity.Direction + dirIncrease
		rays[i] = core.Vector(Entity.Position.X + dir.X * Distance, Entity.Position.Y + dir.Y * Distance)
		colls = Raycast(core.Vector(Entity.Position.X, Entity.Position.Y), rays[i], IgnoreList)
		if #colls > 0 then
			rays[i].X = colls[1].Position.X
			rays[i].Y = colls[1].Position.Y
			SendMessage(colls[1].ID, {Event = 'Hit', Weapon = 'Shotgun', Source = IgnoreList[1], Damage = damage, Position = colls[1].Position})
		end
	end
	NewTimer(core.Time(0, 0, 0, 10), {Event = 'die'})
	SpawnParticles(SmokeParticleInfo)
end

function Draw()
	for i = 1, #rays do
		DrawLine(core.Vector(Entity.Position.X, Entity.Position.Y), rays[i], rayColor, 1, Entity.Depth)
	end
end

function End()
end

function OnMessage(message)
	if message.BORDERPASSED ~= nil then
		RemoveEntity(Entity.ID)
	end
	if message.Event ~= nil then
		if message.Event == 'die' then
			RemoveEntity(Entity.ID)
		end
	end
end
--Copyright d4, 2017--