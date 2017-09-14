--MOD:Shattershot
--Copyright d4, 2017--
core = require("d4core")

function EmitCollisionParticle(position)
	HitParticleInfo.Position = position
	SpawnParticles(HitParticleInfo)
end

function Initialize(params)
	SetTexture(params.Texture)
	Entity.IsCollidable = true
	Entity.Direction = params.Direction
	Entity.Depth = 0.9
	HitParticleInfo = {
			AmountMinimum = 20,
			AmountMaximum = 40,
			DirectionMinimum = 0,
			DirectionMaximum = math.pi * 2,
			SpeedMinimum = 130,
			SpeedMaximum = 170,
			ScaleMinimum = 0.3,
			ScaleMaximum = 0.43,
			RotationSpeedMaximum = 0,
			RotationSpeedMinimum = 0,
			LifeMinimum = 0.05,
			LifeMaximum = 0.15,
			Position = {
				X = Entity.Position.X,
				Y = Entity.Position.Y
				},
			Sprites = { core.PARTICLE_CIRCLE },
			Colors = {
				core.Color(70, 200, 82, 255),
			},
			Depth = 0.5
		}
end

function OnMessage(message)
	if message.Event ~= nil then
		if message.Event == 'Hit' then
			if message.Weapon ~= 'RocketBlast' then
				EmitCollisionParticle(message.Position)
			end
		end
	end
end
--Copyright d4, 2017--