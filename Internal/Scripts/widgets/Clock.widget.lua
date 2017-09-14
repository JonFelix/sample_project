--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Position = { X = 1, Y = 1 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = false
	}
end

function SetTime()
	local minutePrefix = ''
	local hourPrefix = ''
	local secondPrefix = ''
	if Time.Second < 10 then
		secondPrefix = '0'
	end
	if Time.Minute < 10 then
		minutePrefix = '0'
	end
	if Time.Hour < 10 then
		hourPrefix = '0'
	end
	text = hourPrefix .. Time.Hour .. ':' .. minutePrefix .. Time.Minute .. ':' .. secondPrefix .. Time.Second
	textSize = TextSize(font, text)
end

function Initialize()
	font = 'Fonts/ConsoleFontSmall'
	text = ''
	textSize = 0
	spacer = 5
	SetTime()
end

function Update()
	SetTime()
end

function Draw ()
	DrawRectangle(core.Rect(-(textSize.X - spacer) - (textSize.X/2), -textSize.Y, textSize.X + (spacer * 2), textSize.Y), core.Color(0, 0, 0, 100))
	DrawText(font, text, core.Vector(-(textSize.X - (spacer * 2)) - (textSize.X/2), -textSize.Y), core.Color(255, 255, 255, 255))
end
--Copyright d4, 2017--