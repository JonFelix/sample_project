--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")

function Initialize(params)
	Entity.Direction = params.Direction
	Entity.Depth = 0.5
	destination = params.Destination
	textureSpeed = core.Time(0, 0, 0, 125)
	textureIndex = 1
	texture = {
		'Sprites/teleport_0',
		'Sprites/teleport_1',
		'Sprites/teleport_2',
		'Sprites/teleport_3',
		'Sprites/teleport_4',
		'Sprites/teleport_5',
		'Sprites/teleport_6'
	}
	targetTexture = 'Sprites/teleport_target'
	SetTexture(texture[textureIndex])
	NewTimer(textureSpeed, {Event = "ChangeTexture"})
end

function Update()
	colls = CheckPosition(core.Rect(Entity.Position.X, Entity.Position.Y, Entity.Texture.Width, Entity.Texture.Height))
	for i = 1, #colls do
		SendMessage(colls[i], {Event = 'EntityEnteredTeleporter', Position = destination, Speed = 0.027})
	end
end

function Draw()
	DrawTexture(targetTexture, destination, 0, core.Color(255, 255, 255, 255), 0.3)
end

function OnMessage(message)
	if message.Event ~= nil then
		if message.Event == "ChangeTexture" then
			textureIndex = textureIndex + 1
			if textureIndex > #texture then
				textureIndex = 1
			end
			SetTexture(texture[textureIndex])
			NewTimer(textureSpeed, {Event = "ChangeTexture"})
		end
	end
end

function End()

end
--Copyright d4, 2017--