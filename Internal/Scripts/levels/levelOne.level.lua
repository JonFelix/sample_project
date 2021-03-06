--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function Initialize()
	SetMenuLevel(false)
	SetBoundaries(core.Rect(0, 0, 2240, 1728))
	tile = 64
	SpawnEntity('Player', core.Vector(tile * 13, tile * 2))
	
	SpawnEntity('Cube', core.Vector(tile * 10, 0), { Texture = 'Sprites/Cube64_1', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 11, 0), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 12, 0), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 13, 0), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 14, 0), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 15, 0), { Texture = 'Sprites/Cube64_1', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 1), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 15, tile * 1), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 2), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 15, tile * 2), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 3), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 15, tile * 3), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 4), { Texture = 'Sprites/Cube64_1', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 3, tile * 4), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 4, tile * 4), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 5, tile * 4), { Texture = 'Sprites/Cube64_8', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 6, tile * 4), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 7, tile * 4), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 8, tile * 4), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 9, tile * 4), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 4), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 15, tile * 4), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 16, tile * 4), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 4), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 18, tile * 4), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 4), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 20, tile * 4), { Texture = 'Sprites/Cube64_1', Direction = core.Direction.Down })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 5), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 5, tile * 5), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 20, tile * 5), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 6), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 5, tile * 6), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 20, tile * 6), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 7), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 5, tile * 7), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 20, tile * 7), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })
	
	SpawnEntity('Cube', core.Vector(tile * 2, tile * 8), { Texture = 'Sprites/Cube64_8', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 3, tile * 8), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 5, tile * 8), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 20, tile * 8), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 21, tile * 8), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 22, tile * 8), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 8), { Texture = 'Sprites/Cube64_7', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 9), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 8, tile * 9), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 9, tile * 9), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 12, tile * 9), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 13, tile * 9), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 16, tile * 9), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 9), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 23, tile * 9), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 10), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 8, tile * 10), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 9, tile * 10), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 12, tile * 10), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 13, tile * 10), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 16, tile * 10), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 10), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 23, tile * 10), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 11), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 11), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 27, tile * 11), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 28, tile * 11), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 29, tile * 11), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 30, tile * 11), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 12), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 12), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 26, tile * 12), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 31, tile * 12), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 13), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 8, tile * 13), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 9, tile * 13), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 12, tile * 13), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 13, tile * 13), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 16, tile * 13), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 13), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 23, tile * 13), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 25, tile * 13), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 32, tile * 13), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 14), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 8, tile * 14), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 9, tile * 14), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 12, tile * 14), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 13, tile * 14), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 16, tile * 14), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 14), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 23, tile * 14), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 24, tile * 14), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 28, tile * 14), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 29, tile * 14), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 33, tile * 14), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 15), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 5, tile * 15), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 20, tile * 15), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 15), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 27, tile * 15), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 28, tile * 15), { Texture = 'Sprites/Cube64_1', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 29, tile * 15), { Texture = 'Sprites/Cube64_7', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 30, tile * 15), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 34, tile * 15), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 35, tile * 15), { Texture = 'Sprites/Cube64_7', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 16), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 5, tile * 16), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 20, tile * 16), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 27, tile * 16), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 30, tile * 16), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 35, tile * 16), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 17), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 5, tile * 17), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 20, tile * 17), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 17), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 27, tile * 17), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 28, tile * 17), { Texture = 'Sprites/Cube64_7', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 29, tile * 17), { Texture = 'Sprites/Cube64_7', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 30, tile * 17), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 35, tile * 17), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 18), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 5, tile * 18), { Texture = 'Sprites/Cube64_10', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 6, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 7, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 8, tile * 18), { Texture = 'Sprites/Cube64_11', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 9, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 11, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 12, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 13, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 14, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 15, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 16, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 18), { Texture = 'Sprites/Cube64_11', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 18, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 20, tile * 18), { Texture = 'Sprites/Cube64_11', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 21, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 22, tile * 18), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 18), { Texture = 'Sprites/Cube64_11', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 24, tile * 18), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 28, tile * 18), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 29, tile * 18), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 33, tile * 18), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 35, tile * 18), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 19), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 8, tile * 19), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 19), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 25, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 32, tile * 19), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 35, tile * 19), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 20), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 20), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 20), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 20, tile * 20), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 21, tile * 20), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 20), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 26, tile * 20), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 31, tile * 20), { Texture = 'Sprites/Cube64_12', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 34, tile * 20), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 2, tile * 21), { Texture = 'Sprites/Cube64_1', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 3, tile * 21), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 4, tile * 21), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 5, tile * 21), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 6, tile * 21), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 7, tile * 21), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 8, tile * 21), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 12, tile * 21), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 13, tile * 21), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 21), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 21), { Texture = 'Sprites/Cube64_4', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 24, tile * 21), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 27, tile * 21), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 28, tile * 21), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 29, tile * 21), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 30, tile * 21), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 33, tile * 21), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 8, tile * 22), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 22), { Texture = 'Sprites/Cube64_11', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 18, tile * 22), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 22, tile * 22), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 22), { Texture = 'Sprites/Cube64_8', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 25, tile * 22), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 32, tile * 22), { Texture = 'Sprites/Cube64_9', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 8, tile * 23), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 23), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 11, tile * 23), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 14, tile * 23), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 15, tile * 23), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 23), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 23), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 26, tile * 23), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 27, tile * 23), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 28, tile * 23), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 29, tile * 23), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 30, tile * 23), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 31, tile * 23), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })

	SpawnEntity('Cube', core.Vector(tile * 6, tile * 24), { Texture = 'Sprites/Cube64_1', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 7, tile * 24), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Down })
	SpawnEntity('Cube', core.Vector(tile * 8, tile * 24), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 24), { Texture = 'Sprites/Cube64_3', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 24), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 21, tile * 24), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 24), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 6, tile * 25), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 12, tile * 25), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 13, tile * 25), { Texture = 'Sprites/Cube64_2', Direction = core.Direction.Right })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 25), { Texture = 'Sprites/Cube64_4', Direction = core.Direction.Left })
	SpawnEntity('Cube', core.Vector(tile * 18, tile * 25), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 19, tile * 25), { Texture = 'Sprites/Cube64_8', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 20, tile * 25), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 21, tile * 25), { Texture = 'Sprites/Cube64_8', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 22, tile * 25), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 23, tile * 25), { Texture = 'Sprites/Cube64_7', Direction = core.Direction.Right })

	SpawnEntity('Cube', core.Vector(tile * 6, tile * 26), { Texture = 'Sprites/Cube64_1', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 7, tile * 26), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 8, tile * 26), { Texture = 'Sprites/Cube64_6', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 26), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Left })

	SpawnEntity('Cube', core.Vector(tile * 8, tile * 27), { Texture = 'Sprites/Cube64_1', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 9, tile * 27), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 10, tile * 27), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 11, tile * 27), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 12, tile * 27), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 13, tile * 27), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 14, tile * 27), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 15, tile * 27), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 16, tile * 27), { Texture = 'Sprites/Cube64_0', Direction = core.Direction.Up })
	SpawnEntity('Cube', core.Vector(tile * 17, tile * 27), { Texture = 'Sprites/Cube64_7', Direction = core.Direction.Right })





	SpawnEntity('Floor', core.Vector(tile * 11, tile * 3), { Texture = 'Sprites/floor_edge_0', Direction = core.Direction.Down})
	SpawnEntity('Floor', core.Vector(tile * 12, tile * 3), { Texture = 'Sprites/floor_edge_0', Direction = core.Direction.Down})
	SpawnEntity('Floor', core.Vector(tile * 13, tile * 3), { Texture = 'Sprites/floor_edge_0', Direction = core.Direction.Down})
	SpawnEntity('Floor', core.Vector(tile * 14, tile * 3), { Texture = 'Sprites/floor_edge_0', Direction = core.Direction.Down})
	SpawnEntity('Lava64', core.Vector(tile * 11, tile * 4))
	SpawnEntity('Lava64', core.Vector(tile * 12, tile * 4))
	SpawnEntity('Lava64', core.Vector(tile * 13, tile * 4))
	SpawnEntity('Lava64', core.Vector(tile * 14, tile * 4))
	SpawnEntity('Floor', core.Vector(tile * 11, tile * 5), { Texture = 'Sprites/floor_edge_0', Direction = core.Direction.Up})
	SpawnEntity('Floor', core.Vector(tile * 12, tile * 5), { Texture = 'Sprites/floor_edge_0', Direction = core.Direction.Up})
	SpawnEntity('Floor', core.Vector(tile * 13, tile * 5), { Texture = 'Sprites/floor_edge_0', Direction = core.Direction.Up})
	SpawnEntity('Floor', core.Vector(tile * 14, tile * 5), { Texture = 'Sprites/floor_edge_0', Direction = core.Direction.Up})
	SpawnEntity('Lava64', core.Vector(tile * 28, tile * 13))
	SpawnEntity('Lava64', core.Vector(tile * 29, tile * 13))
	SpawnEntity('Lava64', core.Vector(tile * 27, tile * 14))
	SpawnEntity('Lava64', core.Vector(tile * 30, tile * 14))
	SpawnEntity('Lava64', core.Vector(tile * 26, tile * 15))
	SpawnEntity('Lava64', core.Vector(tile * 31, tile * 15))
	SpawnEntity('Lava64', core.Vector(tile * 26, tile * 16))
	SpawnEntity('Lava64', core.Vector(tile * 31, tile * 16))
	SpawnEntity('Lava64', core.Vector(tile * 26, tile * 17))
	SpawnEntity('Lava64', core.Vector(tile * 31, tile * 17))
	SpawnEntity('Lava64', core.Vector(tile * 27, tile * 18))
	SpawnEntity('Lava64', core.Vector(tile * 30, tile * 18))
	SpawnEntity('Lava64', core.Vector(tile * 28, tile * 19))
	SpawnEntity('Lava64', core.Vector(tile * 29, tile * 19))

	SpawnEntity('Teleport', core.Vector(tile * 12, tile * 1), {Direction = core.Direction.Down, Destination = core.Vector(tile * 12, tile * 6)})
	SpawnEntity('Teleport', core.Vector(tile * 22, tile * 24), {Direction = core.Direction.Up, Destination = core.Vector(tile * 12, tile * 6)})

	SpawnEntity('door', core.Vector(tile * 4, tile * 8), {Direction = core.Direction.Down})

	


	SpawnEntity('RoboK9', core.Vector(tile * 10, tile * 11), {Weapon = 'LaserRifle'})
	SpawnEntity('RoboK9', core.Vector(tile * 11, tile * 11), {Weapon = 'LaserRifle'})
	SpawnEntity('RoboK9', core.Vector(tile * 10, tile * 12), {Weapon = 'LaserRifle'})
	SpawnEntity('RoboK9', core.Vector(tile * 11, tile * 12), {Weapon = 'LaserRifle'})

	SpawnEntity('RoboK9', core.Vector(tile * 15, tile * 11), {Weapon = 'LaserRifle'})
	SpawnEntity('RoboK9', core.Vector(tile * 14, tile * 11), {Weapon = 'LaserRifle'})
	SpawnEntity('RoboK9', core.Vector(tile * 15, tile * 12), {Weapon = 'LaserRifle'})
	SpawnEntity('RoboK9', core.Vector(tile * 14, tile * 12), {Weapon = 'LaserRifle'})

	SpawnEntity('RoboK9', core.Vector(tile * 4, tile * 6), {Weapon = 'LaserRifle'})


	SpawnEntity('Orb', core.Vector(tile * 3, tile * 16))
	SpawnEntity('Orb', core.Vector(tile * 4, tile * 16))
	SpawnEntity('Orb', core.Vector(tile * 3, tile * 17))
	SpawnEntity('Orb', core.Vector(tile * 4, tile * 17))
	SpawnEntity('Orb', core.Vector(tile * 3, tile * 18))
	SpawnEntity('Orb', core.Vector(tile * 4, tile * 18))
	SpawnEntity('Orb', core.Vector(tile * 3, tile * 19))
	SpawnEntity('Orb', core.Vector(tile * 4, tile * 19))


	SpawnEntity('RoboK9', core.Vector(tile * 25, tile * 15), {Weapon = 'LaserRifle'})
	SpawnEntity('RoboK9', core.Vector(tile * 25, tile * 17), {Weapon = 'LaserRifle'})
	SpawnEntity('RoboK9', core.Vector(tile * 31, tile * 14), {Weapon = 'LaserRifle'})
	SpawnEntity('RoboK9', core.Vector(tile * 31, tile * 18), {Weapon = 'LaserRifle'})
	SpawnEntity('RoboK9', core.Vector(tile * 33, tile * 16), {Weapon = 'LaserRifle'})

	SpawnEntity('Orb', core.Vector(tile * 26, tile * 13))
	SpawnEntity('Orb', core.Vector(tile * 26, tile * 14))
	SpawnEntity('Orb', core.Vector(tile * 26, tile * 18))
	SpawnEntity('Orb', core.Vector(tile * 26, tile * 19))
end
--Copyright d4, 2017--