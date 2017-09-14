--&copy;opyright d4, 2017--
# Entity Documentation
## Events
These are the events that the engine will call.
### Initialize(table params)
>  Fires when the entity is created.
### Update()
>  Fires once each frame.
### End()
>  Fires when the entity is removed.
### OnMessage()
>  Fires when the entity recieves a message.


## Functions
These functions are available within the entity.
### SetTexture(string path)
>   Sets the texture representing this entity.</br>
>   <b>Return:</b>  int
### SetCollidable(bool collidable)
>   Sets if this entity should trigger collision checks.</br>
>   <b>Return:</b>  int
### SetPosition(vector position)
>   Sets the position</br>
>   <b>Return:</b>  int
### SetDepth(int depth)
>   Sets the texture depth.</br>
>   <b>Return:</b>  int
### SetDirection(float rotation)
>   Sets rotation of the texture. Uses radians.</br>
>   <b>Return:</b>  int
### Move(vector position)
>   Moves the position relative to its position.</br>
>   <b>Return:</b>  int
### KeyDown(string keyMap)
>   Checks if a specific is held down this frame.</br>
>   <b>Return:</b>  bool
### KeyPressed(string keyMap)
>   Checks if a specific was pressed this frame.</br>
>   <b>Return:</b>  bool
### KeyReleased(string keyMap)
>   Checks if a specific was released this frame.</br>
>   <b>Return:</b>  bool
### CheckPosition(rect place)
>   Checks if the Rect intersects other entities with textures and set as collidable. Returns an array of entity ID's.</br>
>   <b>Return:</b>  int[]
### GetEntity(int ID)
>   Gets an entity with a specific ID.</br>
>   <b>Return:</b>  entity
### SendMessage(int ID, table information)
>   Sends a table to an entity with a specific ID.</br>
>   <b>Return:</b>  int
### BroadcastMessage(table information)
>   Sends a table to all active entities.</br>
>   <b>Return:</b>  int
### Log(string message)
>   Logs a message.</br>
>   <b>Return:</b>  int
### LogTable(table information)
>   Prints a table in the log.</br>
>   <b>Return:</b>  int
### SpawnEntity(string name, table parameters)
>   Spawns an entity with a specific name. Also provide a table with params.</br>
>   <b>Return:</b>  int
### SetCamera(vector position, float rotation, float zoom)
>   Sets the camera position on Vector with specific rotation and zoom.</br>
>   <b>Return:</b>  int
### SetCameraOrigin(vector origin)
>   Sets the camera offset in relation to its position.</br>
>   <b>Return:</b>  int


==========================
## Variables
These variables are available within the entity. They are <b>read only</b>!
### LogInfo
><b>Return:</b>  string[]
### CameraPosition
><b>Return:</b>  vector
### CameraRotation
><b>Return:</b>  float
### CameraZoom
><b>Return:</b>  float
### DeltaTime
><b>Return:</b>  float
### MousePosition
><b>Return:</b>  vector
### ScreenWidth
><b>Return:</b>  int
### ScreenHeight
><b>Return:</b>  int
### Position
><b>Return:</b>  vector
### Direction
><b>Return:</b>  float
### Depth
><b>Return:</b>  int
### ID
><b>Return:</b>  int
### TextureHeight
><b>Return:</b>  int
### TextureWidth
><b>Return:</b>  int

--&copy;opyright d4, 2017--