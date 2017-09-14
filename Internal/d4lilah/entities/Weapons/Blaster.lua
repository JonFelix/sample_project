--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

local laserrifle = {}
laserrifle.Name = 'Cyprene Co. Blaster Rifle V.4'
laserrifle.Ammo = 150
laserrifle.MaxAmmo = 150
laserrifle.Rate = 0.2
laserrifle.Cooldown = 0
laserrifle.Owned = true
laserrifle.Key = 'selectlaserrifle'
laserrifle.Color = core.Color(97, 175, 239, 255)
laserrifle.Icon = 'Sprites/laserrifle_icon_0'
laserrifle.Sprite = 'Sprites/laserrifle'
laserrifle.SpriteOffset = core.Vector(8, 8)
laserrifle.FirePointOffset = 64
laserrifle.IsNPC = false
laserrifle.Damage = 5
function laserrifle:Fire(direction)
	if KeyDown('fire') or self.IsNPC then
		if self.Ammo > 0 then
			if self.Cooldown == 0 then
				local offset = core.DirectionToVector(direction)
				offset = core.MultiplyVector(offset, self.FirePointOffset)
				local projectileparams = {
					direction = direction,
					Parent = Entity.ID,
					Color = self.Color,
					Damage = self.Damage
				}
				SpawnEntity("blasterprojectile", core.JoinVectors(Entity.Position, offset), projectileparams)
				if self.IsNPC == false then
					self.Ammo = self.Ammo - 1
				end
				self.Cooldown = self.Rate
			end
		end
	end
end
function laserrifle:Update()
	self.Cooldown = core.Max(self.Cooldown - Time.DeltaTime, 0)
end
return laserrifle