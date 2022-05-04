using System.Collections.Generic;
using UnityEngine;

public class Nick : MonoBehaviour 
{
     private Brain brain;
    public enum facingPos 
    {
        u, ur, r, dr, d, dl, l, ul
    }

	public int headPosRow;
	public int headPosColumn;
	public int facing = 0;
	public List<int> lastMoves = new List<int>();
	private int cellsPerRow = 0;
    public float nicksSpeed = 1.0f;
    private float timer = 0;

	public void Init (int worldSize) {
        this.brain = new Brain();
        this.cellsPerRow = worldSize;
        this.headPosRow = worldSize / 2;
        this.headPosColumn = worldSize / 2;
        this.transform.position = new Vector3(this.headPosRow, 0.125f, this.headPosColumn - 0.5f);
	}

    void Start () {

    }
	
	void Update () {        
        if (cellsPerRow == 0) {
            return;
        }

        timer += Time.deltaTime;

        if (timer > nicksSpeed) 
        {
            timer -= nicksSpeed;
        } 
        else 
        {
            return;
        }

		List<float> nicksView = GetNicksView();
        
		int newDirection = this.brain.GetNextMove(nicksView.ToArray());
        
		if (lastMoves.Count == 0) 
        {
			lastMoves.Add(newDirection);
		} 
        else 
        {
			for (int updateCount = lastMoves.Count - 1; updateCount > 0; updateCount--) 
            {
				lastMoves [updateCount] = lastMoves [updateCount - 1];
			}

			lastMoves [0] = newDirection;

			if (lastMoves.Count > 16) 
            {
				lastMoves.RemoveAt(lastMoves.Count - 1);
			}
		}

		switch(newDirection) 
        {
            case 0:
                if (!IsWallToLeft())
                {
                    TurnLeft();
                }
                break;
            case 1:
                if (!IsWallInFront())
                {
                    MoveForward();
                }
                break;
            case 2:
                if (!IsWallToRight())
                {
                    TurnRight();
                }
                break;
		}

        SetPositionAndRotation();

        CheckAteFood();
        
        //brain.UpdateBrain();
	}

    public void SetPositionAndRotation() 
    {
        switch (this.facing) 
        {
            case 0: //up
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 1:
                this.transform.rotation = Quaternion.Euler(0, 45, 0);
                break;
            case 2: //right
                this.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case 3:
                this.transform.rotation = Quaternion.Euler(0, 135, 0);
                break;
            case 4: //down
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case 5:
                this.transform.rotation = Quaternion.Euler(0, -135, 0);
                break;
            case 6: //left
                this.transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case 7:
                this.transform.rotation = Quaternion.Euler(0, -45, 0);
                break;
        } 

        this.transform.position = new Vector3(this.headPosRow, 0.125f, this.headPosColumn - 0.5f);
    }

    private void CheckAteFood()
    {
        if (WorldManager.Grid[this.headPosRow * this.cellsPerRow + this.headPosColumn] == 1)
        {

        }
    }

