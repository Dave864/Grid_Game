# Grid_Game
### Basic Premise
A strategy role-playing game where the random encounters are turn-based tactics battles.
### Controls
Overworld
* Player Movement
 * move up    : w, up arrow
 * move down  : s, down arrow
 * move left  : a, left arrow
 * move right : d, right arrow
* Player Rotation
 * rotate clockwise         : e, left click
 * rotate counter-clockwise : q, right click
Encounter Controls
 * return to overworld : Escape key

### How it Works
Each major piece of the game (the overworld, encounters, etc.) are handled in seperate scenes.

##### OverWorld
This scene loads the player avatar and handles player movement. This scene handles world creation.
* Player Movement
  * Input for player movement and/or player rotation is recieved.
  * Rotation input is recieved.
    1. Rotates the player clockwise or counter-clockwise along the y-axis.
  * Movement input is recieved.
    1. Look at the player's current rotation and changes movement input values. The changes are made so movement corresponds to the player's orientation.
    2. Split up horizontal and vertical movement to prevent diagonal movement. This is done only when horizontal and vertical input are held down at the same time.
    3. Calculate the destination coordinates based on input.
    4. Get the start cell type. Alter the destination coordinates based on the destination's cell type.
    5. Move the player to the updated destination coordinates

* World Creation
  * The starting coordinates of the player are initialized. The starting coordinates are section, row, column.
  * The visible world is made up of 9 sections: Curent, Top_Left, Top, Top_Right, Left, Right, Bottom_Left, Bottom, Bottom_Right.
  * Each section's cells are created and the sections are placed in the correct positions.
   1. The file path for the section's cell information is created based on what number the section coordinate is.
   2. The information is read from the file and is used to create and position each game object.
  * Each time the player move to a new section, the sections' cells are reassigned to different sections.
   * Player moves to the Top section.
    1. Cells in the Bottom_Left, Bottom, and Bottom_Right sections are deleted.
    2. Cells in the Left, Current, and Right sections are moved to the Bottom_Left, Bottom, and Bottom_Right sections respectively.
    3. Cells in the Top_Left, Top, and Top_Right sections are moved to the Left, Current, and Right sections respectively.
    4. The Top_Left, Top, and Top_Right are loaded with new cells.
   * Player moves to the Bottom section.
    1. Cells in the Top_Left, Top, and Top_Right sections are deleted.
    2. Cells in the Left, Current, and Right sections are moved to the Top_Left, Top, and Top_Right sections respectively.
    3. Cells in the Bottom_Left, Bottom, and Bottom_Right sections are moved to the Left, Current, and Right sections respectively.
    4. The Bottom_Left, Bottom, and Bottom_Right are loaded with new cells.
   * Player moves to the Left section.
    1. Cells in the Top_Right, Right, and Bottom_Right sections are deleted.
    2. Cells in the Top, Current, and Bottom sections are moved to the Top_Left, Left, and Bottom_Left sections respectively.
    3. Cells in the Top_Left, Left, and Bottom_Left sections are moved to the Top, Current, and Bottom sections respectively.
    4. The Top_Right, Right, and Bottom_Right are loaded with new cells.
   * Player moves to the Right section.
    1. Cells in the Top_Left, Left, and Bottom_Left sections are deleted.
    2. Cells in the Top, Current, and Bottom sections are moved to the Top_Right, Right, and Bottom_Right sections respectively.
    3. Cells in the Top_Right, Right, and Bottom_Right sections are moved to the Top, Current, and Bottom sections respectively.
    4. The Top_Left, Left, and Bottom_Left are loaded with new cells.

##### Encounter
This scene loads the arena where the encounter will take place.
* Arena Creation

### Bug Report
Sometimes the layout of the terrain will show the empty areas of the world space when rotating the player. When loading up the arena for encounters, the game will only load adjacent sections under certain conditions. When in the Encounter scene, the player can only exit the scene.
