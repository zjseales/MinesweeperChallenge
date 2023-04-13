MinesweeperUnity project

Author: Zac Seales

My second implementation of the game minesweeper.
Only had to reference my previous iteration once (for the chainReveal function).

IMPROVEMENTS:
- UI menus, including:  
	- an options menu with real-time resolution, graphics, and volume customization,
	- a difficulty selection menu with customizable grid size and number of mines,
	- a main menu that can be opened from in-game to change settings, or create new game.
- Update methods (methods called each frame) only detect input, rather than constantly iterating
  the entire grid to detect any state changes.
- Added an original music loop.

POSSIBLE FUTURE IMPROVEMENTS:
- Make window size continuously resizable (rather than choosing from a set of options).
- Have multiple soundtracks to choose from (custom playlists).
- Everything is in 2 scripts, should probably separate these to show better use of object orientation.
- Make hitting a mine cause an explosion that leaves the grid in disarray.
- Could also add sounds and music to signal end of game.
- Add a character that provides commentary on your game.
- Could add accessibility options: 
	- larger (resizable) text .
	- audio to indicate where your mouse is and what the state of the board is at that point.
	- middle mouse button could zoom in, with sliders or buttons (wasd), to navigate the screen.
	- Multiple language options.
	- Keyboard only navigation.
- Could add an undo function.
- For Experts: Make grid changes voice activated.
	-("Reveal square 6, 12", "Flag square 8, 10", "Restart", "New Game - Difficulty Hard").

RESOURCES:

(Unity API)
https://docs.unity3d.com/ScriptReference/

(Unity Version 2021.3.22f1)
https://unity.com/releases/editor/whats-new/2021.3.22

(Unity GUI Asset pack)
https://assetstore.unity.com/packages/2d/gui/simple-fantasy-gui-99451