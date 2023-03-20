package game;

/** RunGame.java
 *  2023 - Personal Challenge
 *  The main class which begins the Minesweeper game.
 *  
 *@author Zac Seales
 */

public class RunGame implements Runnable {
	
	/* initialize the GUI of the Minesweeper game.*/
	GUI gui = new GUI();
	
	/** Creates a new thread that holds an instance of the
	 *  RunGame class and starts execution.
	 *@param args - unused parameter.
	 */
	public static void main(String[] args) {
		new Thread(new RunGame()).start();
	} //end main

	/** Overrides the run method of the runnable class, and
	 *  contains an infinite loop to constantly refresh the GUI.
	 */
	@Override
	public void run() {
		while(true) {
			gui.repaint();
			if (!gui.board.resetter) {
				gui.board.checkVictoryStatus();
			}
		}
	} //end run

}// end RunGame class
