--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Position = { X = 0.5, Y = 0.3 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = false
	}
end

function Initialize()
	leftArrow = 'd4lilah/sprites/d4_ui_key_arrow_left'
	rightArrow = 'd4lilah/sprites/d4_ui_key_arrow_right'
	jumpArrow = 'd4lilah/sprites/d4_ui_key_arrow_jump'
	player = {}
	color = core.Color(255, 255, 255, 255)
	keycolor = core.Color(255, 0, 0, 255)
end

function Update()
end

function Draw()
	if player.Keys ~= nil then 
		if player.Keys.Left then
			DrawTexture(leftArrow, core.Vector(-48, 0), color)
		end
		if player.Keys.Right then
			DrawTexture(rightArrow, core.Vector(48, 0), color)
		end
		if player.Keys.Jump then
			DrawTexture(jumpArrow, core.Vector(0, -48), color)
		end
	end
	if KeyDown('left') then
		DrawTexture(leftArrow, core.Vector(-24, 0), keycolor)
	end
	if KeyDown('right') then
		DrawTexture(rightArrow, core.Vector(24, 0), keycolor)
	end
	if KeyDown('jump') then
		DrawTexture(jumpArrow, core.Vector(0, -24), keycolor)
	end
end

function OnMessage(information)
	player = information
end
--Copyright d4, 2017--