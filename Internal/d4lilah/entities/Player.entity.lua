--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")
pistol = require("Pistol")
rocketlauncher = require("RocketLauncher") 
grenadelauncher = require("GrenadeLauncher")
blaster = require("Blaster")
shotgun = require("Shotgun")
----------------------
--     MOVEMENT     --
----------------------
function GetInput()
	if Console.Open then return input end
	local input = {
		X = 0,
		Y = 0
	}
	player.Keys.Left = false
	player.Keys.Right = false
	player.Keys.Jump = false
	if player.Grounded then
		if KeyDown('left') then
			player.Keys.Left = true
			input.X = input.X - 1
		end
		if KeyDown('right') then
		player.Keys.Right = true
			input.X = input.X + 1
		end
		if KeyDown('jump') then
			player.Keys.Jump = true
			player.Velocity.Y = player.Velocity.Y - player.JumpStrength
			if core.Sign(player.Velocity.X) < 12 then
				player.Velocity.X = player.Velocity.X + input.X
			end
		end
	end
	return input
end

function SetGrouneded()
	 player.Grounded = #CheckPosition(core.Rect(Entity.Position.X, Entity.Position.Y + 1, Entity.Texture.Width, Entity.Texture.Height), 0, {Entity.ID}) > 0
end
function SetGravitationalPull()
	if player.Grounded == false then
		player.Velocity.Y = player.Velocity.Y + (player.GravitationalPull * Time.DeltaTime)
	end
end

function SetDir()
	if Console.Open then return end
	local dir = {
		X = MousePosition.X - Entity.Position.X,
		Y = MousePosition.Y - Entity.Position.Y
	}
	Entity.Direction = core.VectorToDirection(player.Look.GeneralDirection)
	player.Look.Vector = dir
	player.Look.Direction = core.VectorToDirection(dir)
end

function MovePlayer(input)
	if Console.Open then return end
	if player.IsTeleporting then return end
	if player.Grounded then
		if player.Velocity.X > 0 then
			player.Velocity.X = core.Max(0, player.Velocity.X - (player.Deacceleration * Time.DeltaTime))
		end
		if player.Velocity.X < 0 then
			player.Velocity.X = core.Min(0, player.Velocity.X + (player.Deacceleration * Time.DeltaTime))
		end
	end

	local vel = core.Vector(0, 0)
	if input.X > 0 then
		vel.X = (player.Acceleration * Time.DeltaTime)
	end
	if input.X < 0 then
		vel.X = -(player.Acceleration * Time.DeltaTime)
	end
	if input.Y > 0 then
		vel.Y = (player.Acceleration * Time.DeltaTime)
	end
	if input.Y < 0 then
		vel.Y = -(player.Acceleration * Time.DeltaTime)
	end

	if core.Distance(core.Vector(0,0), player.Velocity) < player.MaxWalkSpeed then
		if core.Distance(core.Vector(0,0), core.Vector(player.Velocity.X + vel.X, player.Velocity.Y + vel.Y)) > player.MaxWalkSpeed then
			local dir = core.NormalizeVector(player.Velocity)
			player.Velocity = core.MultiplyVector(dir, player.MaxWalkSpeed)
		else
			player.Velocity = core.JoinVectors(player.Velocity, vel)
		end
	end

	local appliedSpeed = {
		X = player.Velocity.X,
		Y = player.Velocity.Y
	}
	while #CheckPosition(core.Rect(Entity.Position.X + appliedSpeed.X, Entity.Position.Y, Entity.Texture.Width, Entity.Texture.Height), 0, {Entity.ID}) > 0 do
		player.Velocity.X = 1 * core.Sign(player.Velocity.X)
		if core.Abs(appliedSpeed.X) < 1 then
			appliedSpeed.X = 0
			break
		end
		appliedSpeed.X = appliedSpeed.X - core.Sign(appliedSpeed.X)
	end
	Move(core.Vector(appliedSpeed.X, 0))
	while #CheckPosition(core.Rect(Entity.Position.X, Entity.Position.Y + appliedSpeed.Y, Entity.Texture.Width, Entity.Texture.Height), 0, {Entity.ID}) > 0 do
		player.Velocity.Y = 0
		if core.Abs(appliedSpeed.Y) < 1 then
			appliedSpeed.Y = 0
			break
		end
		appliedSpeed.Y = appliedSpeed.Y - core.Sign(appliedSpeed.Y)
	end
	Move(core.Vector(0, appliedSpeed.Y))
	player.Position = core.Vector(Entity.Position.X, Entity.Position.Y)
	player.CameraPosition = core.JoinVectors(player.CameraPosition, appliedSpeed)
	player.State = playerState.Idle
	if appliedSpeed.X  ~= 0 then
		player.State = playerState.Moving
	end
	if appliedSpeed.Y  ~= 0 then
		player.State = playerState.Moving
	end
