--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function Initialize(params)
	SetTexture(params.Texture)
	Entity.IsCollidable = false
	Entity.Direction = params.Direction
	if params.Depth ~= nil then
		Entity.Depth = params.Depth
	else
		Entity.Depth = 0.3
	end
end
--Copyright d4, 2017--