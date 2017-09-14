--MOD:Shattershot
--Copyright d4, 2017--
core = require("d4core")

local pistol = {}
pistol.Name = 'Pistol'
pistol.Ammo = 100
pistol.MaxAmmo = 100
pistol.Rate = 0.2
pistol.Cooldown = 0
pistol.Owned = true
pistol.Key = 'selectpistol'
pistol.Damage = 10
function pistol:Fire()
	if KeyPressed('fire') then
		if self.Ammo > 0 then
			if self.Cooldown == 0 then
				local offset = core.MultiplyVector(core.DirectionToVector(Entity.Direction), 32)
				local projectileparams = {
					Direction = Entity.Direction,
					Parent = Entity.ID,
					Damage = self.Damage
				}
				SpawnEntity("Laserprojectile", core.JoinVectors(Entity.Position, offset), projectileparams)
				self.Ammo = self.Ammo - 1
				self.Cooldown = self.Rate
			end
		end
	end
end
function pistol:Update()
	self.Cooldown = core.Max(self.Cooldown - Time.DeltaTime, 0)
end
return pistol