end

----------------------
--      TEXTURE     --
----------------------
function UpdateTexture()
	if Console.Open then return input end
	if player.State == playerState.Idle then
		player.MoveTextureIndex = 1
	end
	if player.State == playerState.Moving then
		if player.Look.GeneralDirection.X == 1 then
			player.MoveForwardTextureIndex = player.MoveForwardTextureIndex + 1
			if player.MoveForwardTextureIndex > #player.MoveForwardTexture then
				player.MoveForwardTextureIndex = 1
			end
		else
			player.MoveBackTextureIndex = player.MoveBackTextureIndex + 1
			if player.MoveBackTextureIndex > #player.MoveBackTexture then
				player.MoveBackTextureIndex = 1
			end
		end
	end
end

----------------------
--      WEAPONS     --
----------------------
function SelectWeapon() 
	if KeyPressed('nextweapon') then
		player.SelectedWeapon = player.SelectedWeapon + 1
		if player.SelectedWeapon > #player.Weapons then
			player.SelectedWeapon = 1
		end
	end
	if KeyPressed('prevweapon') then
		player.SelectedWeapon = player.SelectedWeapon - 1
		if player.SelectedWeapon < 1 then
			player.SelectedWeapon = #player.Weapons
		end
	end
	for i = 1, #player.Weapons do
		if KeyPressed(player.Weapons[i].Key) then
			player.SelectedWeapon = i
		end
	end
end

----------------------
--    INTERFACE     --
----------------------
function UpdateInterface()
	SendMessageToWidget("PlayerInfo", player)
	SendMessageToWidget('WeaponRack', player)
	SendMessageToWidget('VelocityMeter', player)
	SendMessageToWidget('PlayerHealth', player)
	SendMessageToWidget('Keys', player)
end

----------------------
--   TELEPORTATION  --
----------------------
function CheckTeleportationDisc()
	if player.TeleportationDiscID == nil then
		return false
	end
	return true
end

function TeleportToDisc()
	if KeyPressed('playerteleport') then
		if CheckTeleportationDisc() then
			--teleport logic
		else
			local offset = core.DirectionToVector(Entity.Direction)
			offset = core.MultiplyVector(offset, 32)
			local teleportationparams = {
				direction = Entity.Direction,
				PlayerID = Entity.ID
			}
			player.TeleportationDiscID = SpawnEntity('teleportdisc', core.JoinVectors(Entity.Position, offset), teleportationparams)
		end
	end
end

function Teleporter()
	if player.IsTeleporting then
		if core.Distance(core.Vector(Entity.Position.X, Entity.Position.Y), player.TeleportDestination) < 3 then
			player.IsTeleporting = false
			player.TeleportDestination = {}
			player.TeleportSpeed = 0
			teleportParticleInfo.Position = core.Vector(Entity.Position.X, Entity.Position.Y)
			SpawnParticles(teleportParticleInfo)
		else
			Entity.IsCollidable = false
			movePos = core.VectorLerp(core.Vector(Entity.Position.X, Entity.Position.Y), player.TeleportDestination, player.TeleportSpeed)
			Entity.Position.X = movePos.X
			Entity.Position.Y = movePos.Y
			player.CameraPosition = core.Vector(movePos.X, movePos.Y)
		end
	else
		Entity.IsCollidable = true
	end
