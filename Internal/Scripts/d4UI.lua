core = require("d4core")

local checkbox_checked_sprite = 'Sprites/UI/checked_checkbox'
local checkbox_unchecked_sprite = 'Sprites/UI/unchecked_checkbox'

local function BorderRect(rect, color, depth)
	DrawRectangle(core.Rect(rect.X - 2, rect.Y, 2, rect.H), color, depth)
	DrawRectangle(core.Rect(rect.X + rect.W, rect.Y, 2, rect.H), color, depth)
	DrawRectangle(core.Rect(rect.X - 2, rect.Y - 2, rect.W + 4, 2), color, depth)
	DrawRectangle(core.Rect(rect.X - 2, rect.Y + rect.H, rect.W + 4, 2), color, depth)
end

local function CheckBox(value, position, checkedColor, uncheckedColor, mouseOverColor, selectable)
	local mouseOver = false
	if selectable then
		if MouseInRect(core.Rect(position.X, position.Y, 32, 32)) then
			mouseOver = true
			if KeyPressed('menuclick') then
				if value == true then
					value = false
				else
					value = true
				end
			end
		end
	else
		DrawTexture(checkbox_unchecked_sprite, position, uncheckedColor)
		return value
	end
	if value == true then
		if mouseOver then
			DrawTexture(checkbox_checked_sprite, position, mouseOverColor)
		else
			DrawTexture(checkbox_checked_sprite, position, checkedColor)
		end
		
	else
		if mouseOver then
			DrawTexture(checkbox_unchecked_sprite, position, mouseOverColor)
		else
			DrawTexture(checkbox_unchecked_sprite, position, uncheckedColor)
		end
	end
	return value
end

------------
-- BUTTON --
------------
local function CreateButton(text, position, font, endPosition)
	if endPosition == nil then
		endPosition = position
	end
	button = {
			Text = text,
			Hitbox = core.Rect(0, 0, TextSize(font, text).X, TextSize(font, text).Y),
			Selected = false,
			HitBoxPosition = position,
			HitBoxEndPosition = endPosition,
			TransitionSpeed = 0.12,
			TextColor = core.Color(70, 200, 82, 255),
			Font = font,
			ShadowOffset = core.Vector(3, 3),
			ShadowColor = core.Color(79, 80, 85, 255),
			SelectorSize = 0,
			SelectorSizeMax = TextSize(font, text).X,
			SelectorSpeed = 0.12
		}
		return button
end

local function DrawButton(button)
	returnValue = false
	button.HitBoxPosition = core.VectorLerp(button.HitBoxPosition, button.HitBoxEndPosition, button.TransitionSpeed)
	button.Selected = false
	if MouseInRect(core.Rect(button.HitBoxPosition.X, button.HitBoxPosition.Y, button.Hitbox.W, button.Hitbox.H)) then
		button.Selected = true
	end
	if button.Selected then
		if KeyPressed('menuclick') then
			returnValue = true
		end
		button.SelectorSize = core.Lerp(button.SelectorSize, button.SelectorSizeMax, button.SelectorSpeed)	
	else
		button.SelectorSize = 0;
	end
	DrawText(button.Font, button.Text, core.Vector(button.HitBoxPosition.X + button.ShadowOffset.X, button.HitBoxPosition.Y + button.ShadowOffset.Y), button.ShadowColor, 0.1)
	DrawText(button.Font, button.Text, button.HitBoxPosition, button.TextColor, 0.2)
	DrawRectangle(core.Rect((button.HitBoxPosition.X + (button.Hitbox.W / 2)) - (button.SelectorSize / 2) + button.ShadowOffset.X, button.HitBoxPosition.Y + button.Hitbox.H + button.ShadowOffset.Y, button.SelectorSize, 2), button.ShadowColor)
	DrawRectangle(core.Rect((button.HitBoxPosition.X + (button.Hitbox.W / 2)) - (button.SelectorSize / 2), button.HitBoxPosition.Y + button.Hitbox.H, button.SelectorSize, 2), backButton.TextColor)
	return returnValue
end

------------
--  LIST  --
------------
local function CreateListBox(items, rect, font, textColor, backColor, selectedColor, endPosition)
	if endPosition == nil then
		endPosition = core.Vector(rect.X, rect.Y)
	end	
	listBox = {
		Items = items,
		Hitbox = rect,
		Expanded = false,
		ItemIndex = 1, 
		HitBoxEndPosition = endPosition,
		TransitionSpeed = 0.12,
		TextColor = textColor,
		BackColor = backColor,
		SelectedColor = selectedColor,
		ListTextColor = core.Color(backColor.R, backColor.G, backColor.B, backColor.A),
		Font = font
	}
	return listBox
end

