--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Position = { X = 0, Y = 1 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = false
	}
end

function Initialize()
	font = 'Fonts/ConsoleFontSmall'
	text = 'd4lilah: ' .. Version.d4lilah .. ' [' .. Version.Commit .. ']\nLUA: ' .. Version.LUA .. ' MS: ' .. Version.MoonSharp
	size = TextSize(font, text)
	spacer = 5
end

function Draw ()
	DrawRectangle(core.Rect(0, -size.Y, size.X + (spacer * 2), size.Y), core.Color(0, 0, 0, 100), 0.9)
	DrawText(font, text, core.Vector(spacer, -size.Y), core.Color(255, 255, 255, 255), 0.91)
end
--Copyright d4, 2017--