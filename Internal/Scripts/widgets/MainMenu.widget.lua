--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Level = "MainMenu",
		Position = { X = 0, Y = 0 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = true
	}
end

function Initialize()
	splashTexture = "Sprites/splashscreen"
	splashBeginColor = core.Color(0, 0, 0, 255)
	splashEndColor = core.Color(255, 255, 255, 255)
	splashTransitionSpeed = 0.12
	splashPosition = core.Vector((ScreenWidth / 2) - (TextureSize(splashTexture).X / 2), ScreenHeight - TextureSize(splashTexture).Y)
	font = "Fonts/Unispace_regular_16"
	selectorMaxSize = 0
	selectorSize = 0
	SelectorSpeed = 0.12
	isSelectingIndex = 0
	textColor = core.Color(70, 200, 82, 255)
	textShadowColor = core.Color(79, 80, 85, 255)
	textTransitionSpeed = 0.12
	textStartPosition = core.Vector((ScreenWidth / 2) - 400, (ScreenHeight / 2) + 200)
	textShadowOffset = core.Vector(3, 3)
	textSelectOffset = core.Vector(0, 0)
	textSelectOffsetEnd = core.Vector(3, 3)
	textSelectOffsetSpeed = 0.12
	textSpacing = 50
	textSeparator = '|'
	textItems = {
		{
			Text = 'PLAY',
			Hitbox = core.Rect(0, 0, TextSize(font, 'PLAY').X + (textSpacing * 2), TextSize(font, 'PLAY').Y),
			Level = 'levelOne',
			Selected = false
		},
		{
			Text = 'SETTINGS',
			Hitbox = core.Rect(0, 0, TextSize(font, 'SETTINGS').X + (textSpacing * 2), TextSize(font, 'SETTINGS').Y),
			Level = 'SettingsMenu',
			Selected = false
		},
		{
			Text = 'ABOUT',
			Hitbox = core.Rect(0, 0, TextSize(font, 'ABOUT').X + (textSpacing * 2), TextSize(font, 'ABOUT').Y),
			Level = 'AboutMenu',
			Selected = false
		},
		{
			Text = 'QUIT',
			Hitbox = core.Rect(0, 0, TextSize(font, 'QUIT').X + (textSpacing * 2), TextSize(font, 'QUIT').Y),
			Level = '',
			Selected = false
		}
	}
end

function Update()
	splashBeginColor = core.ColorLerp(splashBeginColor, splashEndColor, splashTransitionSpeed)
	local textX = textStartPosition.X
	local isSelecting = false
	for i = 1, #textItems do
		textItems[i].Selected = false
		if MouseInRect(core.Rect(textX + textItems[i].Hitbox.X, textStartPosition.Y + textItems[i].Hitbox.Y, textItems[i].Hitbox.W, textItems[i].Hitbox.H)) then
			if isSelectingIndex ~= i then
				isSelectingIndex = i
				selectorSize = 0
			end
			
			isSelecting = true
			textItems[i].Selected = true
			if selectorMaxSize ~= textItems[i].Hitbox.W then
				selectorMaxSize = textItems[i].Hitbox.W
			end
			selectorSize = core.Lerp(selectorSize, selectorMaxSize, SelectorSpeed)
			textSelectOffset = core.Vector(core.Lerp(textSelectOffset.X, textSelectOffsetEnd.X, textSelectOffsetSpeed * DeltaTime), core.Lerp(textSelectOffset.Y, textSelectOffsetEnd.Y, textSelectOffsetSpeed))
			if KeyPressed('menuclick') then 
				ChangeLevel(textItems[i].Level)
			end
		end
		textX = textX + textItems[i].Hitbox.W + TextSize(font, textSeparator).X
	end
	if isSelecting == false then
		selectorMaxSize = 0
		selectorSize = 0
		isSelectingIndex = 0
		textSelectOffset = core.Vector(0, 0)
	end
end

function Draw ()
	DrawTexture(splashTexture, splashPosition, splashBeginColor)
	local textX = textStartPosition.X
	for i = 1, #textItems do
		
		if textItems[i].Selected then
			DrawText(font, textItems[i].Text, core.Vector(textX + textSpacing + textShadowOffset.X + textSelectOffset.X, textStartPosition.Y + textShadowOffset.Y + textSelectOffset.Y), textShadowColor, 0.3)
			DrawText(font, textItems[i].Text, core.Vector(textX + textSpacing - textSelectOffset.X, textStartPosition.Y - textSelectOffset.Y), textColor, 0.31)

			DrawRectangle(core.Rect(textX + (textItems[i].Hitbox.W / 2)  + textShadowOffset.X - ( selectorSize /2), textStartPosition.Y + textItems[i].Hitbox.H  + textShadowOffset.Y,  selectorSize, 2), textShadowColor, 0.3)
			DrawRectangle(core.Rect(textX + (textItems[i].Hitbox.W / 2) - ( selectorSize /2), textStartPosition.Y + textItems[i].Hitbox.H,  selectorSize, 2), textColor, 0.31)
		else
			DrawText(font, textItems[i].Text, core.Vector(textX + textSpacing + textShadowOffset.X, textStartPosition.Y + textShadowOffset.Y), textShadowColor, 0.3)
			DrawText(font, textItems[i].Text, core.Vector(textX + textSpacing, textStartPosition.Y), textColor, 0.31)
		end
		textX = textX + textItems[i].Hitbox.W
		if i < #textItems then
			DrawText(font, textSeparator, core.Vector(textX + textShadowOffset.X, textStartPosition.Y + textShadowOffset.Y), textShadowColor, 0.3)
			DrawText(font, textSeparator, core.Vector(textX, textStartPosition.Y), textColor, 0.31)
			textX = textX + TextSize(font, textSeparator).X
		end
	end
end
--Copyright d4, 2017--