end
----------------------
--      EVENTS      --
----------------------
function Initialize(params)
	--SetShader('Shaders/default')
	Log('Player ID: ' .. Entity.ID)
	playerState = {
		Idle = 0,
		Moving = 1
	}
	player = {
		Health = 100,
		MaxHealth = 100,
		Armor = 100,
		MaxArmor = 100,
		State = 0,
		IsTeleporting = false,
		TeleportSpeed = 0,
		TeleportDestination = {},
		TeleportationDiscID = nil,
		TextureSpeed = core.Time(0, 0, 0, 80),
		StartPosition = Entity.Position,
		CameraPosition = Entity.Position,
		Position = Entity.Position,
		Acceleration = 30,
		JumpStrength = 6,
		Grounded = false,
		GravitationalPull = 12,
		Deacceleration = 15,
		MaxWalkSpeed = 10,
		Velocity = core.Vector(0, 0),
		Look = {
			Vector = core.Vector(0, 0),
			Direction = 0,
			GeneralDirection = core.Vector(1, 0)
		},
		IdleTexture = 'd4lilah/sprites/d4_player_idle_0',
		MoveForwardTexture = {
			'd4lilah/sprites/d4_player_run_forward_0',
			'd4lilah/sprites/d4_player_run_forward_1',
			'd4lilah/sprites/d4_player_run_forward_2',
			'd4lilah/sprites/d4_player_run_forward_3'
			},
		MoveForwardTextureIndex = 1,
		MoveBackTexture = {
			'd4lilah/sprites/d4_player_run_back_0',
			'd4lilah/sprites/d4_player_run_back_1',
			'd4lilah/sprites/d4_player_run_back_2',
			'd4lilah/sprites/d4_player_run_back_3'
			},
		MoveBackTextureIndex = 1,
		SelectedWeapon = 1,
		Keys = {
			Left = false,
			Right = false,
			Jump = false
		},
		Weapons = {
			pistol,
			blaster,
			rocketlauncher,
			shotgun,
			grenadelauncher
		}
	}
	teleportParticleInfo = {
		AmountMinimum = 25,
		AmountMaximum = 75,
		DirectionMinimum = 0,
		DirectionMaximum = math.pi * 2,
		SpeedMinimum = 75,
		SpeedMaximum = 125,
		ScaleMinimum = 0.2,
		ScaleMaximum = 0.4,
		RotationSpeedMaximum = 0,
		RotationSpeedMinimum = 0,
		LifeMinimum = 0.3,
		LifeMaximum = 0.7,
		Position = {
			X = Entity.Position.X, 
			Y = Entity.Position.Y
			},
		Sprites = {
			core.PARTICLE_CIRCLE
		},
		Colors = {
			core.Color(97, 175, 239, 255)
		},
		Depth = 0.5
	}
	SetTexture(player.IdleTexture)
	Entity.IsCollidable = true
	Entity.Depth = 0.5
	SetCameraOrigin(core.Vector((ScreenWidth / 2) - (Entity.Texture.Width/2), (ScreenHeight / 2)- (Entity.Texture.Height/2)))
	NewTimer(player.TextureSpeed, {UpdateTexture = true})
	Log("Player initialized!")
	Log(player)
	SetTable('player', player)
	Log(GetTable('player'))
	Log(Level.CurrentLevel.Name)
end

