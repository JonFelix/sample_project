--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Level = "Start",
		Position = { X = 0.5, Y = 0.5 },
		Origin = { X = -250, Y = -250 },
		Depth = 0,
		MenuWidget = true
	}
end

function Initialize()
	splashTexture = "test"
	splashBeginColor = core.Color(255, 255, 255, 255)
	splashEndColor = core.Color(0, 0, 0, 255)
	splashTransitionSpeed = 700
	font = 'Fonts/Unispace_regular_16'
	text = 'A game made by'
	textOffset = TextSize(font, text);
end

function Update()
	--splashBeginColor = core.ColorLerp(splashBeginColor, splashEndColor, splashTransitionSpeed * DeltaTime)	
end

function Draw ()
	DrawTexture(splashTexture, core.Vector(0, 0), splashBeginColor)
	DrawText(font, text, core.Vector((500 - textOffset.X) / 2, -textOffset.Y), splashBeginColor)
end
--Copyright d4, 2017--