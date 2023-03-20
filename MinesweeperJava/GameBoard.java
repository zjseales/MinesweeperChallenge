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
	
	/** Random ID to keep compiler happy. */
	private static final long serialVersionUID = 1L;

	/* Defines whether game is currently being reset. */
	public boolean resetter;
	
	/* The date used to calculate elapsed time. */
	private Date startDate;
	/* The elapsed time in seconds. */
	private int sec;
	
	/* The size of each square (in pixels). */ 
	private int boxSize;
	/* The space size between each square (in pixels). */
	private int spacing;
	
	/* The x co-ordinate of the mouse. */
	private int mX;
	/* The y coordinate of the mouse. */
	private int mY;
	
	/* The x position of the smiley face. */
	private int smileyX;
	/* The x position of the smiley face. */
	private int smileyY;
	
	/* Loss condition. */
	private boolean loss;
	/* Win condition. */
	private boolean win;
	
	/* The x index of the most recently clicked box. */
	private int bX;
	/* The y index of the most recently clicked box. */
	private int bY;
	
	/* The "values" of each square in the grid. Either the number of neighbouring mines (0-8), or a mine (9). */
	private int[][] squareValues;
	/* The "state" of each square, either flagged (-1), revealed (0), or neither (1). */
	private int[][] squareStates;
	
	/** Constructor: Initializes the default board state. 
	 */
	public GameBoard() {
		resetter = true;
		// initialize all data-field values.
		initFields();
		// Store mines randomly.
		storeMines();
		// Count and record all neighbouring mines for each square.
		setUpNeighbourValues();
		
		// add Mouse event listeners
		this.addMouseMotionListener(new Move());
		this.addMouseListener(new Click());
		resetter = false;
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
				
				// revealed boxes
				if (squareStates[i][j] == 0){
					
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
						//display neighbours
						if (squareValues[i][j] != 0) {
							g.setColor(getColor(squareValues[i][j]));
							g.setFont(new Font("Tahoma", Font.BOLD, 40));
							g.drawString("" + squareValues[i][j], boxXPos + 25, boxYPos + 53);
						}
					}
				// non-revealed boxes
				} else {
					g.setColor(Color.darkGray);
					g.fillRect(boxXPos, boxYPos, boxSize, boxSize);
					//draw flag
					if (squareStates[i][j] == -1) {
						g.setColor(Color.black);
						g.fillRect(boxXPos + 50, boxYPos + 10, 5, 54);
						g.setColor(Color.red);
						g.fillRect(boxXPos + 10, boxYPos + 10, 40, 25);
					}
				}

				// highlight the box currently hovered with the mouse.
				if (inBoxX() == i && inBoxY() == j) {
					g.setColor(Color.red);
					g.drawRect(boxXPos, boxYPos, boxSize, boxSize);
				}
			}
		}
		// draw smiley face
		g.setColor(Color.yellow);
		g.fillOval(smileyX, smileyY, 70, 70);
		g.setColor(Color.black);
		g.drawOval(smileyX, smileyY, 70, 70);
		
		//happy face
		if(!loss) {
			//sun glasses
			if (win) {
				g.fillRect(smileyX + 3, smileyY + 17, 65, 5);
				g.fillArc(smileyX + 14, smileyY + 10, 22, 22, 180, 180);
				g.fillArc(smileyX + 37, smileyY + 10, 22, 22, 180, 180);
			} else {
				g.fillOval(smileyX + 17, smileyY + 20, 10, 10);
				g.fillOval(smileyX + 42, smileyY + 20, 10, 10);
			}
			g.drawArc(smileyX + 10, smileyY + 20, 50, 35, 190, 160);
			
		//sad face
		} else {
			g.drawLine(smileyX + 17, smileyY + 18, smileyX + 21, smileyY + 22);
			g.drawLine(smileyX + 17, smileyY + 22, smileyX + 21, smileyY + 18);
			g.drawLine(smileyX + 50, smileyY + 18, smileyX + 54, smileyY + 22);
			g.drawLine(smileyX + 50, smileyY + 22, smileyX + 54, smileyY + 18);
			g.drawArc(smileyX + 10, smileyY + 35, 50, 35, 15, 150);
		}
		
		// display timer
		g.fillRect(1028, 3, 234, 75);
		// time color depends on game state
		if (!loss && ! win) {
			g.setColor(Color.white);
		} else if (win) {
			g.setColor(Color.green);
		} else {
			g.setColor(Color.red);
		}
		// update time if game is not lost or won
		if (!loss && !win) {
			sec = (int)((new Date().getTime() - startDate.getTime()) / 1000);
		}
		
		g.setFont(new Font("Tahoma", Font.BOLD, 65));
		int timeXPos = 1035;
		int timeYPos = 63;
		
		// add zeroes so time is right aligned.
		if(sec < 10) {
			g.drawString("000" + sec + " s", timeXPos, timeYPos);	
		} else if (sec < 100) {
			g.drawString("00" + sec + " s", timeXPos, timeYPos);	
		} else if (sec < 1000){
			g.drawString("0" + sec + " s", timeXPos, timeYPos);
		} else if (sec < 10000){
			g.drawString("" + sec + " s", timeXPos, timeYPos);
		} else {
			sec = 9999;
			g.drawString("" + sec + " s", timeXPos, timeYPos);
		}
		
	}// end paintComponent
	
	/** Chain reveals all neighbouring squares to a revealed square with no neighbouring mines.
	 * @param x - The x index of the revealed box with no neighbouring mines.
	 * @param y - The y index of the revealed box with no neighbouring mines.
	 */
	private void chainReveal(int x, int y) {
		int nextX;
		int nextY;
		for(int i = -1; i < 2; i++) {
			nextX = x + i;
			for (int j = -1; j < 2; j++) {
				nextY = y + j;
				if (nextY >= 0 && nextY < 9 && nextX >= 0 && nextX < 16) {
					if (nextY >= 0 && squareStates[nextX][nextY] == 1 && !(nextX == x && nextY == y)){
						squareStates[nextX][nextY] = 0;
						if(squareValues[nextX][nextY] == 0) {
							chainReveal(nextX, nextY);
						}
					}
				}
			}
		}	
	}
	
	/** Initializes all data fields with fixed default values.
	 */
	private void initFields() {
		startDate = new Date();
		boxSize = 79;
		spacing = 2;
		mX = 0;
		mY = 0;
		squareValues = new int[16][9];
		squareStates  = new int[16][9];
		loss = false;
		win = false;
		smileyX = 597;
		smileyY = 5;
		bX = -1;
		bY = -1;	
	} //end initFields
	
	/** Returns true if the mouse is over the smiley face, else returns false.
	 *@return - boolean defining whether mouse is over the smiley face.
	 */
	private boolean inSmiley() {
		int xCenter = smileyX + 35;
		int yCenter = smileyY + 35;
		
		int dif = (int)Math.sqrt(Math.pow(xCenter - mX, 2) + Math.pow(yCenter - mY, 2));
		if (dif <= 35) {
			return true;
		}
		return false;		
	}
	
	/** Store mines in the grid randomly.
	 */
	private void storeMines() {
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
	}//end storeMines
	
	/** Checks the win condition and sets the boolean field to true when the game is complete.
	 */
	public void checkVictoryStatus() {
		if (totalBoxesRevealed() + totalMines() == 144) {
			win = true;
		}
	}
	
	/** Returns the total number of mines in the grid.
	 *@return - the number of mines.
	 */
	private int totalMines() {
		int numMines = 0;
		for(int i = 0; i < 16; i++) {
			for(int j = 0; j < 9; j++) {
				if (squareValues[i][j] == 9) {
					numMines++;
				}
			}
		}
		return numMines;
	}
	
	/** Returns the number of revealed boxes.
	 *@return - The number of revealed grid squares.
	 */
	private int totalBoxesRevealed() {
		int revealedBoxes = 0;
		for(int i = 0; i < 16; i++) {
			for(int j = 0; j < 9; j++) {
				if (squareStates[i][j] == 0) {
					revealedBoxes++;
				}
			}
		}
		return revealedBoxes;
	}
	
	private void resetAll() {
		resetter = true;
		initFields();
		storeMines();
		setUpNeighbourValues();
		resetter = false;
	}
	
	/** Determine int values of neighbouring mines and store them in the array. 
	 */
	private void setUpNeighbourValues() {
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
	}// end setUpNeighbourValues
	
	/** Returns the x index value of the box currently hovered.
	 *@return - the x index of the currently hovered box.
	 */
	private int inBoxX() {
		for(int i = 0; i < 16; i++) {
			int boxXPos = spacing + i * boxSize;
			int boxSize = this.boxSize - 2 * spacing;
			// return the x box index currently hovered with the mouse.
			if (mX >= boxXPos && mX < boxXPos + boxSize) {
				return i;
			}
		}
		return -1;
	}// end inBoxX
	
	/** Returns the y index value of the box currently hovered.
	 *@return - the y index of the currently hovered box.
	 */
	private int inBoxY() {
		for(int j = 0; j < 9; j++) {
			int boxYPos = spacing + boxSize + j * boxSize;
			int boxSize = this.boxSize - 2 * spacing;
			// return the y box index currently hovered with the mouse.
			if (mY >= boxYPos && mY < boxYPos + boxSize) {
				return j;
			}
		}
		return -1;
	}// end inBoxY
	
	/** Returns the color, dependent on the value of neighbouring mines.
	 *@param n - the number of neighbouring mines.
	 *@return - the color associated with the input value.
	 */
	private Color getColor(int n) {
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
	}// end getColor
	
	
	/** Determines if a box is neighbouring and contains a mine.
	 *@param mx - the neighbouring box x index being checked.
	 *@param my - the neighbouring box y index being checked.
	 *@param cx - the current x index being checked.
	 *@param cy - the current y index being checked.
	 */
	private boolean isN(int mx, int my, int cx, int cy) {
		if((mx - cx) < 2 && (mx - cx) > -2 && (my - cy) < 2 && (my - cy) > -2 && squareValues[cx][cy] == 9) {
			return true;
		}
		return false;
	}// end isN
	
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
			// flag or un-flag boxes
			if(SwingUtilities.isRightMouseButton(e)) {
				return;
			}
			//smiley button pressed
			if (inSmiley()) {
				resetAll();
				return;
			}
			// do nothing if no box is clicked or game is finished.
			if (x == -1 || y == -1 || loss || win) {
				return;
			}
			//ignore flagged boxes
			if (squareStates[x][y] == -1) {
				return;
			}
			// set loss when a mine is revealed.
			if (squareValues[x][y] == 9) {
				loss = true;
			}
			// set square state to revealed if unflagged.
			if(squareStates[x][y] == 1) {
				squareStates[x][y] = 0;
				if (squareValues[x][y] == 0) {
					chainReveal(x, y);
				}
			}
		}

		@Override
		public void mousePressed(MouseEvent e) {
			if (loss || win) {
				return;
			}
			if(SwingUtilities.isRightMouseButton(e)) {
				if (inBoxX() == -1 || inBoxY() == -1) {
					return;
				} else if(squareStates[inBoxX()][inBoxY()] != 0) {
						squareStates[inBoxX()][inBoxY()] *= -1;
				}
				return;
			}
			// check if smiley was clicked
			if (inSmiley()) {
				bX = 16;
				bY = 16;
			// only save pressed box if box is clicked and game is not lost.
			} else if (!(inBoxX() == -1 || inBoxY() == -1) && !loss) {
				bX = inBoxX();
				bY = inBoxY();
			}
		}

		@Override
		public void mouseReleased(MouseEvent e) {			
			if ((bX == inBoxX() && bY == inBoxY()) || bY == 16) {
				//reveal square)
				if(SwingUtilities.isLeftMouseButton(e)) {
					if (inSmiley() && bX == 16) {
						resetAll();
						return;
					}
					// do nothing if game is lost.
					if (loss) {
						return;
					}
					// do nothing if no box is clicked.
					if (inBoxX() == -1 || inBoxY() == -1) {
						return;
					}
					//ignore flagged boxes
					if(squareStates[inBoxX()][inBoxY()] == -1) {
						return;
					}
					// set loss when a mine is revealed
					if (squareValues[inBoxX()][inBoxY()] == 9) {
						loss = true;
					}
					//reveal unflagged boxes.
					if (squareStates[inBoxX()][inBoxY()] == 1) {
						squareStates[inBoxX()][inBoxY()] = 0;
						if (squareValues[inBoxX()][inBoxY()] == 0) {
							chainReveal(inBoxX(), inBoxY());
						}
					}
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
