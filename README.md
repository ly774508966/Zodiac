# Zodiac
An experimental tool for adding functionality to games using Unity Editor and EditorVR


Motivation:

Text-based scripting languages are really good for complex logic but there are many cases in which it feels like overkill. Of course, as developers we work with whats accessible and ergonomic, and become very proficcient at it. Sometimes its easy to forget how many hours go into learning how to code, and how to understand the link between text based characters and geometry for example.


Consider the following scenarios:

	-Spherical Gravity
	-GUI functionality
	-Triggering events on player entering a room

These behaviours don't nessecarily require the full power of c# to be achieved, in fact they sound like the kind of thing that could benefit from being directly visualizable in the editor.



Architecture:
	-Reflection
	-Generic Methods
	-Delegates

Known Bugs:
	-Zodiac Creator Workspace
		Workspace frequently resets
		Possibly due to inherited ‘inspector workspace’ behaviour
	-Unable to change the value of PropertyMonos
	
Wishlist:
	Bezier curves for connections
	Editable properties in EditorVR
	Better designed 'standalone' values
	
Future Development:
AR
Themes
Baking
Live Previews