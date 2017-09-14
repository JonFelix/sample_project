--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")
ui = require("d4UI")

function RegisterWidget()
	return {
		Level = "SettingsMenu",
		Position = { X = 0, Y = 0 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = true
	}
end

function Initialize()
	splashTexture = "d4lilah/d4_shattershot_img"
	splashStartPos = core.Vector((ScreenWidth / 2) - (TextureSize(splashTexture).X / 2), ScreenHeight - TextureSize(splashTexture).Y)
	splashEndPos = core.Vector(-100, -250)
	splashSpeed = 0.12
	
	textColor = core.Color(255, 255, 255, 255)
	textShadowColor = core.Color(0, 0, 0, 0)
	textShadowOffset = core.Vector(3, 3)

	textBeginOffset = core.Vector(0, 600)
	textEndOffset = core.Vector(300, 600)
	textSpeed = 0.12
	font = 'Fonts/Unispace_regular_16'
	fontOptions = 'Fonts/Unispace_regular_12'
	fontSubHeader = 'Fonts/Unispace_regular_18'
	fontHeader = 'Fonts/Unispace_regular_24'
	backButton = ui.CreateButton('BACK', core.Vector(ScreenWidth, ScreenHeight - 30 - TextSize(font, 'BACK').Y), font, core.Vector(ScreenWidth - 30 - TextSize(font, 'BACK').X, ScreenHeight - 30 - TextSize(font, 'BACK').Y))
	backButton.TextColor = core.Color(255, 255, 255, 255)
	backButton.ShadowColor = core.Color(0, 0, 0, 0)
	advancedButton = ui.CreateButton('ADVANCED', core.Vector(ScreenWidth, ScreenHeight - 30 - TextSize(font, 'ADVANCED').Y), font, core.Vector(ScreenWidth - 100 - TextSize(font, 'ADVANCED').X, ScreenHeight - 30 - TextSize(font, 'ADVANCED').Y))
	advancedButton.TextColor = core.Color(255, 255, 255, 255)
	advancedButton.ShadowColor = core.Color(0, 0, 0, 0)

	mouseOverColor = core.Color(255, 255, 255, 255)
	horizontalSpacer = 32
	optionSpacer = 250

	secondTextBeginOffset = core.Vector(650 , ScreenHeight)
	secondTextEndOffset = core.Vector(650, 600)

	thirdTextBeginOffset = core.Vector(ScreenWidth , 600)
	thirdTextEndOffset = core.Vector(1000, 600)

	supportedResolutions = Video.supportedResolutions;
	supportedResolutionsText = {}
	selectedResolution = 1
	for i = 1, #supportedResolutions do
		supportedResolutionsText[i] = supportedResolutions[i][1] .. ' x ' .. supportedResolutions[i][2]
		if supportedResolutions[i][1] == ClientSettings.Width and supportedResolutions[i][2] == ClientSettings.Height then
			selectedResolution = i
		end
	end
	resolutionList = ui.CreateListBox(supportedResolutionsText, core.Rect(0, 0, 120, 20), fontOptions, textColor, core.Color(22, 22, 24, 255), mouseOverColor, core.Vector(0, 0))
	resolutionList.ItemIndex = selectedResolution

	masterVolume = 0.5
	musicVolume = 0.5
	ambienceVolume = 0.5
end

function Update()
	splashStartPos = core.VectorLerp(splashStartPos, splashEndPos, splashSpeed)
	textBeginOffset = core.VectorLerp(textBeginOffset, textEndOffset, textSpeed)
	secondTextBeginOffset = core.VectorLerp(secondTextBeginOffset, secondTextEndOffset, textSpeed)
	thirdTextBeginOffset = core.VectorLerp(thirdTextBeginOffset, thirdTextEndOffset, textSpeed)

	if resolutionList.ItemIndex ~= selectedResolution then
		selectedResolution = resolutionList.ItemIndex
		ClientSettings.Width = supportedResolutions[selectedResolution][1]
		ClientSettings.Height = supportedResolutions[selectedResolution][2]
	end
end

function Draw ()
	DrawTexture(splashTexture, splashStartPos, core.Color(255, 255, 255, 255))

	if ui.DrawButton(backButton) then
		ChangeLevel('MainMenu')
	end
	if ui.DrawButton(advancedButton) then
		ChangeLevel('MainMenu')
	end

	DrawText(fontHeader, 'Settings', core.Vector(textBeginOffset.X + textShadowOffset.X, textBeginOffset.Y - 70 + textShadowOffset.Y), textShadowColor, 0.3)
	DrawText(fontHeader, 'Settings', core.Vector(textBeginOffset.X, textBeginOffset.Y - 70), textColor, 0.31)

	------------------
	-- VID SETTINGS --
	------------------
	local pos = core.Vector(core.RemoveExponent(textBeginOffset.X), core.RemoveExponent(textBeginOffset.Y))
	DrawText(fontSubHeader, 'Video Settings', core.Vector(pos.X, pos.Y - 5), textColor)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'Resolution', core.Vector(pos.X, pos.Y), textColor)
	resolutionList.Hitbox = core.Rect(pos.X + optionSpacer + 19 - resolutionList.Hitbox.W, pos.Y, resolutionList.Hitbox.W, resolutionList.Hitbox.H)
	resolutionList = ui.DrawListBox(resolutionList)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'FullScreen', core.Vector(pos.X, pos.Y), textColor)
	ClientSettings.Fullscreen = ui.CheckBox(ClientSettings.Fullscreen, core.Vector(pos.X + optionSpacer, pos.Y), textColor, textColor, mouseOverColor, true)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'Vertical sync', core.Vector(pos.X, pos.Y), textColor)
	ClientSettings.Vsync = ui.CheckBox(ClientSettings.Vsync, core.Vector(pos.X + optionSpacer, pos.Y), textColor, textColor, mouseOverColor, true)
	pos.Y = pos.Y + horizontalSpacer

	------------------
	-- DEV SETTINGS --
	------------------
	pos = core.Vector(core.RemoveExponent(secondTextBeginOffset.X), core.RemoveExponent(secondTextBeginOffset.Y))
	DrawText(fontSubHeader, 'Developer Settings', core.Vector(pos.X , pos.Y - 5), textColor)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'Console enabled', core.Vector(pos.X , pos.Y), textColor)
	ClientSettings.ConsoleEnabled = ui.CheckBox(ClientSettings.ConsoleEnabled, core.Vector(pos.X + optionSpacer, pos.Y), textColor, textColor, mouseOverColor, true)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'Console timestamps', core.Vector(pos.X, pos.Y), textColor)
	ClientSettings.LogTimestamp = ui.CheckBox(ClientSettings.LogTimestamp, core.Vector(pos.X + optionSpacer, pos.Y), textColor, textColor, mouseOverColor, true)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'Log console output', core.Vector(pos.X, pos.Y), textColor)
	ClientSettings.EnableLog = ui.CheckBox(ClientSettings.EnableLog, core.Vector(pos.X + optionSpacer, pos.Y), textColor, textColor, mouseOverColor, true)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'Open log on exit', core.Vector(pos.X, pos.Y), textColor)
	ClientSettings.OpenLogOnClose = ui.CheckBox(ClientSettings.OpenLogOnClose, core.Vector(pos.X + optionSpacer, pos.Y), textColor, textColor, mouseOverColor, ClientSettings.EnableLog)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'Persistent history', core.Vector(pos.X, pos.Y), textColor)
	ClientSettings.ConsolePersistentHistory = ui.CheckBox(ClientSettings.ConsolePersistentHistory, core.Vector(pos.X + optionSpacer, pos.Y), textColor, textColor, mouseOverColor, true)
	pos.Y = pos.Y + horizontalSpacer


	------------------
	-- AUD SETTINGS --
	------------------
	pos = core.Vector(core.RemoveExponent(thirdTextBeginOffset.X), core.RemoveExponent(thirdTextBeginOffset.Y))
	DrawText(fontSubHeader, 'Audio Settings', core.Vector(pos.X , pos.Y - 5), textColor)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'Master Volume', core.Vector(pos.X, pos.Y), textColor)
	masterVolume = ui.Slider(core.Vector(pos.X + optionSpacer, pos.Y), 200, masterVolume, textColor, textColor, mouseOverColor, true)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'Music Volume', core.Vector(pos.X, pos.Y), textColor)
	musicVolume = ui.Slider(core.Vector(pos.X + optionSpacer, pos.Y), 200, musicVolume, textColor, textColor, mouseOverColor, true)
	pos.Y = pos.Y + horizontalSpacer
	DrawText(font, 'Ambience Volume', core.Vector(pos.X, pos.Y), textColor)
	ambienceVolume = ui.Slider(core.Vector(pos.X + optionSpacer, pos.Y), 200, ambienceVolume, textColor, textColor, mouseOverColor, true)
	pos.Y = pos.Y + horizontalSpacer
end
--Copyright d4, 2017--