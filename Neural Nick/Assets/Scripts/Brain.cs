public class Brain{
    private int[] neurons;
    private BackProp net;
    public BackProp Net { get { return this.net; } set {this.net = value; } }

    public Brain () {
        this.neurons = new int[] { 24, 14, 3 };
        this.net = new BackProp(this.neurons, 0.6f, 0.9f, 0.01f);
        float[][] Inputs = new float[][] {new float[] {0,0,0,0,0,
							  0,0,0,0,0,
							  0,0,-1,-1,0,
							  0,0,-1,-1,0,
							  0,0,  0,0},
							  new float[] {0,0,0,0,0,
							  0,0,0,0,0,
							  0,0,-1,-1,-1,
							  0,0,-1,-1,-1,
							  0,0,  0,0},
							  new float[] {0,0,0,0,0,
							  0,0,0,0,0,
							  0,-1,-1,0,0,
							  0,-1,-1,0,0,
							  0,0,  0,0},
							  new float[] {-1,-1,-1,-1,-1,
							  -1,-1,-1,-1,-1,
							  -1,-1,-1,-1,-1,
							  -1,-1,-1,-1,-1,
							  0,0,  0,0},
							  new float[] {-1,-1,-1,-1,-1,
							  -1,-1,-1,-1,-1,
							  -1,-1,-1,-1,0,
							  -1,-1,-1,0,0,
							  -1,-1,  0,0},
							  new float[] {-1,-1,-1,-1,-1,
							  -1,-1,-1,-1,-1,
							  -1,-1,-1,-1,-1,
							  0,0,0,0,0,
							  0,0,  0,0},
							  new float[] {0,0,0,-1,-1,
							  0,0,0,-1,-1,
							  0,0,0,-1,-1,
							  0,0,0,-1,-1,
							  0,0,  -1,-1},
							  new float[] {-1,-1,0,0,0,
							  -1,-1,0,0,0,
							  -1,-1,0,0,0,
							  -1,-1,0,0,0,
                              -1,-1 ,0,0},
							  new float[] {0,0,0,0,0,
							  0,0,0,0,0,
							  0,0,0,0,0,
							  -1,-1,0,0,0,
							  0,0,  0,0},
							  new float[] {0,0,0,0,0,
							  0,0,0,0,0,
							  0,0,0,0,0,
							  0,0,0,0,0,
							  -1,-1,  0,0},
							  new float[] {0,-1,0,0,0,
							  0,-1,0,0,0,
							  0,-1,0,0,0,
							  0,-1,0,0,0,
							  0,-1,  0,0},
							  new float[] {0,0,0,-1,0,
							  0,0,0,-1,0,
							  0,0,0,-1,0,
							  0,0,0,-1,0,
							  0,0,  -1,0},
							  new float[] {0,0,0,0,0,
							  0,0,0,0,0,
							  0,0,0,0,0,
							  0,0,0,-1,-1,
							  0,0,  0,0},
							  new float[] {-1,-1,-1,-1,-1,
							  -1,-1,-1,-1,-1,
							  0,-1,-1,-1,-1,
							  0,0,-1,-1,-1,
							  0,0,  -1,-1}};
        int[] Wants = new int[] {2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 2, 0, 1, 2};
        for(var i = 0; i < 1000; i++){
            for(var j = 0; j < 14; j++){
                //console.log("Test: " + i + " " + "Want: " + Wants[j] + " Output: " + this.net.Test(Inputs[j]));
                float[] Output = new float[3];
                switch(Wants[j]){
                    case 0:
                        Output = new float[] {1,0,0};
                        break;
                    case 1:
                        Output = new float[] {0,1,0};
                        break;
                    case 2:
                        Output = new float[] {0,0,1};
                        break;
                }
                this.net.TrainNetwork(Inputs[j], Output);
            }
        }
    }

    public int GetNextMove(float[] viewInputs)
    {
        return this.net.Test(viewInputs);
    }

    public void UpdateBrain()
    {

    }
}