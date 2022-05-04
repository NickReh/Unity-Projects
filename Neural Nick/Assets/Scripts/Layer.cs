using System;

public class Layer {
    private float[] inputs;
    public float[] Inputs { get { return this.inputs; } set {this.inputs = value; } }
    private Node[] nodes;
    public Node[] Nodes { get { return this.nodes; } set {this.nodes = value; } }
    private float net;
    public float Net { get { return this.net; } set {this.net = value; } }


    public Layer (int numberOfNodes, int numberOfInputs) {
        //init inputs
        inputs = new float[numberOfInputs];
        for(int i = 0; i < numberOfInputs; i++){
            this.inputs[i] = 0.0f;
        }

        //init nodes
        nodes = new Node[numberOfNodes];
        for(int i = 0; i < numberOfNodes; i++){
            this.nodes[i] = new Node(numberOfInputs);
        }
    }

    public void FeedForward () {
        foreach(Node node in this.nodes){
			this.net = node.Threshold;
			for(var i = 0; i < node.Weight.Length; i++){
				this.net += this.inputs[i] * node.Weight[i];
			}
			node.Output = Sigmoid(this.net);
        }
    }

    public float Sigmoid (float net) {
        return (float)(1 / (1 + Math.Exp(-net)));
    }

    public float[] OutputVector () {
        float[] vectors = new float[this.nodes.Length];
        
        for(int i = 0; i < vectors.Length; i++){
            vectors[i] = this.nodes[i].Output;
        }

        return vectors;
    }
}