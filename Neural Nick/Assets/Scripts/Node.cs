using System;

public class Node {
    private int numberOfInputs;
    public int NumberOfInputs { get { return this.numberOfInputs; } set {this.numberOfInputs = value; }}
    private float output;
    public float Output { get { return this.output; } set {this.output = value; } }
    private float[] weight;
    public float[] Weight { get { return this.weight; } set {this.weight = value; } }
    private float[] weightDiff;
    public float[] WeightDiff { get { return this.weightDiff; } set {this.weightDiff = value; } }
    private float threshold;
    public float Threshold { get { return this.threshold; } set {this.threshold = value; } }
    private float thresholdDiff = 0;
    public float ThresholdDiff { get { return this.thresholdDiff; } set {this.thresholdDiff = value; } }
    private float signalError;
    public float SignalError { get { return this.signalError; } set {this.signalError = value; } }

    public Node (int numberOfInputs) {
        this.numberOfInputs = numberOfInputs;
        Random rnd = new Random();
        this.Threshold = (float)(-1 + 2 * rnd.NextDouble());
        this.initWeights();
    }

    private void initWeights() {
        this.weight = new float[this.numberOfInputs];
        this.weightDiff = new float[this.numberOfInputs];
        Random rnd = new Random();

        for(int i = 0; i < this.numberOfInputs; i++){
            this.weight[i] = (float)(-1 + 2 * rnd.NextDouble());
            this.weightDiff[i] = 0;
        }
    }
}