--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")
function Initialize()
	SetMenuLevel(true)
	NewTimer(core.Time(0, 0, 1, 0), {ChangeLevel = true})
end

function OnMessage(message)
	ChangeLevel('MainMenu')
end
--Copyright d4, 2017--