--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")
function Initialize()
	SetMenuLevel(true)
	NewTimer(core.Time(0, 0, 1, 0), {ChangeLevel = true})
	SetBackColor(core.Color(17, 16, 24, 255))
end

function OnMessage(message)
	ChangeLevel('MainMenu')
end
--Copyright d4, 2017--