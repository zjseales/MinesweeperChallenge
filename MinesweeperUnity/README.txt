MinesweeperUnity project

Author: Zac Seales

My second implementation of the game minesweeper.
Only had to reference my previous iteration once (for the chainReveal function).
Also got stuck detecting right clicks on UI buttons so had to find a solution online.

(Currently unfinished - 17/04/2023)
TO DO:
- Make clear win and loss conditions, and allow an easy reset of game.
- Once all non-mines have been revealed, automatically flag all unrevealed squares.
- Add explosion particle effect.
- Allow custom game creation, possibly use sliders to avoid invalid input checks.
- Ensure flags can't be toggled after game is over.
- Change Unrevealed square image to suit the theme, and make flags more clearly visible.
- Make flags resize with grid squares and ensure their bounds do not overlap other grid squares.

IMPROVEMENTS:
- UI menus, including:  
	- an options menu with real-time resolution, graphics, and volume customization,
	- a difficulty selection menu with customizable grid size and number of mines,
	- a main menu that can be opened from in-game to change settings, or create new game.
- Update methods (methods called each frame) only detect input, rather than constantly iterating
  the entire grid to detect any state changes.
- Added an original music loop.
- Ensured all grid sizes fit on any screen size.

POSSIBLE FUTURE IMPROVEMENTS:
- Question mark toggles for possible grid squares.
- Make window size continuously resizable (rather than choosing from a set of options).
- Have multiple soundtracks to choose from (custom playlists).
- Everything is in 2 scripts, should probably separate these to show better use of object orientation.
- Make hitting a mine cause an explosion that leaves the grid in disarray.
- Could also add sounds and music to signal end of game.
- Create accessors and mutators so that global fields are not made public when unecessary.
- Add a character that provides commentary on your game.
- Could add accessibility options: 
	- larger (resizable) text .
	- audio to indicate where your mouse is and what the state of the board is at that point.
	- middle mouse button could zoom in, with sliders or buttons (wasd), to navigate the screen.
	- Multiple language options.
	- Keyboard only navigation.
- Could add an undo function.
- Add achievements.
- For Experts: Make grid changes voice activated.
	-("Reveal square 6, 12", "Flag square 8, 10", "Restart", "New Game - Difficulty Hard").

RESOURCES:

(Right Click on UI buttons - solution by jfarias)
https://forum.unity.com/threads/can-the-ui-buttons-detect-a-right-mouse-click.279027/

(Unity API)
https://docs.unity3d.com/ScriptReference/

(Unity Version 2021.3.22f1)
https://unity.com/releases/editor/whats-new/2021.3.22

(GUI Menu Tutorial)
https://www.youtube.com/watch?v=roOM53ZF9Cw&list=PLbbmTaHgSifwOapFSH1gBaAhTRITRHXkw

(Unity GUI Asset pack)
https://assetstore.unity.com/packages/2d/gui/simple-fantasy-gui-99451
