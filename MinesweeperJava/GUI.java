package game;

import javax.swing.JFrame;

/** GUI.java
 *  2023 - Personal Challenge
 *  The GUI containing all graphics for the Minesweeper game.
 *  
 *  @author Zac Seales
 */

public class GUI extends JFrame{
	
	/** Random ID to keep compiler happy. */
	private static final long serialVersionUID = 1L;
	
	/* The game board panel containing all UI elements and the paintComponent method. */
	public GameBoard board;
	
	/** Constructor: Initializes a default game of Minesweeper
	 *  using a (16x9) grid.
	 */
	public GUI(){
		// set window options with a fixed size.
		this.setTitle("Minesweeper");
		this.setSize(1280,829);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setVisible(true);
		this.setResizable(false);
		
		// add the GameBoard UI panel to this window frame.
		board = new GameBoard();
		this.setContentPane(board);
		
	}

}
