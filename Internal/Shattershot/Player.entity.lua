--MOD:Shattershot
--Copyright d4, 2017--
core = require("d4core")
pistol = require("Lasergun")

function GetInput()
	local input = core.Vector(0, 0)
	Player.Keys.Left = true
	Player.Keys.Right = true
	Player.Keys.Up = true
	Player.Keys.Down = true
	if KeyDown('left') then
		input.X = -1
		Player.Keys.Left = true
	end
	if KeyDown('right') then
		input.X = input.X + 1
		Player.Keys.Right = true
	end
	if KeyDown('up') then
		input.Y = -1
		Player.Keys.Up = true
	end
	if KeyDown('down') then
		input.Y = input.Y + 1
		Player.Keys.Down = true
	end
	return input
end

function MovePlayer(input)
	if core.Distance(core.Vector(0, 0), Player.Velocity) < Player.WalkMaxSpeed then
		if input.X ~= 0 then
			Player.Velocity.X = Player.Velocity.X + (Player.Acceleration * input.X * Time.DeltaTime)
		else
			if Player.Velocity.X ~= 0 then
				if core.Abs(Player.Velocity.X) - (Player.Deacceleration * Time.DeltaTime) > 0 then
					Player.Velocity.X = Player.Velocity.X - ((Player.Deacceleration * Time.DeltaTime) * core.Sign(Player.Velocity.X))
				else
					Player.Velocity.X = 0
				end
			end
		end
		if input.Y ~= 0 then
			Player.Velocity.Y = Player.Velocity.Y + (Player.Acceleration * input.Y * Time.DeltaTime)
		else
			if Player.Velocity.Y ~= 0 then
				if core.Abs(Player.Velocity.Y) - (Player.Deacceleration * Time.DeltaTime) > 0 then
					Player.Velocity.Y = Player.Velocity.Y - ((Player.Deacceleration * Time.DeltaTime) * core.Sign(Player.Velocity.Y))
				else
					Player.Velocity.Y = 0
				end
			end
		end
	end
	while #CheckPosition(core.Rect(Entity.Position.X + Player.Velocity.X, Entity.Position.Y, 32, 32), 0, {Entity.ID}) > 0 do
		if core.Abs(Player.Velocity.X) > Player.BounceTreshold then
			Player.Velocity.X = Player.Velocity.X * -0.3
			break
		end
		if core.Abs(Player.Velocity.X) < 1 then
			Player.Velocity.X = 0
			break
		end
		Player.Velocity.X = Player.Velocity.X - core.Sign(Player.Velocity.X)
	end
	while #CheckPosition(core.Rect(Entity.Position.X, Entity.Position.Y + Player.Velocity.Y, 32, 32), 0, {Entity.ID}) > 0 do
		if core.Abs(Player.Velocity.Y) > Player.BounceTreshold then
			Player.Velocity.Y = Player.Velocity.Y * -0.3
			break
		end
		if core.Abs(Player.Velocity.Y) < 1 then
			Player.Velocity.Y = 0
			break
		end
		Player.Velocity.Y = Player.Velocity.Y - core.Sign(Player.Velocity.Y)
	end
end

function LookAtMouse()
	local dir = {
		X = MousePosition.X - Entity.Position.X,
		Y = MousePosition.Y - Entity.Position.Y
	}
	Entity.Direction = core.VectorToDirection(dir)
end

function UpdateInterface()
	SendMessageToWidget('VelocityMeter', Player)
end

function UpdateWeapons() 
	Player.Weapons[Player.SelectedWeapon].Fire(Player.Weapons[Player.SelectedWeapon])
	for i = 1, #Player.Weapons do
		Player.Weapons[i].Update(Player.Weapons[Player.SelectedWeapon])
	end
end

function Initialize(params)
	Player = {
		Keys = {
			Left = false,
			Right = false,
			Up = false,
			Down = false
		},
		Velocity = core.Vector(0, 0),
		Acceleration = 10,
		Deacceleration = 10,
		WalkMaxSpeed = 10,
		BounceTreshold = 8,
		Position = core.Vector(Entity.Position.X, Entity.Position.Y),
		Camera = {
			Origin = core.Vector(Entity.Position.X, Entity.Position.Y),
			Position = core.Vector(Entity.Position.X, Entity.Position.Y)
		},
		Weapons = {
			pistol
		},
		SelectedWeapon = 1
	}
	SetCameraOrigin(core.Vector((ScreenWidth / 2), (ScreenHeight / 2)))
end

function Update()
	MovePlayer(GetInput())
	LookAtMouse()

	SetCamera(Player.Camera.Position, 1, 0)
	Move(Player.Velocity)
	UpdateInterface()
	UpdateWeapons()
end

function Draw()
	DrawRectangle(core.Rect(Entity.Position.X - 16, Entity.Position.Y - 16, 32, 32), core.Color(0, 0, 0, 255))
end

function OnMessage(message)
	
end
--Copyright d4, 2017--