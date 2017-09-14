--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Position = { X = 0.5, Y = 0.5 },
		Origin = { X = -512, Y = -512 },
		Depth = 0,
		MenuWidget = false
	}
end

function Initialize()
	image = 'Sprites/ScreenBloodSplatter_0'
	displayImage = false
	color = core.Color(124, 0, 0, 0)
end

function Update()
	if displayImage then
		color = core.ColorLerp(color, core.Color(124, 0, 0, 255), 0.2)
	else
		color = core.ColorLerp(color, core.Color(124, 0, 0, 0), 1)
	end
end

function Draw()
	if color.A > 0 then
		DrawTexture(image, core.Vector(0, 0), color)
	end
end

function OnMessage(message)
	if message.Event ~= nil then
		if message.Event == 'DisplayBlood' then
			displayImage = true
			NewTimer(core.Time(0, 0, 1, 0), {Event = 'RemoveBlood'})
		end
		if message.Event == 'RemoveBlood' then
			displayImage = false
		end
	end
end
--Copyright d4, 2017--