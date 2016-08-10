# Grid_Game
### Basic Premise
A strategy role-playing game where the random encounters are turn-based tactics battles.
### Controls
Player Movement
*move up    : w, up arrow
*move down  : s, down arrow
*move left  : a, left arrow
*move right : d, right arrow
Player Rotation
*rotate clockwise         : e, left click
*rotate counter-clockwise : q, right click
### How it Works
Each major piece of the game (the overworld, encounters, etc.) are handled in seperate scenes.
##### OverWorld
This scene loads the player avatar and handles player movement. This scene handles world creation.
* Player Movement 1
  * Input for player movement and/or player rotation is recieved. 1a
  * Rotation input is recieved 1b
    1. Rotates the player clockwise or counter-clockwise along the y-axis. 1b.1
  * Movement input is recieved 1c
    1. Look at the player's current rotation and changes movement input values. The changes are made so movement corresponds to the player's orientation. 1c.1
    2. Split up horizontal and vertical movement to prevent diagonal movement. This is done only when horizontal and vertical input are held down at the same time. 1c.2
    3. Calculate the destination coordinates based on input. 1c.3
    4. Get the start cell type. Alter the destination coordinates based on the destination's cell type. 1c.4
    5. Move the player to the updated destination coordinates 1c.5
* World Creation 2
  1. The starting coordinates of the player are initialized. The starting coordinates are section, row, column. 2a.1
### Bug Report
Sometimes the layout of the terrain will show the empty areas of the world space when rotating the player. The player can only move around in the "center" section of the world.
