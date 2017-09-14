--MOD:Shattershot
--Copyright d4, 2017--
core = require("d4core")

function Initialize(params)
	Entity.Direction = params.Direction
	parent = params.Parent
	damage = params.Damage
	speed = 500
	linePoints = {}
end

function Update()
	linePoints = {}
	local distance = speed * Time.DeltaTime
	local dir = core.DirectionToVector(Entity.Direction)
	local endpoint = core.Vector(Entity.Position.X + dir.X * distance, Entity.Position.Y + dir.Y * distance)
	local colls = Raycast(core.Vector(Entity.Position.X, Entity.Position.Y), endpoint)
	local collsPos = core.Vector(0, 0)
	linePoints[#linePoints + 1] = core.Vector(Entity.Position.X, Entity.Position.Y)
	for i = 1, #colls do
		local ent = GetEntity(colls[i].ID)
		if ent.Name == 'Cube.entity.lua' then
			collsPos = core.Vector(ent.Position.X, ent.Position.Y)
			linePoints[#linePoints + 1] = collsPos
			SendMessage(colls[1].ID, {Event = 'Hit', Weapon = 'Laser', Source = parent, Damage = damage, Position = colls[i].Position})
			break
		end
	end
	if collsPos.X == 64 * 10 or collsPos.X == 64 * 19 then
		dir.X = dir.X * -1
	end
	if collsPos.Y == 64 * 10 or collsPos.Y == 64 * 19 then
		dir.Y = dir.Y * -1
	end
	Entity.Direction = core.VectorToDirection(dir)
	if collsPos.X ~= 0 or collsPos.Y ~= 0 then
		distance = distance - core.Distance(core.Vector(Entity.Position.X, Entity.Position.Y), collsPos)
		local newPos = core.Vector(collsPos.X + dir.X * distance, collsPos.Y + dir.Y * distance)
		
	else
		Move(core.MultiplyVector(dir, distance))
	end
	linePoints[#linePoints + 1] = core.Vector(Entity.Position.X, Entity.Position.Y)
end

function Draw()
	for i = 2, #linePoints do
		DrawLine(linePoints[i - 1], linePoints[i], core.Color(255, 255, 255, 255), 3)
	end
end

function OnMessage(message)
	
end
--Copyright d4, 2017--