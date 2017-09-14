--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")
ui = require("d4UI")

function RegisterWidget()
	return {
		Level = "CreditsMenu",
		Position = { X = 0, Y = 0 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = true
	}
end

function Initialize()
	splashTexture = "Sprites/splashscreen"
	splashStartPos =core.Vector(-100, -250) 
	splashEndPos = core.Vector((ScreenWidth / 2) - (TextureSize(splashTexture).X / 2), (ScreenHeight - TextureSize(splashTexture).Y) - 200)
	splashSpeed = 0.12
	credits = {
		{Title = 'Designer', Name = 'Jon Felix Jennemann', Nickname = 'd4'},
		{Title = 'Programmer', Name = 'Jon Felix Jennemann', Nickname = 'd4'},
		{Title = 'Artist', Name = 'Jon Felix Jennemann', Nickname = 'd4'},
		{Title = 'QA', Name = 'Jon Felix Jennemann', Nickname = 'd4'},
		{Title = 'Morale support', Name = 'Catplusplus', Nickname = 'C++'}
	}
	creditsIndex = 1
	creditDuration = core.Time(0, 0, 2, 0)
	textColor = core.Color(70, 200, 82, 255)
	textShadowColor = core.Color(79, 80, 85, 255)
	textShadowOffset = core.Vector(3, 3)
	textTransitionSpeed = 0.12
	
	titleEndOffset = core.Vector((ScreenWidth/2) - 50, (ScreenHeight/ 2) + 100)
	titleCurrentOffset = core.Vector(0, (ScreenHeight/ 2) + 100)

	nicknameEndOffset = core.Vector((ScreenWidth/2) - 50, (ScreenHeight/ 2) + 130)
	nicknameCurrentOffset = core.Vector(ScreenWidth, (ScreenHeight/ 2) + 130)

	nameEndOffset = core.Vector((ScreenWidth/2) - 50, (ScreenHeight/ 2) + 160)
	nameCurrentOffset = core.Vector((ScreenWidth/2) - 50, ScreenHeight)

	font = 'Fonts/Unispace_regular_16'
	fontHeader = 'Fonts/Unispace_regular_24'
	aboutString = 'This game was is made by d4. The project started 23:24 on 23rd of April, 2017!'
	backButton = ui.CreateButton('BACK', core.Vector(ScreenWidth, ScreenHeight - 30 - TextSize(font, 'BACK').Y), font, core.Vector(ScreenWidth - 30 - TextSize(font, 'BACK').X, ScreenHeight - 30 - TextSize(font, 'BACK').Y))
	
	NewTimer(creditDuration, {Event = 'ChangeCredit'})
end

function Update()
	splashStartPos = core.VectorLerp(splashStartPos, splashEndPos, splashSpeed)	

	titleCurrentOffset = core.VectorLerp(titleCurrentOffset, titleEndOffset, textTransitionSpeed)
	nameCurrentOffset = core.VectorLerp(nameCurrentOffset, nameEndOffset, textTransitionSpeed)
	nicknameCurrentOffset = core.VectorLerp(nicknameCurrentOffset, nicknameEndOffset, textTransitionSpeed)
end

function Draw()
	DrawTexture(splashTexture, splashStartPos, core.Color(255, 255, 255, 255))	

	DrawText(font, string.upper(credits[creditsIndex].Title), core.Vector(titleCurrentOffset.X + textShadowOffset.X, titleCurrentOffset.Y + textShadowOffset.Y), textShadowColor, 0.3)
	DrawText(font, '"' .. credits[creditsIndex].Nickname .. '"', core.Vector(nicknameCurrentOffset.X + textShadowOffset.X, nicknameCurrentOffset.Y + textShadowOffset.Y) , textShadowColor, 0.3)
	DrawText(font, credits[creditsIndex].Name, core.Vector(nameCurrentOffset.X  + textShadowOffset.X, nameCurrentOffset.Y  + textShadowOffset.Y), textShadowColor, 0.3)
	DrawText(font, string.upper(credits[creditsIndex].Title), titleCurrentOffset, textColor, 0.31)
	DrawText(font, '"' .. credits[creditsIndex].Nickname .. '"', nicknameCurrentOffset, textColor, 0.31)
	DrawText(font, credits[creditsIndex].Name, nameCurrentOffset, textColor, 0.31)

	if ui.DrawButton(backButton) then
		ChangeLevel('MainMenu')
	end
end

function OnMessage(message)
	if message.Event ~= nil then
		if message.Event == 'ChangeCredit' then
			creditsIndex = creditsIndex + 1
			titleCurrentOffset = core.Vector(0, (ScreenHeight/ 2) + 100)
			nicknameCurrentOffset = core.Vector(ScreenWidth, (ScreenHeight/ 2) + 130)
			nameCurrentOffset = core.Vector((ScreenWidth/2) - 50, ScreenHeight)
			if creditsIndex > #credits then
				creditsIndex = 1
			end
			NewTimer(creditDuration, {Event = 'ChangeCredit'})
		end
	end
end
--Copyright d4, 2017--