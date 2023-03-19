MinesweeperJava project

Author: Zac Seales

My initial attempt at coding Minesweeper.
I've mainly used the tutorial series by "Jon" on youtube, with a few improvements.
(Referenced below under "Resources").

MY IMPROVEMENTS:
- Stored grid square sizes and positions in local variables to reduce number of calculations in the paint method.
- Only used two 2d arrays to hold all information (instead of 4).
- Moved some expressions to separate methods to improve efficiency, (lower complexity).
- Clicking boxes is more reliable. Dragging mouse while clicked does not prevent box reveals, as long as the box is pressed and 
  released with the same mouse press then the box will be revealed (or flagged if right clicked).
- Flags are added and removed with right click. (Instead of pressing flag button then checking if flag is active).
- Used a switch statement to determine number colours, (Instead of multiple 'if' statements).

POSSIBLE FUTURE IMPROVEMENTS:
- Let user choose a difficulty level.
- Let grid size and window size be resizable.
- Let user choose grid size and number of mines.
- Make UI elements easily modifiable. Currently, with the fixed sizes, any change in position or size can affect 
  other unrelated elements.

RESOURCES:

(Youtube Tutorial)
https://www.youtube.com/watch?v=RFpJp62ZoY8&list=PLGxHvpw-PAk6QvPw0fYe8bks31GRKvymK&index=1

(Eclipse IDE)
https://www.eclipse.org/downloads/

(JDK v17.03)
https://www.oracle.com/java/technologies/downloads/#java17