    public List<float> GetNicksView()
    {
		int[] g = WorldManager.Grid;
		int cpr = 12; //World.cellsPerRow;
		int r = headPosRow;
		int c = headPosColumn;
		List<float> ng = new List<float>();
		switch (facing) {
            case 0: //up
                ng = new List<float>{g[(r-4)*cpr+(c-2)], g[(r-4)*cpr+(c-1)], g[(r-4)*cpr+c], g[(r-4)*cpr+(c+1)], g[(r-4)*cpr+(c+2)],
                                g[(r-3)*cpr+(c-2)], g[(r-3)*cpr+(c-1)], g[(r-3)*cpr+c], g[(r-3)*cpr+(c+1)], g[(r-3)*cpr+(c+2)],
                                g[(r-2)*cpr+(c-2)], g[(r-2)*cpr+(c-1)], g[(r-2)*cpr+c], g[(r-2)*cpr+(c+1)], g[(r-2)*cpr+(c+2)],
                                    g[(r-1)*cpr+(c-2)], g[(r-1)*cpr+(c-1)], g[(r-1)*cpr+c], g[(r-1)*cpr+(c+1)], g[(r-1)*cpr+(c+2)],
                                    g[r*cpr+(c-2)],     g[r*cpr+(c-1)],                     g[r*cpr+(c+1)],     g[r*cpr+(c+2)]};
                    break;
                case 1: //up-right
                ng = new List<float>{g[(r-6)*cellsPerRow+(c+2)], g[(r-5)*cellsPerRow+(c+3)], g[(r-4)*cellsPerRow+(c+4)], g[(r-3)*cellsPerRow+(c+5)], g[(r-2)*cellsPerRow+(c+6)],
                        g[(r-5)*cellsPerRow+(c+1)], g[(r-4)*cellsPerRow+(c+2)], g[(r-3)*cellsPerRow+(c+3)], g[(r-2)*cellsPerRow+(c+4)], g[(r-1)*cellsPerRow+(c+5)],
                        g[(r-4)*cellsPerRow+c],     g[(r-3)*cellsPerRow+(c+1)], g[(r-2)*cellsPerRow+(c+2)], g[(r-1)*cellsPerRow+(c+3)],     g[r*cellsPerRow+(c+4)],
                        g[(r-3)*cellsPerRow+(c-1)], g[(r-2)*cellsPerRow+c],     g[(r-1)*cellsPerRow+(c+1)],     g[r*cellsPerRow+(c+2)], g[(r+1)*cellsPerRow+(c+3)],
                        g[(r-2)*cellsPerRow+(c-2)], g[(r-1)*cellsPerRow+(c-1)],                                  g[(r+1)*cellsPerRow+(c+1)], g[(r+2)*cellsPerRow+(c+2)]};
                    break;
                case 2: //right
                ng = new List<float> {g[(r-2)*cellsPerRow+(c+4)], g[(r-1)*cellsPerRow+(c+4)], g[r*cellsPerRow+(c+4)], g[(r+1)*cellsPerRow+(c+4)], g[(r+2)*cellsPerRow+(c+4)],
                        g[(r-2)*cellsPerRow+(c+3)], g[(r-1)*cellsPerRow+(c+3)], g[r*cellsPerRow+(c+3)], g[(r+1)*cellsPerRow+(c+3)], g[(r+2)*cellsPerRow+(c+3)],
                        g[(r-2)*cellsPerRow+(c+2)], g[(r-1)*cellsPerRow+(c+2)], g[r*cellsPerRow+(c+2)], g[(r+1)*cellsPerRow+(c+2)], g[(r+2)*cellsPerRow+(c+2)],
                        g[(r-2)*cellsPerRow+(c+1)], g[(r-1)*cellsPerRow+(c+1)], g[r*cellsPerRow+(c+1)], g[(r+1)*cellsPerRow+(c+1)], g[(r+2)*cellsPerRow+(c+1)],
                    g[(r-2)*cellsPerRow+c],     g[(r-1)*cellsPerRow+c],                                  g[(r+1)*cellsPerRow+c],     g[(r+2)*cellsPerRow+c]};
                    break;
                case 3: //down-right
                ng = new List<float> {g[(r+2)*cellsPerRow+(c+6)], g[(r+3)*cellsPerRow+(c+5)], g[(r+4)*cellsPerRow+(c+4)], g[(r+5)*cellsPerRow+(c+3)], g[(r+6)*cellsPerRow+(c+2)],
                        g[(r+1)*cellsPerRow+(c+5)], g[(r+2)*cellsPerRow+(c+4)], g[(r+3)*cellsPerRow+(c+3)], g[(r+4)*cellsPerRow+(c+2)], g[(r+5)*cellsPerRow+(c+1)],
                        g[r*cellsPerRow+(c+4)], g[(r+1)*cellsPerRow+(c+3)], g[(r+2)*cellsPerRow+(c+2)], g[(r+3)*cellsPerRow+(c+1)], g[(r+4)*cellsPerRow+c],
                        g[(r-1)*cellsPerRow+(c+3)],     g[r*cellsPerRow+(c+2)], g[(r+1)*cellsPerRow+(c+1)], g[(r+2)*cellsPerRow+c],     g[(r+3)*cellsPerRow+(c-1)],
                    g[(r-2)*cellsPerRow+(c+2)], g[(r-1)*cellsPerRow+(c+1)],                                  g[(r+1)*cellsPerRow+(c-1)], g[(r+2)*cellsPerRow+(c-2)]};
                    break;
                case 4: //down
                ng = new List<float> {g[(r+4)*cellsPerRow+(c+2)], g[(r+4)*cellsPerRow+(c+1)], g[(r+4)*cellsPerRow+c], g[(r+4)*cellsPerRow+(c-1)], g[(r+4)*cellsPerRow+(c-2)],
                        g[(r+3)*cellsPerRow+(c+2)], g[(r+3)*cellsPerRow+(c+1)], g[(r+3)*cellsPerRow+c], g[(r+3)*cellsPerRow+(c-1)], g[(r+3)*cellsPerRow+(c-2)],
                        g[(r+2)*cellsPerRow+(c+2)], g[(r+2)*cellsPerRow+(c+1)], g[(r+2)*cellsPerRow+c], g[(r+2)*cellsPerRow+(c-1)], g[(r+2)*cellsPerRow+(c-2)],
                        g[(r+1)*cellsPerRow+(c+2)], g[(r+1)*cellsPerRow+(c+1)], g[(r+1)*cellsPerRow+c], g[(r+1)*cellsPerRow+(c-1)], g[(r+1)*cellsPerRow+(c-2)],
                    g[r*cellsPerRow+(c+2)],     g[r*cellsPerRow+(c+1)],                                  g[r*cellsPerRow+(c-1)],     g[r*cellsPerRow+(c-2)]};
                    break;
                case 5:	//down-left
                ng = new List<float> {g[(r+6)*cellsPerRow+(c-2)], g[(r+5)*cellsPerRow+(c-3)], g[(r+4)*cellsPerRow+(c-4)], g[(r+3)*cellsPerRow+(c-5)], g[(r+2)*cellsPerRow+(c-6)],
                        g[(r+5)*cellsPerRow+(c-1)], g[(r+4)*cellsPerRow+(c-2)], g[(r+3)*cellsPerRow+(c-3)], g[(r+2)*cellsPerRow+(c-4)], g[(r+1)*cellsPerRow+(c-5)],
                        g[(r+4)*cellsPerRow+c],     g[(r+3)*cellsPerRow+(c-1)], g[(r+2)*cellsPerRow+(c-2)], g[(r+1)*cellsPerRow+(c-3)],     g[r*cellsPerRow+(c-4)],
                        g[(r+3)*cellsPerRow+(c+1)], g[(r+2)*cellsPerRow+c],     g[(r+1)*cellsPerRow+(c-1)],     g[r*cellsPerRow+(c-2)], g[(r-1)*cellsPerRow+(c-3)],
                    g[(r+2)*cellsPerRow+(c+2)], g[(r+1)*cellsPerRow+(c+1)],                                  g[(r-1)*cellsPerRow+(c-1)], g[(r-2)*cellsPerRow+(c-2)]};
                    break;
                case 6: //left
                ng = new List<float> {g[(r+2)*cellsPerRow+(c-4)], g[(r+1)*cellsPerRow+(c-4)], g[r*cellsPerRow+(c-4)], g[(r-1)*cellsPerRow+(c-4)], g[(r-2)*cellsPerRow+(c-4)],
                        g[(r+2)*cellsPerRow+(c-3)], g[(r+1)*cellsPerRow+(c-3)], g[r*cellsPerRow+(c-3)], g[(r-1)*cellsPerRow+(c-3)], g[(r-2)*cellsPerRow+(c-3)],
                        g[(r+2)*cellsPerRow+(c-2)], g[(r+1)*cellsPerRow+(c-2)], g[r*cellsPerRow+(c-2)], g[(r-1)*cellsPerRow+(c-2)], g[(r-2)*cellsPerRow+(c-2)],
                        g[(r+2)*cellsPerRow+(c-1)], g[(r+1)*cellsPerRow+(c-1)], g[r*cellsPerRow+(c-1)], g[(r-1)*cellsPerRow+(c-1)], g[(r-2)*cellsPerRow+(c-1)],
                    g[(r+2)*cellsPerRow+c],     g[(r+1)*cellsPerRow+c],                                  g[(r-1)*cellsPerRow+c],     g[(r-2)*cellsPerRow+c]};
                    break;
                case 7: //up-left
                ng = new List<float> {g[(r-2)*cellsPerRow+(c-6)], g[(r-3)*cellsPerRow+(c-5)], g[(r-4)*cellsPerRow+(c-4)], g[(r-5)*cellsPerRow+(c-3)], g[(r-6)*cellsPerRow+(c-2)],
                        g[(r-1)*cellsPerRow+(c-5)], g[(r-2)*cellsPerRow+(c-4)], g[(r-3)*cellsPerRow+(c-3)], g[(r-4)*cellsPerRow+(c-2)], g[(r-5)*cellsPerRow+(c-1)],
                        g[r*cellsPerRow+(c-4)], g[(r-1)*cellsPerRow+(c-3)], g[(r-2)*cellsPerRow+(c-2)], g[(r-3)*cellsPerRow+(c-1)], g[(r-4)*cellsPerRow+c],
                        g[(r+1)*cellsPerRow+(c-3)],     g[r*cellsPerRow+(c-2)], g[(r-1)*cellsPerRow+(c-1)], g[(r-2)*cellsPerRow+c],     g[(r-3)*cellsPerRow+(c+1)],
                    g[(r+2)*cellsPerRow+(c-2)], g[(r+1)*cellsPerRow+(c-1)],                                  g[(r-1)*cellsPerRow+(c+1)], g[(r-2)*cellsPerRow+(c+2)]};
                    break;
		}
		return ng;
	}
	
