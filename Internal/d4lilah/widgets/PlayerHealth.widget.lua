--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Position = { X = 0, Y = 0.8 },
		Origin = { X = 120, Y = 0 },
		Depth = 0,
		MenuWidget = false
	}
end

function Initialize()
	font = "Fonts/Unispace_regular_20"
	player = {}
	healthGoodColor = core.Color(0, 155, 0, 255)
	healthNormalColor = core.Color(239, 239, 124, 255)
	healthCriticalColor = core.Color(255, 94, 94, 255)
	currentHealthColor = healthGoodColor
	playerHealth = 1
	playerTrueHealth = 100
	playerHealthTransitionSpeed = 1;
	textColor = core.Color(255, 255, 255, 255)
	backColor = core.Color(0, 0, 0, 100)
	textHealthOffset = {}
	playerArmor = 1
	playerTrueArmor = 100
	textArmorOffset = {}
	armorColor = core.Color(0, 149, 149, 255)
end

function Update()
	if player.Velocity ~= nil then
		playerHealth = core.Lerp(playerHealth, player.Health / player.MaxHealth, playerHealthTransitionSpeed * 10)
		playerTrueHealth = core.Lerp(playerTrueHealth, player.Health, playerHealthTransitionSpeed)
		textHealthOffset = TextSize(font, string.format("%d", playerTrueHealth))
		playerArmor = core.Lerp(playerArmor, player.Armor / player.MaxArmor, playerHealthTransitionSpeed * 10)
		playerTrueArmor = core.Lerp(playerTrueArmor, player.Armor, playerHealthTransitionSpeed)
		textArmorOffset = TextSize(font, string.format("%d", playerTrueArmor))
		if playerHealth > 0.74 then
			currentHealthColor = core.ColorLerp(currentHealthColor, healthGoodColor, playerHealthTransitionSpeed)
		end
		if playerHealth < 0.75 and playerHealth > 0.32 then
			currentHealthColor = core.ColorLerp(currentHealthColor, healthNormalColor, playerHealthTransitionSpeed)
		end
		if playerHealth < 0.33 then
			currentHealthColor = core.ColorLerp(currentHealthColor, healthCriticalColor, playerHealthTransitionSpeed)
		end
	end
end

function Draw()
	if player.Velocity ~= nil then 
		DrawRectangle(core.Rect(0, 0, 300, 30), backColor, 0.68)
		DrawRectangle(core.Rect(0, 0, 300 * playerHealth, 30), currentHealthColor, 0.69)
		DrawText(font, string.format("%d", playerTrueHealth), core.Vector(150 - (textHealthOffset.X / 2), (30 - textHealthOffset.Y) / 3), textColor, 0.7)
		DrawLine(core.Vector(0, -1), core.Vector(300, -1), textColor)
		DrawLine(core.Vector(0, 30), core.Vector(300, 30), textColor)
		DrawLine(core.Vector(0, -1), core.Vector(0, 30), textColor)
		DrawLine(core.Vector(301, -1), core.Vector(301, 30), textColor)
		DrawRectangle(core.Rect(0, 50, 300, 30), backColor, 0.68)
		DrawRectangle(core.Rect(0, 50, 300 * playerArmor, 30), armorColor, 0.69)
		DrawText(font, string.format("%d", playerTrueArmor), core.Vector(150 - (textArmorOffset.X / 2), 50 + ((30 - textArmorOffset.Y) / 3)), textColor, 0.7)
		DrawLine(core.Vector(-1, 49), core.Vector(300, 49), textColor)
		DrawLine(core.Vector(0, 80), core.Vector(300, 80), textColor)
		DrawLine(core.Vector(0, 50), core.Vector(0, 80), textColor)
		DrawLine(core.Vector(301, 50), core.Vector(301, 80), textColor)
	end
end

function OnMessage(information)
	player = information
end
--Copyright d4, 2017--