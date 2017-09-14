--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

local shotgun = {}
shotgun.Name = 'Bossmann 24 gauge Double Barrel Shotgun'
shotgun.Ammo = 30
shotgun.MaxAmmo = 100
shotgun.Rate = 1.2
shotgun.Cooldown = 0
shotgun.Owned = true
shotgun.Key = 'selectshotgun'
shotgun.Color = core.Color(209, 140, 75, 255)
shotgun.Icon = 'Sprites/shotgun_icon_0'
shotgun.Sprite = 'Sprites/shotgun_0'
shotgun.SpriteOffset = core.Vector(16, 16)
shotgun.IsNPC = false
shotgun.Damage = 2
shotgun.Loaded = 0
function shotgun:Fire(direction)
	if KeyPressed('fire') or self.IsNPC then
		if self.Loaded ~= 0 then
			local offset = core.DirectionToVector(direction)
			offset = core.MultiplyVector(offset, 10)
			local projectileparams = {
				direction = direction,
				Parent = Entity.ID,
				Damage = self.Damage,
			}
			SpawnEntity("shotgunprojectile", core.JoinVectors(Entity.Position, offset), projectileparams)
			self.Loaded = self.Loaded - 1
			if	self.Loaded == 0 then
				self.Cooldown = self.Rate
			end
		end
	end
end
function shotgun:Update()
	if self.Loaded == 0 then
		if self.Cooldown == 0 then
			if self.Ammo > 1 then
				self.Loaded = 2
				self.Ammo = core.Max(self.Ammo - 2, 0)
			end
			if self.Ammo == 1 then
				self.Loaded = 1
				self.Ammo = core.Max(self.Ammo - 1, 0)
			end
		end
		self.Cooldown = core.Max(self.Cooldown - Time.DeltaTime, 0)
	end
end
return shotgun