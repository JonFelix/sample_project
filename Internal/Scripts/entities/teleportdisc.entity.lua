--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function ReflectVelocity()
	local dir = core.DirectionToVector(Entity.Direction)
	local dot = core.VectorDot(core.Vector(dir.X * Speed, dir.Y * Speed), dir) * 2
	local newVec = core.Vector(dir.X * dot, dir.Y * dot)
	newVec.X = newVec.X - (dir.X * Speed)
	newVec.Y = newVec.Y - (dir.Y * Speed)
	return (core.VectorToDirection(newVec) * -1)
end

function Initialize(params)
	SetTexture("Sprites/teleportationdisc")
	Entity.IsCollidable = false
	Entity.Direction = params.direction
	Speed = 500
	PlayerID = params.PlayerID
	NewTimer(core.Time(0, 0, 1, 0), {TTK = true})
end

function Update()
	local dir = core.DirectionToVector(Entity.Direction)
	if #CheckPosition(core.Rect(Entity.Position.X + (dir.X * Speed * Time.DeltaTime), Entity.Position.Y + (dir.Y * Speed * Time.DeltaTime), Entity.Texture.Width, Entity.Texture.Height)) > 0 then
		Entity.Direction = ReflectVelocity()
	else
		Move(core.Vector(dir.X * Speed *Time.DeltaTime, dir.Y * Speed * Time.DeltaTime))
	end
end

function End()

end

function OnMessage(message)
	if message.BORDERPASSED ~= nil then
	end
	if message.TTK ~= nil then
		SendMessage(PlayerID, { TPDiscDied = true })
		RemoveEntity(Entity.ID)
	end
end
--Copyright d4, 2017--