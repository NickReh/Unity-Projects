using System;

public class BackProp {
    private Layer[] layers;
    public Layer[] Layers { get { return this.layers; } set {this.layers = value; } }
    private float[] actualOutput;
    public float[] ActualOutput { get { return this.actualOutput; } set {this.actualOutput = value; } }
    private float learningRate;
    public float LearningRate { get { return this.learningRate; } set {this.learningRate = value; } }
    private float momentum;
    public float Momentum { get { return this.momentum; } set {this.momentum = value; } }

    public BackProp (int[] numNodesInLayers, float learnRate, float momentum, float minError) {
        this.learningRate = learnRate;
        this.momentum = momentum;

        //init layers
        this.layers = new Layer[numNodesInLayers.Length];
        this.layers[0] = new Layer(numNodesInLayers[0], numNodesInLayers[0]);
        for(int i = 1; i < numNodesInLayers.Length; i++){
            this.layers[i] = new Layer(numNodesInLayers[i], numNodesInLayers[i - 1]);
        }

        this.actualOutput = new float[this.layers[numNodesInLayers.Length - 1].Nodes.Length];
    }

    public void FeedForward () {
        for(int i = 0; i < this.layers[0].Nodes.Length; i++){
			this.layers[0].Nodes[i].Output = this.layers[0].Inputs[i];
		}
		this.layers[1].Inputs = this.layers[0].Inputs;
		for(int j = 1; j < layers.Length; j++){
			this.layers[j].FeedForward();
			if(j != layers.Length - 1){
				this.layers[j + 1].Inputs = this.layers[j].OutputVector();
			}
		}
    }

    public void UpdateWeights (float[] output) {
        this.CalculateSignalErrors(output);
		this.BackPropagateError();
    }

    public void CalculateSignalErrors (float[] output) {
        float sum;
		int outputLayer = this.layers.Length - 1;
		for(int n = 0; n < this.layers[outputLayer].Nodes.Length; n++){
			this.layers[outputLayer].Nodes[n].SignalError = (output[n] - this.layers[outputLayer].Nodes[n].Output) *
															this.layers[outputLayer].Nodes[n].Output * (1 - this.layers[outputLayer].Nodes[n].Output);
		}
		for(int i = this.layers.Length - 2; i > 0; i--){
			for(int j = 0; j < this.layers[i].Nodes.Length; j++){
				sum = 0;
				for(int k = 0; k < this.layers[i + 1].Nodes.Length; k++){
					sum += this.layers[i + 1].Nodes[k].Weight[j] * this.layers[i + 1].Nodes[k].SignalError;
				}
				this.layers[i].Nodes[j].SignalError = this.layers[i].Nodes[j].Output * (1 - this.layers[i].Nodes[j].Output) * sum;
			}
		}
    }

    public void BackPropagateError () {
        for(int i = this.layers.Length - 1; i > 0; i--){
			for(int j = 0; j < this.layers[i].Nodes.Length; j++){
				this.layers[i].Nodes[j].ThresholdDiff = this.learningRate * this.layers[i].Nodes[j].SignalError + this.momentum * this.layers[i].Nodes[j].ThresholdDiff;
				this.layers[i].Nodes[j].Threshold += this.layers[i].Nodes[j].ThresholdDiff;
				for(int k = 0; k < this.layers[i].Inputs.Length; k++){
					this.layers[i].Nodes[j].WeightDiff[k] = this.learningRate * this.layers[i].Nodes[j].SignalError * this.layers[i - 1].Nodes[k].Output + this.momentum * this.layers[i].Nodes[j].WeightDiff[k];
					this.layers[i].Nodes[j].Weight[k] += this.layers[i].Nodes[j].WeightDiff[k];
				}
			}
		}
    }

    public float CalculateOverallError (float[] expectedOutput) {
        float overallError = 0;

		for(var j = 0; j < this.layers[this.layers.Length - 1].Nodes.Length; j++){
			overallError += 0.5f * ((float)Math.Pow(expectedOutput[j] - this.actualOutput[j], 2));
		}

        return overallError;
    }

    public void TrainNetwork (float[] inputs, float[] output) {
        if(inputs.Length != this.layers[0].Nodes.Length) {
			return;
        }
							
		for(int i = 0; i < this.layers[0].Nodes.Length; i++){
			this.layers[0].Inputs[i] = inputs[i];
		}
		this.FeedForward();
		for(int j = 0; j < this.layers[this.layers.Length - 1].Nodes.Length; j++){
			this.actualOutput[j] = this.layers[this.layers.Length - 1].Nodes[j].Output;
		}
		this.UpdateWeights(output);
    }

    public int Test (float[] inputs) {
        int winner = 0;
		Node[] output_nodes;

		for(int j = 0; j < this.layers[0].Nodes.Length; j++){
			this.layers[0].Inputs[j] = inputs[j];
		}

		this.FeedForward();

		output_nodes = this.layers[this.layers.Length - 1].Nodes;
		for(int k = 0; k < output_nodes.Length; k++){
			if(output_nodes[winner].Output < output_nodes[k].Output){
				winner = k;
			}
		}
		return winner;
    }

    public float GetError (float[] expectedOutput) {
		return this.CalculateOverallError(expectedOutput);
	}
}