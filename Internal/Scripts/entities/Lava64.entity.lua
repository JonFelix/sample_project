--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function DealDamage(id)
	SendMessage(id, {Event = "DealDamage", Damage = 10, SourceID = Entity.ID, Source = 'Lava'})
	entitiesInLava[id] = true
	NewTimer(timer, {Event = 'RefreshTimer', ID = id})
end

function Initialize(params)
	texture = {
		'Sprites/lava64_0',
		'Sprites/lava64_1',
		'Sprites/lava64_2',
		'Sprites/lava64_3',
		'Sprites/lava64_4',
		'Sprites/lava64_5',
		'Sprites/lava64_6',
		'Sprites/lava64_7',
		'Sprites/lava64_8',
		'Sprites/lava64_9'
	}
	textureIndex = 1
	textureSpeed = core.Time(0, 0, 0, 200)
	SetTexture(texture[textureIndex])
	Entity.Depth = 0.3
	entitiesInLava = {}
	timer = core.Time(0, 0, 1, 0)
	NewTimer(textureSpeed, {Event = 'ChangeTexture'})
	partMinTimer = 0
	partMaxTimer = 1
	ParticleInfo = {
		AmountMinimum = 30,
		AmountMaximum = 50,
		DirectionMinimum = 0,
		DirectionMaximum = math.pi * 2,
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
			core.PARTICLE_CIRCLE
		},
		Colors = {
			core.Color(237, 210, 99, 255),
			core.Color(255, 255, 170, 255),
			core.Color(222, 148, 53, 255),
			core.Color(215, 123, 43, 255),
			core.Color(202, 83, 27, 255),
		},
		Depth = 0.5
	}
	NewTimer(core.Time(0, 0, Random(partMinTimer, partMaxTimer), Random(1, 999)), {Event = 'FireParticles'})
end

function Update()
	colls = CheckPosition(core.Rect(Entity.Position.X, Entity.Position.Y, Entity.Texture.Width, Entity.Texture.Height))
	for i = 1, #colls do
		if entitiesInLava[colls[i]] == nil then
			DealDamage(colls[i])
		else
			if entitiesInLava[colls[i]] == false then
				DealDamage(colls[i])
			end
		end
	end
end

function OnMessage(message)
	if message.Event ~= nil then
		if message.Event == 'RefreshTimer' then
			entitiesInLava[message.ID] = false
		end
		if message.Event == 'ChangeTexture' then
			textureIndex = textureIndex + 1
			if textureIndex > #texture then
				textureIndex = 1
			end
			SetTexture(texture[textureIndex])
			NewTimer(textureSpeed, {Event = 'ChangeTexture'})
		end
		if message.Event == 'FireParticles' then
			local x = Random(0, Entity.Texture.Width)
			local y = Random(0, Entity.Texture.Height)
			ParticleInfo.Position = core.Vector((Entity.Position.X - (Entity.Texture.Width / 2)) + x, (Entity.Position.Y - (Entity.Texture.Height / 2)) + y)
			SpawnParticles(ParticleInfo)
			NewTimer(core.Time(0, 0, Random(partMinTimer, partMaxTimer), Random(1, 999)), {Event = 'FireParticles'})
		end
	end
end
--Copyright d4, 2017--