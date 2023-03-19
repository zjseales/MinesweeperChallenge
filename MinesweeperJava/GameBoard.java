package game;

import java.util.*;
import java.awt.*;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;

import javax.swing.*;

/** GameBoard.java
 *  2023 - Personal Challenge
 *  The panel containing all UI elements for the game, 
 *  Minesweeper.
 *  
 * @author Zac Seales
 */
public class GameBoard extends JPanel{
	
	/* The size of each square (in pixels). */ 
	private int boxSize;
	/* The space size between each square (in pixels). */
	private int spacing;
	
	/* The x co-ordinate of the mouse. */
	private int mX;
	/* The y coordinate of the mouse. */
	private int mY;
	
	/* The x index of the most recently clicked box. */
	private int bX = -1;
	/* The y index of the most recently clicked box. */
	private int bY = -1;
	
	/* The "values" of each square in the grid. Either the number of neighbouring mines (0-8), or a mine (9). */
	private int[][] squareValues;
	/* The "state" of each square, either flagged (-1), revealed (0), or neither (1). */
	private int[][] squareStates;
	
	/** Constructor: Initializes the default board state. 
	 */
	public GameBoard() {
		// initialize data-field values.
		boxSize = 79;
		spacing = 2;
		mX = 0;
		mY = 0;
		squareValues = new int[16][9];
		squareStates  = new int[16][9];
		
		// Store mines randomly.
		Random rand = new Random();
		for(int i = 0; i < 16; i++) {
			for(int j = 0; j < 9; j++) {
				// 20 percent chance of mine spawn.
				if(rand.nextInt(100) < 20) {
					squareValues[i][j] = 9;
				} else {
					squareValues[i][j] = -1;
				}
				// all squares start unflagged and unrevealed
				squareStates[i][j] = 1;
			}
		}
		// counts all neighbouring mines for each square.
		for(int i = 0; i < 16; i++) {
			for(int j = 0; j < 9; j++) {
				int neighs = 0;
				for(int m = 0; m < 16; m++) {
					for(int n = 0; n < 9; n++) {
						if(isN(i, j, m, n) && !(m == i && n == j)) {
							neighs++;
						}
					}
				}
				// record neighbouring mines only if no mine is stored
				if (squareValues[i][j] != 9) {
					squareValues[i][j] = neighs;
				}
			}
		}
		
		// add the mouse movement event listener to the panel.
		Move move = new Move();
		this.addMouseMotionListener(move);
		// add the mouse click event listener.
		Click click = new Click();
		this.addMouseListener(click);
	}// end constructor
	
	/** The paint method, determines the current state of the UI.
	 *@param g - the Graphics component.
	 */
	public void paintComponent(Graphics g) {
		
		// set background color.
		g.setColor(Color.lightGray);
		g.fillRect(0, 0, 1280, 800);
		
		// create grid of squares (16x9).
		for(int i = 0; i < 16; i++) {
			int boxXPos = spacing + i * boxSize;
			for(int j = 0; j < 9; j++) {
				int boxYPos = spacing + boxSize + j * boxSize;
				int boxSize = this.boxSize - 2 * spacing;
				
				// non-revealed boxes
				if (squareStates[i][j] == 1) {
					g.setColor(Color.darkGray);
					g.fillRect(boxXPos, boxYPos, boxSize, boxSize);
				
				// revealed boxes
				} else if (squareStates[i][j] == 0){
					// revealed mine
					if (squareValues[i][j] == 9) {
						g.setColor(Color.red);
						g.fillRect(boxXPos, boxYPos, boxSize, boxSize);
						//draw mine
						g.setColor(Color.black);
						g.fillRect(boxXPos + 28, boxYPos + 18, 21, 41);
						g.fillRect(boxXPos + 18, boxYPos + 28, 41, 21);
						g.fillRect(boxXPos + 23, boxYPos + 23, 31, 31);
						g.fillRect(boxXPos + 36, boxYPos + 13, 5, 51);
						g.fillRect(boxXPos + 13, boxYPos + 36, 51, 5);
					} else {
						g.setColor(Color.black); // no mine
						g.drawRect(boxXPos, boxYPos, boxSize, boxSize);
						if (squareValues[i][j] != 0) {
							g.setColor(getColor(squareValues[i][j]));
							g.setFont(new Font("Tahoma", Font.BOLD, 40));
							g.drawString("" + squareValues[i][j], boxXPos + 25, boxYPos + 53);
						}
					}
				} else {
					//draw flag
				}

				// highlight the box currently hovered with the mouse.
				if (inBoxX() == i && inBoxY() == j) {
					g.setColor(Color.red);
					g.drawRect(boxXPos, boxYPos, boxSize, boxSize);
				}
			}
		}
		
	}// end paintComponent
	
