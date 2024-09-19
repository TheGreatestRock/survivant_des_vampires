extends CharacterBody2D

const SPEED = 300.0

@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D

func _process(delta: float) -> void:
	pass

func _physics_process(delta: float) -> void:

	var direction_leftright := Input.get_axis("ui_left", "ui_right")
	var direction_updown := Input.get_axis("ui_up", "ui_down")

	if direction_leftright != 0.0:
		velocity.x = direction_leftright * SPEED
		animated_sprite_2d.flip_h = velocity.x > 0.0
		animated_sprite_2d.play("walk_left")
	else:
		velocity.x = 0.0

	if direction_updown != 0.0:
		velocity.y = direction_updown * SPEED
		animated_sprite_2d.play("walk_left")
	else:
		velocity.y = 0.0

	if !direction_leftright and !direction_updown:
		animated_sprite_2d.stop()

	velocity = velocity.normalized() * SPEED 
	
	move_and_slide()
	