	private void TurnLeft()
    {
		switch (facing) {
            case 0: //up
                headPosColumn--;
                break;
            case 1:
                headPosColumn--;
                break;
            case 2: //right
                headPosRow--;
                break;
            case 3:
                headPosRow--;
                break;
            case 4: //down
                headPosColumn++;
                break;
            case 5:
                headPosColumn++;
                break;
            case 6: //left
                headPosRow++;
                break;
            case 7:
                headPosRow++;
                break;
		}

		facing--;

		if (facing < 0) {
		    facing = 7;
        }
	}
	public void MoveForward()
    {
		switch(facing)
        {
            case 0:
                headPosRow--;
                break;
            case 1:
                headPosColumn++;
                headPosRow--;
                break;
            case 2:
                headPosColumn++;
                break;
            case 3:
                headPosColumn++;
                headPosRow++;
                break;
            case 4:
                headPosRow++;
                break;
            case 5:
                headPosColumn--;
                headPosRow++;
                break;
            case 6:
                headPosColumn--;
                break;
            case 7:
                headPosColumn--;
                headPosRow--;
                break;
		}
	}
	private void TurnRight()
    {
		switch(facing){
            case 0: //up
                headPosColumn++;
                break;
            case 1:
                headPosRow++;
                break;
            case 2: //right
                headPosRow++;
                break;
            case 3:
                headPosColumn--;
                break;
            case 4: //down
                headPosColumn--;
                break;
            case 5:
                headPosRow--;
                break;
            case 6: //left
                headPosRow--;
                break;
            case 7:
                headPosColumn++;
                break;
		}

		facing++;

		if (facing > 7) {
		    facing = 0;
        }
	}
	public bool IsWallToLeft()
    {
		int[] g = WorldManager.Grid;
		int r = headPosRow;
		int c = headPosColumn;
		switch(facing){
            case 0: //up
                return g[r*cellsPerRow+(c-1)] == -1;
            case 1:
                return g[r*cellsPerRow+(c-1)] == -1;
            case 2: //right
                return g[(r-1)*cellsPerRow+c] == -1;
            case 3:
                return g[(r-1)*cellsPerRow+c] == -1;
            case 4: //down
                return g[r*cellsPerRow+(c+1)] == -1;
            case 5:
                return g[r*cellsPerRow+(c+1)] == -1;
            case 6: //left
                return g[(r+1)*cellsPerRow+c] == -1;
            case 7:
                return g[(r+1)*cellsPerRow+c] == -1;
		}
		return true;
	}
	public bool IsWallInFront()
    {
		int[] g = WorldManager.Grid;
		int r = headPosRow;
		int c = headPosColumn;
		switch (facing) {
            case 0: //up
                return g[(r-1)*cellsPerRow+c] == -1;
            case 1:
                return g[(r-1)*cellsPerRow+(c+1)] == -1;
            case 2: //right
                return g[r*cellsPerRow+(c+1)] == -1;
            case 3:
                return g[(r+1)*cellsPerRow+(c+1)] == -1;
            case 4: //down
                return g[(r+1)*cellsPerRow+c] == -1;
            case 5:
                return g[(r+1)*cellsPerRow+(c-1)] == -1;
            case 6: //left
                return g[r*cellsPerRow+(c-1)] == -1;
            case 7:
                return g[(r-1)*cellsPerRow+(c-1)] == -1;
		}
		return true;
	}
	public bool IsWallToRight()
    {
		int[] g = WorldManager.Grid;
		int r = headPosRow;
		int c = headPosColumn;
		switch (facing) {
            case 0: //up
                return g[r*cellsPerRow+(c+1)] == -1;
            case 1:
                return g[(r+1)*cellsPerRow+c] == -1;
            case 2: //right
                return g[(r+1)*cellsPerRow+c] == -1;
            case 3:
                return g[r*cellsPerRow+(c-1)] == -1;
            case 4: //down
                return g[r*cellsPerRow+(c-1)] == -1;
            case 5:
                return g[(r-1)*cellsPerRow+c] == -1;
            case 6: //left
                return g[(r-1)*cellsPerRow+c] == -1;
            case 7:
                return g[r*cellsPerRow+(c+1)] == -1;
		}
		return true;
	}
	public bool IsSpinning()
    {
		return lastMoves.Count > 15 &&
		(
			(lastMoves.IndexOf(0) == -1 && lastMoves.IndexOf(1) == -1)
			||
			(lastMoves.IndexOf(1) == -1 && lastMoves.IndexOf(2) == -1)
			||
			(
				lastMoves[0] == lastMoves[2] && lastMoves[0] == lastMoves[4] && lastMoves[0] == lastMoves[6] &&
				lastMoves[0] == lastMoves[8] && lastMoves[0] == lastMoves[10] && lastMoves[0] == lastMoves[12] &&
				lastMoves[0] == lastMoves[14]
				&&
				lastMoves[1] == lastMoves[3] && lastMoves[1] == lastMoves[5] && lastMoves[1] == lastMoves[7] &&
				lastMoves[1] == lastMoves[9] && lastMoves[1] == lastMoves[11] && lastMoves[1] == lastMoves[13] &&
				lastMoves[1] == lastMoves[15]
				&&
				(lastMoves[0] != lastMoves[1])
			)
		);
	}
}