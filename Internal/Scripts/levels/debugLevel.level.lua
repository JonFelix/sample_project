--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function Initialize()
	SetMenuLevel(false)
	SetBoundaries(core.Rect(0, 0, 2240, 1728))
	tile = 64
	SpawnEntity('Player', core.Vector(tile * 13, tile * 2))
	
	
	


	SpawnEntity('RoboK9', core.Vector(tile * 10, tile * 11), {Weapon = 'LaserRifle'})
	
end
--Copyright d4, 2017--