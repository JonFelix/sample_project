--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

local rocketlauncher = {}
rocketlauncher.Name = 'StarX Grenade Thrower'
rocketlauncher.Ammo = 100
rocketlauncher.MaxAmmo = 100
rocketlauncher.Rate = 0.8
rocketlauncher.Cooldown = 0
rocketlauncher.Owned = true
rocketlauncher.Key = 'selectgrenadelauncher'
rocketlauncher.Color = core.Color(238, 187, 34, 255)
rocketlauncher.Icon = 'Sprites/rocketlauncher_icon_0'
rocketlauncher.Sprite = 'Sprites/rocketlauncher'
rocketlauncher.SpriteOffset = core.Vector(32, 32)
rocketlauncher.FirePointOffset = 64
rocketlauncher.IsNPC = false
rocketlauncher.Damage = 50
function rocketlauncher:Fire(direction)
	if KeyPressed('fire') or self.IsNPC then
		if self.Ammo > 0 then
			if self.Cooldown == 0 then
				local offset = core.DirectionToVector(direction)
				offset = core.MultiplyVector(offset, self.FirePointOffset)
				local projectileparams = {
					direction = direction,
					Parent = Entity.ID,
					Damage = self.Damage
				}
				SpawnEntity("grenadeprojectile", core.JoinVectors(Entity.Position, offset), projectileparams)
				if self.IsNPC == false then
					self.Ammo = self.Ammo - 1
				end
				self.Cooldown = self.Rate
			end
		end
	end
end
function rocketlauncher:Update()
	self.Cooldown = core.Max(self.Cooldown - Time.DeltaTime, 0)
end
return rocketlauncher