function Update()
	if Console.Open == false  then
		SetGrouneded();
		SetGravitationalPull();
		MovePlayer(GetInput())
		SetDir()
		SelectWeapon()
		player.Weapons[player.SelectedWeapon].Fire(player.Weapons[player.SelectedWeapon], player.Look.Direction)
		for i = 1, #player.Weapons do
			player.Weapons[i].Update(player.Weapons[i])
		end
	end
	TeleportToDisc()
	Teleporter()
	SetCamera(core.Vector(player.CameraPosition.X, player.CameraPosition.Y), 2, 0)
	if player.State == playerState.Idle and player.IsTeleporting == false then
		SetTexture(player.IdleTexture)
	end
	if player.State == playerState.Moving and player.IsTeleporting == false then
		if player.Look.GeneralDirection.X == 1 then
			SetTexture(player.MoveForwardTexture[player.MoveForwardTextureIndex])
		else
			SetTexture(player.MoveBackTexture[player.MoveBackTextureIndex])
		end
	end
	if player.IsTeleporting then
		SetTexture("")
	end
	UpdateInterface()
end

function Draw()
	if player.Weapons[player.SelectedWeapon].Sprite ~= nil then
		local pos = core.DirectionToVector(Entity.Direction)
		pos.X = Entity.Position.X + (pos.X * player.Weapons[player.SelectedWeapon].SpriteOffset.X)
		pos.Y = Entity.Position.Y + (pos.Y * player.Weapons[player.SelectedWeapon].SpriteOffset.Y)
		DrawTexture(player.Weapons[player.SelectedWeapon].Sprite, pos, Entity.Direction, core.Color(255, 255, 255, 255), Entity.Depth - 0.01)
	end
end

function OnMessage(message)
	if message.UpdateTexture ~= nil then
		UpdateTexture()
		NewTimer(player.TextureSpeed, {UpdateTexture = true})
	end
	if message.BORDERPASSED ~= nil then
		if message.Offset.X < 0 then
			if player.Velocity.X < 0 then
				player.Velocity.X = 0
			end
		end
		if message.Offset.X > 0 then
			if player.Velocity.X > 0 then
				player.Velocity.X = 0
			end
		end
		if message.Offset.Y < 0 then
			if player.Velocity.Y < 0 then
				player.Velocity.Y = 0
			end
		end
		if message.Offset.Y > 0 then
			if player.Velocity.Y > 0 then
				player.Velocity.Y = 0
			end
		end
	end
	if message.Event ~= nil then
		if message.Event == 'Hit' then
			if message.Weapon == 'RocketLauncher' then
				dir = {}
				dir.X = Entity.Position.X - message.Position.X
				dir.Y = Entity.Position.Y - message.Position.Y
				dir = core.NormalizeVector(dir)
				player.Velocity = core.JoinVectors(player.Velocity, core.MultiplyVector(dir, 16))
			end
			if message.Weapon == 'RocketBlast' then
				dir = {}
				dir.X = Entity.Position.X - message.Position.X
				dir.Y = Entity.Position.Y - message.Position.Y
				dir = core.NormalizeVector(dir)
				player.Velocity = core.JoinVectors(player.Velocity, core.MultiplyVector(dir, 10 * message.DamageRatio))
			end
			if message.Weapon == 'Blaster' then
				dir = {}
				dir.X = Entity.Position.X - message.Position.X
				dir.Y = Entity.Position.Y - message.Position.Y
				dir = core.NormalizeVector(dir)
				player.Velocity = core.JoinVectors(player.Velocity, core.MultiplyVector(dir, 4 * message.DamageRatio))
			end
		end
		if message.Event == 'EntityEnteredTeleporter' then
			player.IsTeleporting = true
			player.TeleportSpeed = message.Speed
			player.TeleportDestination = message.Position
			player.Velocity = core.Vector(0, 0)
			teleportParticleInfo.Position = core.Vector(Entity.Position.X, Entity.Position.Y)
			SpawnParticles(teleportParticleInfo)
		end
		if message.Event == 'DealDamage' then
			if message.Source == 'Lava' then
				if core.Distance(core.Vector(0,0), player.Velocity) < 10.1 then
					player.Health = core.Max(player.Health - message.Damage, 0)
				end
			end
		end
	end
	if message.TPDiscDied ~= nil then
		player.TeleportationDiscID = nil
	end
end

function End()

end
--Copyright d4, 2017--