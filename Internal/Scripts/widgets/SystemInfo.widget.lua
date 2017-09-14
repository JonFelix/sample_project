--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Position = { X = 0, Y = 0 },
		Origin = { X = 0, Y = 0 },
		Depth = 0
	}
end

function Initialize()
	text = "This is a test, written by a neat widget!"
	text2 = ""
	font = "Fonts/ConsoleFontSmall"
	width = ScreenWidth;
	textheight = 32
	maxCount = 10
	player = GetEntitiesByScript("Player.entity.lua")
	player = player[1]
	
end

function Update()
	text = "FPS: " .. Performance.FPS
	text = text .. " | Delta: ".. Performance.DeltaTime
	text = text .. " | Ents: " .. Performance.EntityCount
	text = text .. " | DrawCount: " .. Performance.DrawCount
	text = text .. " | Sprites: " .. Performance.SpriteCount
	text = text .. " | ParticleCount: " .. Performance.ParticleCount
	text = text .. " | DecalCount: " .. Performance.DecalCount
	text = text .. " | CollisionCount: " .. Performance.CollisionCount
	text = text .. " | ProcessTime: " .. string.format('%d', Performance.ProcessorTime) .. "ms"
	text = text .. " | Memory: " .. string.format('%d', Performance.AllocatedByteCount / 1024) .. "B"
	text2 = LogStack[LogStackCount]
end

function Draw ()
	DrawRectangle(core.Rect(0, 0, width, textheight * 2), core.Color(0, 0, 0, 100), 0.8)
	DrawText (font, text, core.Vector(10, 10), core.Color(255, 255, 255, 255), 0.81)
	DrawText (font, text2, core.Vector(10, 42), core.Color(255, 255, 255, 255), 0.81)
	
end
--Copyright d4, 2017--