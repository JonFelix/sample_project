--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

local pistol = {}
pistol.Name = 'West Elite AR403 w/ GATMOD'
pistol.Ammo = 100
pistol.MaxAmmo = 100
pistol.Rate = 0.2
pistol.Cooldown = 0
pistol.Owned = true
pistol.Key = 'selectpistol'
pistol.Color = core.Color(70, 200, 82, 255)
pistol.Icon = 'Sprites/pistol_icon_0'
pistol.Sprite = 'Sprites/rifle_0'
pistol.SpriteOffset = core.Vector(8, 8)
pistol.IsNPC = false
pistol.Damage = 10
pistol.SpriteIndex = 1
pistol.RecoilOffset = core.Vector(0, 0)
pistol.SpriteReturnOffset = core.Vector(8, 8)
pistol.Recoil = 3
pistol.Spread = 0.03
function pistol:Fire(direction)
	if KeyDown('fire') or self.IsNPC then
		if self.Ammo > 0 then
			if self.Cooldown == 0 then
				local offset = core.DirectionToVector(direction)
				offset = core.MultiplyVector(offset, 32)
				local projectileparams = {
					direction = direction + Random(-(self.Spread/2), self.Spread/2),
					Parent = Entity.ID,
					Damage = self.Damage
				}
				SpawnEntity("pistolprojectile", core.JoinVectors(Entity.Position, offset), projectileparams)
				if self.IsNPC == false then
					self.Ammo = self.Ammo - 1
				end
				self.Cooldown = self.Rate
				self.SpriteIndex = self.SpriteIndex + 1
				if self.SpriteIndex == 3 then self.SpriteIndex = 1 end
				if self.SpriteIndex == 1 then
					self.Sprite = 'Sprites/rifle_1'
				end
				if self.SpriteIndex == 2 then
					self.Sprite = 'Sprites/rifle_0'
				end
				self.RecoilOffset = core.MultiplyVector(core.NormalizeVector(offset), -self.Recoil)
				self.SpriteOffset = core.SubtractVectors(self.SpriteReturnOffset, self.RecoilOffset)
			end
		end
	end
end
function pistol:Update()
	self.Cooldown = core.Max(self.Cooldown - Time.DeltaTime, 0)
	if self.SpriteOffset.X ~= self.SpriteReturnOffset.X or self.SpriteOffset.X ~= self.SpriteReturnOffset.Y then
		self.SpriteOffset = core.VectorLerp(self.SpriteOffset, self.SpriteReturnOffset, self.Rate)
	end
end
return pistol