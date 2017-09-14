--&copy;opyright d4, 2017--
# Level Documentation
## Events
These are the events that the engine will call.
### Initialize()
>  Fires when the entity is created.



## Functions
These functions are available within the level.
### KeyDown(string keyMap)
>   Checks if a specific is held down this frame.</br>
>   <b>Return:</b>  bool
### KeyPressed(string keyMap)
>   Checks if a specific was pressed this frame.</br>
>   <b>Return:</b>  bool
### KeyReleased(string keyMap)
>   Checks if a specific was released this frame.</br>
>   <b>Return:</b>  bool
### CheckPosition(Rect place)
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
These variables are available within the level. They are <b>read only</b>!

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

--&copy;opyright d4, 2017--