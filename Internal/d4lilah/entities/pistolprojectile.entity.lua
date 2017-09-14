--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")

function Initialize(params)
	Entity.Direction = params.direction + Random(-0.03, 0.03)
	Entity.Depth = 0.5
	Distance = 1000
	damage = params.Damage
	IgnoreList = {params.Parent}
	dir = core.DirectionToVector(Entity.Direction)
	endpoint = core.Vector(Entity.Position.X + dir.X * Distance, Entity.Position.Y + dir.Y * Distance)
	colls = Raycast(core.Vector(Entity.Position.X, Entity.Position.Y), endpoint, IgnoreList)
	if #colls > 0 then
		endpoint.X = colls[1].Position.X
		endpoint.Y = colls[1].Position.Y
		SendMessage(colls[1].ID, {Event = 'Hit', Weapon = 'Pistol', Source = IgnoreList[1], Damage = damage, Position = colls[1].Position})
	end
	NewTimer(core.Time(0, 0, 0, 10), {Event = 'die'})
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
	SpawnParticles(SmokeParticleInfo)
end

function Draw()
	DrawLine(core.Vector(Entity.Position.X, Entity.Position.Y), endpoint, core.Color(46, 51, 54, 255), 1, Entity.Depth)
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