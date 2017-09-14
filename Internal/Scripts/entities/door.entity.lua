--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")

function Initialize(params)
	
	doorTimer = core.Time(0, 0, 5, 0)
	closeDoorRecheckTimer = core.Time(0, 0, 0, 500)
	openSpeed = core.Time(0, 0, 0, 10)
	texture = {
		'Sprites/door_0',
		'Sprites/door_1',
		'Sprites/door_2',
		'Sprites/door_3',
		'Sprites/door_4'
	}
	textureIndex = 1
	playerID = nil
	doorOpen = false
	SetTexture(texture[textureIndex])
	Entity.IsCollidable = true
	Entity.Depth = 0.6
	Entity.Direction = params.Direction
	playerInRange = false
end

function Update()
	if playerID == nil then
		playerID = GetEntitiesByScript('Player.entity.lua')[1].ID
	else
		if doorOpen == false then
			local colls = CheckPosition(core.Rect(Entity.Position.X, Entity.Position.Y, Entity.Texture.Width+64, Entity.Texture.Height+64), 0, {Entity.ID})
			local playerCheck = false
			for i = 1, #colls do
				if colls[i] == playerID then
					playerCheck = true
					if playerInRange == false then
						SendMessageToWidget('PlayerMessages', {Event = 'DisplayMessage', Message = 'Press E to open the door!'})
					end
					playerInRange = true
					if KeyPressed('interact') then
						Entity.IsCollidable = false
						Entity.Depth = 0.1
						NewTimer(doorTimer, {Event = 'CloseDoor'})
						NewTimer(openSpeed, {Event = 'ChangeTexture', Open = true})
						doorOpen = true
						Log('Player opened door: ' .. Entity.ID)
					end
				end
			end
			if playerCheck == false then
				playerInRange = false
			end
		end
		
	end
end

function Draw()

end

function OnMessage(message)
	if message.Event ~= nil then
		if message.Event == 'CloseDoor' then
			if #CheckPosition(core.Rect(Entity.Position.X, Entity.Position.Y, Entity.Texture.Width, Entity.Texture.Height), 0, {Entity.ID}) == 0 then
				doorOpen = false
				Entity.IsCollidable = true
				Entity.Depth = 0.6
				Log('Closing door: ' .. Entity.ID)
				NewTimer(openSpeed, {Event = 'ChangeTexture', Open = false})
			else
				Log('Door: ' .. Entity.ID .. ' is blocked')
				NewTimer(closeDoorRecheckTimer, {Event = 'CloseDoor'})
			end
		end
		if message.Event == 'ChangeTexture' then
			if message.Open then
				textureIndex = core.Min(textureIndex + 1, #texture)
			else
				textureIndex = core.Max(1, textureIndex - 1)
			end
			SetTexture(texture[textureIndex])
			if textureIndex ~= 1 and textureIndex ~= #texture then
				NewTimer(openSpeed, message)
			end
		end
	end
end
--Copyright d4, 2017--