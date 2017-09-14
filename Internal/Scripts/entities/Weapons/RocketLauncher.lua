--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

local rocketlauncher = {}
rocketlauncher.Name = 'Rocket Launcher'
rocketlauncher.Ammo = 100
rocketlauncher.MaxAmmo = 100
rocketlauncher.Rate = 0.8
rocketlauncher.Cooldown = 0
rocketlauncher.Owned = true
rocketlauncher.Key = 'selectrocketlauncher'
rocketlauncher.Color = core.Color(200, 70, 82, 255)
rocketlauncher.Icon = 'Sprites/rocketlauncher_icon_0'
rocketlauncher.Sprite = 'Sprites/rocketlauncher'
rocketlauncher.SpriteOffset = core.Vector(8, 8)
rocketlauncher.FirePointOffset = 32
rocketlauncher.IsNPC = false
rocketlauncher.Damage = 50
function rocketlauncher:Fire()
	if KeyPressed('fire') or self.IsNPC then
		if self.Ammo > 0 then
			if self.Cooldown == 0 then
				local offset = core.DirectionToVector(Entity.Direction)
				offset = core.MultiplyVector(offset, self.FirePointOffset)
				local projectileparams = {
					direction = Entity.Direction,
					Parent = Entity.ID,
					Damage = self.Damage
				}
				SpawnEntity("rocketprojectile", core.JoinVectors(Entity.Position, offset), projectileparams)
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