	/** Returns the x index value of the box currently hovered.
	 *@return - the x index of the currently hovered box.
	 */
	public int inBoxX() {
		for(int i = 0; i < 16; i++) {
			int boxXPos = spacing + i * boxSize;
			int boxSize = this.boxSize - 2 * spacing;
			// return the x box index currently hovered with the mouse.
			if (mX >= boxXPos && mX < boxXPos + boxSize) {
				return i;
			}

		}
		return -1;
	}
	
	/** Returns the y index value of the box currently hovered.
	 *@return - the y index of the currently hovered box.
	 */
	public int inBoxY() {
		for(int j = 0; j < 9; j++) {
			int boxYPos = spacing + boxSize + j * boxSize;
			int boxSize = this.boxSize - 2 * spacing;
			// return the y box index currently hovered with the mouse.
			if (mY >= boxYPos && mY < boxYPos + boxSize) {
				return j;
			}
		}
		return -1;
	}
	
	/** Returns the color, dependent on the value of neighbouring mines.
	 *@param n - the number of neighbouring mines.
	 *@return - the color associated with the input value.
	 */
	public Color getColor(int n) {
		Color c = null;
		switch(n) {
			case 1:
				c = Color.blue;
				break;
			case 2:
				c = new Color(0, 102, 0);
				break;
			case 3:
				c = Color.red;
				break;
			case 4:
				c = new Color(76, 0, 153);
				break;
			case 5:
				c = new Color(102, 0, 0);
				break;
			case 6:
				c = new Color(0, 255, 255);
				break;
			case 7:
				c = Color.black;
				break;
			case 8:
				c = Color.darkGray;
				break;
			default:
				System.err.println("Unknown Value");
		}
		return c;
	}
	
	
	/** Determines if a box is neighbouring and contains a mine.
	 *@param mx - the neighbouring box x index being checked.
	 *@param my - the neighbouring box y index being checked.
	 *@param cx - the current x index being checked.
	 *@param cy - the current y index being checked.
	 */
	public boolean isN(int mx, int my, int cx, int cy) {
		if((mx - cx) < 2 && (mx - cx) > -2 && (my - cy) < 2 && (my - cy) > -2 && squareValues[cx][cy] == 9) {
			return true;
		}
		return false;
	}
	
	/** Mouse movement Action Event Listener. */
	public class Move implements MouseMotionListener {

		@Override
		public void mouseDragged(MouseEvent e) {
			mX = e.getX();
			mY = e.getY();
		}

		/** Records current mouse coordinates in global data-fields.
		 *@param e - the mouse movement event.
		 */
		@Override
		public void mouseMoved(MouseEvent e) {
			mX = e.getX();
			mY = e.getY();
		}
	}// end Move class
	
	/** Mouse Click Action Event Listener. */
	public class Click implements MouseListener {

		/** Determines all actions to perform when mouse buttons are clicked.
		 *@param e - the mouse click event.
		 */
		@Override
		public void mouseClicked(MouseEvent e) {
			int x = inBoxX();
			int y = inBoxY();
			// do nothing if no box is clicked.
			if (x == -1 || y == -1) {
				return;
			}
			// set square state to revealed
			squareStates[x][y] = 0;
		}

		@Override
		public void mousePressed(MouseEvent e) {
			// do nothing if no box is clicked.
			if (inBoxX() == -1 || inBoxY() == -1) {
				return;
			} else {
				bX = inBoxX();
				bY = inBoxY();
			}
		}

		@Override
		public void mouseReleased(MouseEvent e) {
			// do nothing if no box is clicked.
			if (inBoxX() == -1 || inBoxY() == -1) {
				return;
			}
			if (bX == inBoxX() && bY == inBoxY()) {
				//reveal square
				if(SwingUtilities.isLeftMouseButton(e)) {
					squareStates[inBoxX()][inBoxY()] = 0;
					bX = -1;
					bY = -1;
				}
				
			}
		}

		@Override
		public void mouseEntered(MouseEvent e) {
			// TODO Auto-generated method stub
		}

		@Override
		public void mouseExited(MouseEvent e) {
			// TODO Auto-generated method stub
			
		}
		
	}// end Click class
	
}
