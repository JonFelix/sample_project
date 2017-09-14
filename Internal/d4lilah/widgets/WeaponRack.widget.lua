--MOD:d4lilah2
--Copyright d4, 2017--
core = require("d4core")

function RegisterWidget()
	return {
		Position = { X = 0.5, Y = 0.8 },
		Origin = { X = 0, Y = 0 },
		Depth = 0,
		MenuWidget = false
	}
end

function Initialize()
	font = "Fonts/Unispace_regular_16"
	player = {}
	offset = 0
	spriteSize = 64
end

function Update()
	if player.Weapons ~= nil then
		local count = 1
		for i = 1, #player.Weapons do
			count = i
		end
		offset = -((count * spriteSize) / 2)
	end
end

function Draw ()
	if player.Weapons ~= nil then
		local xPos = 0
		for i = 1, #player.Weapons do
			DrawTexture(player.Weapons[i].Icon, core.Vector(offset + xPos, 0), player.Weapons[i].Color)
			if player.SelectedWeapon == i then
				DrawLine(core.Vector(offset + xPos, -2), core.Vector(offset + xPos + spriteSize, -2), player.Weapons[i].Color, 2)
				DrawLine(core.Vector(offset + xPos, spriteSize), core.Vector(offset + xPos + spriteSize, spriteSize), player.Weapons[i].Color, 2)
				local text = player.Weapons[i].Ammo .. ' / ' .. player.Weapons[i].MaxAmmo
				DrawText(font, text, core.Vector(-(TextSize(font, text).X / 2), -spriteSize), player.Weapons[i].Color)
				DrawText(font, player.Weapons[i].Name, core.Vector(-(TextSize(font, player.Weapons[i].Name).X / 2), spriteSize * 1.2), player.Weapons[i].Color)
				if player.Weapons[i].Key == 'selectshotgun' then
					if player.Weapons[i].Loaded == 0 then
						DrawTexture('Sprites/shotgun_shell_icon_0', core.Vector((TextSize(font, text).X / 2) + (spriteSize * 0.25), -(spriteSize * 1.2)), player.Weapons[i].Color, 5)
					end
					if player.Weapons[i].Loaded == 1 then
						DrawTexture('Sprites/shotgun_shell_icon_1', core.Vector((TextSize(font, text).X / 2) + (spriteSize * 0.25), -(spriteSize * 1.2)), player.Weapons[i].Color, 5)
					end
					if player.Weapons[i].Loaded == 2 then
						DrawTexture('Sprites/shotgun_shell_icon_2', core.Vector((TextSize(font, text).X / 2) + (spriteSize * 0.25), -(spriteSize * 1.2)), player.Weapons[i].Color, 5)
					end
				end
			end
			xPos = xPos + spriteSize
		end
	end
end

function OnMessage(information)
	player = information
end
--Copyright d4, 2017--