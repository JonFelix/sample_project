--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Position = { X = 0, Y = 0 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = false
	}
end

function Initialize()
	color = core.Color(255, 255, 255, 255)
	crosshair = 'Sprites/gamecursor'
	crosshairColor = core.Color(0, 255, 0, 255)
	ClientSettings.ShowMouse = false
end

function Draw()
	mousepos = MousePosition
	DrawTexture(crosshair, core.Vector(mousepos.X-16, mousepos.Y-16), crosshairColor)
end
--Copyright d4, 2017--