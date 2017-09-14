--Copyright d4, 2017--
--d4 Core Library
local PARTICLE_SQUARE = 'Sprites/Core/Particles/Square'
local PARTICLE_CIRCLE = 'Sprites/Core/Particles/Circle'
local PARTICLE_LINE = 'Sprites/Core/Particles/Line'
local PARTICLE_SMOKE = {
	'Sprites/Core/Particles/smoke0',
	'Sprites/Core/Particles/smoke1',
	'Sprites/Core/Particles/smoke2',
	'Sprites/Core/Particles/smoke3',
	'Sprites/Core/Particles/smoke4',
	'Sprites/Core/Particles/smoke5',
	'Sprites/Core/Particles/smoke6',
	'Sprites/Core/Particles/smoke7',
	'Sprites/Core/Particles/smoke8',
	'Sprites/Core/Particles/smoke9',
	'Sprites/Core/Particles/smoke10',
	'Sprites/Core/Particles/smoke11',
	'Sprites/Core/Particles/smoke12',
	'Sprites/Core/Particles/smoke13',
	'Sprites/Core/Particles/smoke14',
	'Sprites/Core/Particles/smoke15',
	'Sprites/Core/Particles/smoke16',
	'Sprites/Core/Particles/smoke17',
	'Sprites/Core/Particles/smoke18',
	'Sprites/Core/Particles/smoke19',
	'Sprites/Core/Particles/smoke20',
	'Sprites/Core/Particles/smoke21',
	'Sprites/Core/Particles/smoke22',
	'Sprites/Core/Particles/smoke23',
	'Sprites/Core/Particles/smoke24'
}
local directonUp = (math.pi / 2) + math.pi
local directonDown = math.pi / 2
local directonLeft = math.pi
local directonRight = 0

local function Color(r, g, b, a)
	return { R = r, G = g, B = b, A = a }
end

local function Rect(x, y, w, h)
	return { X = x, Y = y, W = w, H = h }
end

local function Vector(x, y)
	return { X = x, Y = y }
end

local function Min(val1, val2)
	if val1 < val2 then return val1 end
	return val2
end

local function Max(val1, val2)
	if val1 > val2 then
		return val1
	else
		return val2
	end
end

local function MinMax(val, min, max)
	return Min(max, Max(min, val))
end

local function DirectionToVector(direction)
	return Vector(math.cos(direction), math.sin(direction))
end

local function VectorToDirection(vec)
	return math.atan2(vec.Y, vec.X)
end

local function Time(hour, minute, second, millisecond)
	return { Hour = hour, Minute = minute, Second = second, Millisecond = millisecond }
end

local function Sign(val)
	if val > -1 then
		return 1
	end
	return -1
end

local function Abs(val)
	if val > -1 then
		return val
	end
	return val * -1
end

local function Lerp(val1, val2, amount)
	local difference = (Abs(val1 - val2) / amount) * DeltaTime
	if Abs(val1 - val2) < amount then
		return val2
	end
	if val1 < val2 then
		return val1 + Min(Max(val2 - val1, 0), difference)
	end
	return val1 - Min(Max(val1 - val2, 0), difference)
end

local function ColorLerp(col1, col2, amount)
	col1.R = Lerp(col1.R, col2.R, amount)
	col1.G = Lerp(col1.G, col2.G, amount)
	col1.B = Lerp(col1.B, col2.B, amount)
	col1.A = Lerp(col1.A, col2.A, amount)
	return col1
end

local function VectorLerp(vec1, vec2, amount)
	vec1.X = Lerp(vec1.X, vec2.X, amount)
	vec1.Y = Lerp(vec1.Y, vec2.Y, amount)
	return vec1
end

local function Distance(vec1, vec2)
	local vec = {}
	vec.X = vec1.X - vec2.X
	vec.Y = vec1.Y - vec2.Y
	return math.sqrt((vec.X * vec.X) + (vec.Y * vec.Y));
end

local function NormalizeVector(vec)
	local dist = Distance(Vector(0, 0), vec)
	if dist == 0 then return Vector(0, 0) end
	vec.X = vec.X / dist
	vec.Y = vec.Y / dist
	return vec
end

local function RandomRotation()
	return math.random(0, math.pi*2)
end

local function VectorDot(vec1, vec2)
	return (vec1.X * vec2.X) + (vec1.Y * vec2.Y)
end

local function RemoveExponent(digit)
	local int, decimal = math.modf(digit)
	return int
end

local function JoinVectors(vec1, vec2)
	local vec = {}
	vec.X = vec1.X + vec2.X
	vec.Y = vec1.Y + vec2.Y
	return vec
end

local function SubtractVectors(vec1, vec2)
	local vec = {}
	vec.X = vec1.X - vec2.X
	vec.Y = vec1.Y - vec2.Y
	return vec
end

local function MultiplyVector(vec1, num)
	local vec = {X = 0, Y = 0}
	vec.X = vec1.X * num
	vec.Y = vec1.Y * num
	return vec
end

return {
	Color = Color,
	Rect = Rect,
	Vector = Vector,
	Min = Min,
	Max = Max,
	MinMax = MinMax,
	DirectionToVector = DirectionToVector,
	VectorToDirection = VectorToDirection,
	Time = Time,
	Sign = Sign,
	Abs = Abs,
	ColorLerp = ColorLerp,
	VectorLerp = VectorLerp,
	Lerp = Lerp,
	Distance = Distance,
	NormalizeVector = NormalizeVector,
	RandomRotation = RandomRotation,
	VectorDot = VectorDot,
	RemoveExponent = RemoveExponent,
	JoinVectors = JoinVectors,
	SubtractVectors =SubtractVectors,
	MultiplyVector = MultiplyVector,
	Direction = { Up = directonUp, Down = directonDown, Left = directonLeft, Right = directonRight},
	PARTICLE_CIRCLE = PARTICLE_CIRCLE,
	PARTICLE_SQUARE = PARTICLE_SQUARE,
	PARTICLE_LINE = PARTICLE_LINE,
	PARTICLE_SMOKE = PARTICLE_SMOKE
}
--Copyright d4, 2017--