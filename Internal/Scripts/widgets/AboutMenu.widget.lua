--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")
ui = require("d4UI")

function RegisterWidget()
	return {
		Level = "AboutMenu",
		Position = { X = 0, Y = 0 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = true
	}
end

function Initialize()
	splashTexture = "Sprites/splashscreen"
	splashStartPos = core.Vector((ScreenWidth / 2) - (TextureSize(splashTexture).X / 2), ScreenHeight - TextureSize(splashTexture).Y)
	splashEndPos = core.Vector(-100, -250)
	splashSpeed = 0.12
	
	textColor = core.Color(70, 200, 82, 255)
	textShadowColor = core.Color(79, 80, 85, 255)
	textShadowOffset = core.Vector(3, 3)

	textBeginOffset = core.Vector(0, 500)
	textEndOffset = core.Vector(300, 500)
	textSpeed = 0.12
	font = 'Fonts/Unispace_regular_16'
	fontHeader = 'Fonts/Unispace_regular_24'
	aboutString = 'This game was is made by d4. The project started 23:24 on 23rd of April, 2017!'
	backButton = ui.CreateButton('BACK', core.Vector(ScreenWidth, ScreenHeight - 30 - TextSize(font, 'BACK').Y), font, core.Vector(ScreenWidth - 30 - TextSize(font, 'BACK').X, ScreenHeight - 30 - TextSize(font, 'BACK').Y))
	
	creditsButton = ui.CreateButton('CREDITS', core.Vector(ScreenWidth, ScreenHeight - 30 - TextSize(font, 'CREDITS').Y), font, core.Vector(ScreenWidth - 100 - TextSize(font, 'CREDITS').X, ScreenHeight - 30 - TextSize(font, 'CREDITS').Y))
		
end

function Update()
	splashStartPos = core.VectorLerp(splashStartPos, splashEndPos, splashSpeed)	
	textBeginOffset = core.VectorLerp(textBeginOffset, textEndOffset, textSpeed)
end

function Draw ()
	DrawTexture(splashTexture, splashStartPos, core.Color(255, 255, 255, 255))

	DrawText(fontHeader, 'About', core.Vector(textBeginOffset.X + textShadowOffset.X, textBeginOffset.Y - 70 + textShadowOffset.Y), textShadowColor, 0.3)
	DrawText(fontHeader, 'About', core.Vector(textBeginOffset.X, textBeginOffset.Y - 70), textColor, 0.31)

	DrawText(font, aboutString, core.Vector(textBeginOffset.X + textShadowOffset.X, textBeginOffset.Y + textShadowOffset.Y), textShadowColor, 0.3)
	DrawText(font, aboutString, textBeginOffset, textColor, 0.31)

	if ui.DrawButton(backButton) then
		ChangeLevel('MainMenu')
	end
	if ui.DrawButton(creditsButton) then
		ChangeLevel('CreditsMenu')
	end
end
--Copyright d4, 2017--