--&copy;opyright d4, 2017--
# Level Documentation
## Events
These are the events that the engine will call.
### RegisterWidget()
>  Fires when the game starts. It is important to return your widget settings here! if no table is returned the widget will be discarded.
> <b>Returns:</b> table
### Initialize()
>  Fires when the entity is created.
### Update()
>  Fires every frame. Do your logic here.
### Draw()
>  Fires every frame.Draw your widget here. 

## Functions
These functions are available within the level.
### DrawText(string font, string text, vector position, float scale, color color)
>   Draws text on the screen. Only available in the Draw event.</br>
>   <b>Return:</b>  int
### DrawRectangle(rect rectangle, color color)
>   Draws a rectangle on the screen. Only available in the Draw event.</br>
>   <b>Return:</b>  int
### TextWidth(string font, string text)
>   Calculates text's with width with a specific font.</br>
>   <b>Return:</b>  int
### Log(string message)
>   Logs a message.</br>
>   <b>Return:</b>  int
### LogTable(table information)
>   Prints a table in the log.</br>
>   <b>Return:</b>  int



==========================
## Variables
These variables are available within the level. They are <b>read only</b>!
### LogStack
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

--&copy;opyright d4, 2017--