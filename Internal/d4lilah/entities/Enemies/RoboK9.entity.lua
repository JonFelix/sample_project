--MOD:d4lilah
--Copyright d4, 2017--
core = require("d4core")
ai = require("d4AI")
blaster = require("Blaster")

function Initialize(params)
	SetTexture('Sprites/RK9_idle_0')
	Entity.IsCollidable = true
	Entity.Depth = 0.5

	me = ai.InitiateAI()
	me.Weapon = blaster
	me.Weapon.IsNPC = true
	me.Weapon.Rate = 1
	me.Weapon.Color = core.Color(255, 0, 0, 255)
	me.Name = 'RK9'
end

function Update()
	if me.Health > 0 then
		if ai.CanISeePlayer(me) then
			me.Target = me.Player
		end
		if ai.IsTargetInLineOfSight(me, me.Target) then
			ai.ShootAtTarget(me)
		end
		if ai.HasTarget(me) then
			me = ai.MoveToTarget(me)
		end
	else
		me = ai.DeathEvent(me)
	end
	me = ai.Update(me)
end

function OnMessage(message)
	me = ai.CheckForMessages(me, message)
end