local function DrawListBox(listBox)
	if listBox.Expanded then
		listBox.ListTextColor = core.ColorLerp(listBox.ListTextColor, listBox.TextColor, listBox.TransitionSpeed)
		offset = listBox.Hitbox.Y - (listBox.ItemIndex * listBox.Hitbox.H)
		DrawRectangle(core.Rect(listBox.Hitbox.X, offset + listBox.Hitbox.H, listBox.Hitbox.W, listBox.Hitbox.H * #listBox.Items), listBox.BackColor, 0.98) 
		BorderRect(core.Rect(listBox.Hitbox.X, offset + listBox.Hitbox.H, listBox.Hitbox.W, listBox.Hitbox.H * #listBox.Items), listBox.TextColor, 0.98)
		for i = 1, #listBox.Items do
			if i == listBox.ItemIndex then
				DrawRectangle(core.Rect(listBox.Hitbox.X, offset + (i * listBox.Hitbox.H) - 2, listBox.Hitbox.W, 2), listBox.TextColor, 0.98)
				DrawRectangle(core.Rect(listBox.Hitbox.X, offset + (i * listBox.Hitbox.H) + listBox.Hitbox.H, listBox.Hitbox.W, 2), listBox.TextColor, 0.98)
			end
			if MouseInRect(core.Rect(listBox.Hitbox.X, offset + (i * listBox.Hitbox.H), listBox.Hitbox.W, listBox.Hitbox.H)) then
				DrawText(listBox.Font, listBox.Items[i], core.Vector(listBox.Hitbox.X + (core.Abs(listBox.Hitbox.W - TextSize(listBox.Font, listBox.Items[i]).X) / 2), offset + (i * listBox.Hitbox.H) + (core.Abs(listBox.Hitbox.H - TextSize(listBox.Font, listBox.Items[i]).Y))), listBox.SelectedColor, 0.99)
				if KeyPressed('menuclick') then
					listBox.ItemIndex = i
				end
			else
				DrawText(listBox.Font, listBox.Items[i], core.Vector(listBox.Hitbox.X + (core.Abs(listBox.Hitbox.W - TextSize(listBox.Font, listBox.Items[i]).X) / 2), offset + (i * listBox.Hitbox.H)+ (core.Abs(listBox.Hitbox.H - TextSize(listBox.Font, listBox.Items[i]).Y))), listBox.ListTextColor, 0.99)
			end
		end
	else
		DrawRectangle(listBox.Hitbox, listBox.BackColor, 0.3)
		if MouseInRect(listBox.Hitbox) then
			DrawText(listBox.Font, listBox.Items[listBox.ItemIndex], core.Vector(listBox.Hitbox.X + (core.Abs(listBox.Hitbox.W - TextSize(listBox.Font, listBox.Items[listBox.ItemIndex]).X) / 2), listBox.Hitbox.Y + (core.Abs(listBox.Hitbox.H - TextSize(listBox.Font, listBox.Items[listBox.ItemIndex]).Y))), listBox.SelectedColor, 0.31)
			BorderRect(listBox.Hitbox, listBox.SelectedColor, 0.31)
		else
			DrawText(listBox.Font, listBox.Items[listBox.ItemIndex], core.Vector(listBox.Hitbox.X + (core.Abs(listBox.Hitbox.W - TextSize(listBox.Font, listBox.Items[listBox.ItemIndex]).X) / 2), listBox.Hitbox.Y + (core.Abs(listBox.Hitbox.H - TextSize(listBox.Font, listBox.Items[listBox.ItemIndex]).Y))), listBox.TextColor, 0.31)
			BorderRect(listBox.Hitbox, listBox.TextColor, 0.31)
		end
		listBox.ListTextColor = core.Color(listBox.BackColor.R, listBox.BackColor.G, listBox.BackColor.B, listBox.BackColor.A)
	end
	if KeyPressed('menuclick') then
		if MouseInRect(listBox.Hitbox) then
			if listBox.Expanded == false then
				listBox.Expanded = true 
			else
				listBox.Expanded = false
			end
		else
			listBox.Expanded = false
		end
	end
	return listBox
end

------------
-- SlIDER --
------------

local function Slider(position, width, val, forecolor, backcolor, selectcolor, showText)
	if MouseInRect(core.Rect(position.X, position.Y, width, 20)) then
		DrawRectangle(core.Rect(position.X, position.Y + 9, width, 2), backcolor, 0.3)
		DrawRectangle(core.Rect(position.X + (val * width) - 2, position.Y, 4, 20), selectcolor, 0.31)
		if KeyDown('menuclick') then
			mousePos = core.Abs(MousePosition.X - position.X)
			val = mousePos / width
		end
	else
		DrawRectangle(core.Rect(position.X, position.Y + 9, width, 2), backcolor, 0.3)
		DrawRectangle(core.Rect(position.X + (val * width) - 2, position.Y, 4, 20), forecolor, 0.31)
	end
	if showText then
		DrawText('Fonts/Unispace_regular_12', val*100 .. '%', core.Vector(position.X + width + 10, position.Y),forecolor)
	end
	return core.Max(core.Min(val, 1), 0)
end

return {
	CheckBox = CheckBox,
	TextBox = TextBox,
	CreateButton = CreateButton,
	DrawButton = DrawButton,
	CreateListBox = CreateListBox,
	DrawListBox = DrawListBox,
	Slider = Slider
}