--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Position = { X = 0.5, Y = 0.3 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = false
	}
end

function Initialize()
	font = 'Fonts/Unispace_regular_16'
	messages = {}
	messageTreshold = 1
	messageTimer = 0
	color = core.Color(255, 255, 255, 255)
	textOffset = core.Vector(0, 0)
end

function Update()
	if messages ~= nil then
		if #messages > 0 then
			textOffset = core.MultiplyVector(TextSize(font, messages[1]), -0.5)
			messageTimer = messageTimer + Time.DeltaTime
			if messageTimer > messageTreshold then
				for i = 2, #messages do
					messages[i - 1] = messages[i]
				end
				messages[#messages] = nil
				messageTimer = 0
			end
		end
	end
end

function Draw()
	if messages ~= nil then
		if #messages > 0 then
			DrawText(font, messages[1], textOffset, color)
		end
	end
end

function OnMessage(message)
	if message.Event ~= nil then
		if message.Event == 'DisplayMessage' then
			messages[#messages + 1] = message.Message
		end
	end
end
--Copyright d4, 2017--