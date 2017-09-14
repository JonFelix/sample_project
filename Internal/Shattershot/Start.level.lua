--MOD:Shattershot
--Copyright d4, 2017--
core = require("d4core")

function Initialize()
	SetMenuLevel(false)
	SetBoundaries(core.Rect(0, 0, 4032, 1728))
	tile = 64
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 10), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 11), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 12), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 13), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 14), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 15), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 16), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 17), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 18), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 19, tile * 10), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 11), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 12), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 13), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 14), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 15), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 16), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 17), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 18), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 11, tile * 10), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 12, tile * 10), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 13, tile * 10), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 14, tile * 10), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 15, tile * 10), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 16, tile * 10), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 10), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 18, tile * 10), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 11, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 12, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 13, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 14, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 15, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 16, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 18, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })


	SpawnEntity('Player', core.Vector(tile * 15, tile * 15))
end
--Copyright d4, 2017--