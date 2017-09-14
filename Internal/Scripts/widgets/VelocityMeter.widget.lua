--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Position = { X = 0.5, Y = 0.7 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = false
	}
end

function Initialize()
	font = "Fonts/Unispace_regular_16"
	player = {}
	color = core.Color(255, 255, 255, 255)
	textPrefix = ''
	text = ''
	textOffset = core.Vector(0,0)
end

function Update()
	if player.Velocity ~= nil then
		text = textPrefix .. string.format("%d", core.Distance(core.Vector(0, 0), player.Velocity) * 10)
		textOffset = TextSize(font, text)
		textOffset.X = (textOffset.X / 2) * -1
		textOffset.Y = (textOffset.Y / 2) * -1
	end
end

function Draw()
	if player.Velocity ~= nil then 
		DrawText(font, text, core.Vector(textOffset.X, textOffset.Y), color)
	end
end

function OnMessage(information)
	player = information
end
--Copyright d4, 2017--