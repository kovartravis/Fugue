/*
 *The BinaryTree class is designed to hold dialogue options for NPCs. Each node of the binary tree 
 * can hold one pointer, in which case it acts as a linked list, or two pointers in which case the 
 * player will be presented with the option to take the right or left branch of the tree. This has 
 * advantages and limitations, which will be covered later.
 * 
 * Some Terminology:
 * The first node is called the root node. A node that has other nodes connected to it is called 
 * a parent node. A node that is connected to other nodes above it is called a child node. A node 
 * at the tip of the tree is called a leaf. There can be only one root. Each child can only 
 * have one parent. A node can be both a child and parent, and the root is usually also a parent.
 * 
 * A sample binary tree:
 * 											[a]			
 * 											/\
 * 										 [b]  [c]
 *										 /	  / \	 
 * 								 	   [d] 	[h] [e]
 * 									   /\		
 * 								    [f] [g]				
 * As you can see not every parent needs to have two children.
 * 
 * A binary tree for our purposes:
 * 
 * 								["Hello, what can I do for you?"] 
 *						["What is your name?"]["Heard any good rumors?"]	 
 * 								/					\
 *							   / 					 \
 * 					["Its a me, Mario!"]			["Something is happening at midnight"]
 * 											["What is happening?"]["Where did you here that from?"]
 * 													/						\
 * 												   /						 \
 *										["A clan raid!"] 				["At the pub"]
 *															["How drunk were you?"]["Which pub?"]
 *																	/					\
 *																   /					\
 *														["Only a bit!"]				["O'nealys"]
 *
 * This structure allows complex dialogue to be coded in a simple manner. 
 * 
 * In terms of limitations, notice that there is no tree shape like this:
 * 
 * 											[a]
 * 											/\						
 * 										 [b] [c]
 * 										  \  /
 * 										  [d]		
 * 
 * If we want an npc to say the same thing after both options, they cannot link to the same node. 
 * This would mean a node has two parents which is not possible. Such a data structure exists, but
 * in my opinion would be too complex to manage for such a simple application as dialogue boxes.
 * So, if we want an npc to say the same thing after either option, we need two nodes which will be 
 * copies of each other. 
 */			

using System;
using UnityEngine;
using System.Collections;

public class Dialogue{	
	
	private string main;
	private string option_ForkLeft = null;
	private string option_ForkRight = null;
	//contains the data in the node
	public string Main{
		get{return main;}					//the main dialogue box message, must have a value
		set{main = value;}
	}					
	public string Option_ForkLeft{
		get{return option_ForkLeft;}		//the messages for the two option boxes
		set{option_ForkLeft = value;}
	}							
	public string Option_ForkRight{
		get{return option_ForkRight;}		//these are null for a leaf node
		set{option_ForkRight = value;}
	}	
	public Dialogue(){
	}
	public Dialogue(string local_main){
		Main = local_main;
	}

	public Dialogue(string local_main, string local_option_ForkLeft, string local_option_ForkRight){
		Main = local_main;
		Option_ForkLeft = local_option_ForkLeft;
		Option_ForkRight = local_option_ForkRight;
	}
	
}
public class binaryTree{
	
	public class Node : Dialogue{

		private Node forkLeft;				//pointer left
		private Node forkRight;				//pointer right

		public Node ForkLeft{
			get{return forkLeft;}
			set{forkLeft = value;}
		}
		public Node ForkRight{
			get{return forkRight;}
			set{forkRight = value;}
		}

		public Node(ref Dialogue local_message){
			Main = local_message.Main;
			if (local_message.Option_ForkLeft != null &&
			    local_message.Option_ForkRight != null){
				Option_ForkLeft = local_message.Option_ForkLeft;
				Option_ForkRight = local_message.Option_ForkRight;
			}
		}

	}

	//Variables
	private int preIndex = 0;		//used for the constructTree function
	private Node root = null;      //holds first node of the tree
	private Node current = null;   //holds current place in the tree

	//Contructor Functions
	public binaryTree(ref Dialogue[] inOrder, ref Dialogue[] preOrder){
		int len = inOrder.GetLength(0);
		Console.WriteLine(len);
		root = constructTree (ref inOrder,ref preOrder, 0, len - 1);
		current = root;
	}

	//Accessor Functions

	/*	getIsFork will return true if the current node has 2 children, and false if it has 0 or 1.
	 * 	This is used to determine whether option boxes need to be rendered or not. 
	 */ 
	public bool getIsFork(){
		if (current.Option_ForkLeft == null &&
			current.Option_ForkRight == null) {
			return false;
		}
		return true;
	}

	/*	getMessage, getOptionMessageLeft, and getOptionMessageRight are the main interface between the
	 * 	npc driver class and this class. These will return the message stored at the given variable. 
	 */
	public string getMessage(){
		return current.Main;
	}
	public string getOptionMessageLeft(){
		return current.Option_ForkLeft;
	}
	public string getOptionMessageRight(){
		return current.Option_ForkRight;
	}

	// search is a utility function used by the constructTree function.
	private int search(ref Dialogue[] array, int start, int end, string value){
		int i;
		for (i = start; i <= end; ++i) {
			if(array[i].Main == value)
				return i;
		}
		return -1;
	}

	//Mutator Functions

	/* The constructTree function requires 2 different orderings in order to construct the 
	 * binary tree. inOrder and preOrder are the two used in this function. These are
	 * arrays of the type Message, which contains three strings. The option strings 
	 * will be null if the node does not have both a left and right option. This function 
	 * uses recursive calls to contruct a tree on the left and right side of a node.
	 */ 
	public Node constructTree(ref Dialogue[] inOrder, ref Dialogue[] preOrder, int start, int end){
		if(start > end) return null; //if we return here, we are returning to a leaf node

		var node = new Node(ref preOrder[preIndex++]); //create the node and increment preIndex for the next call

		if (start == end) return node; //if we return here we are returning a leaf node

		//if the node is not a leaf, find what should be on the left and right side of it
		int inIndex = search(ref inOrder, start, end, node.Main); 

		//if we get here, the node is a parent node, so construct the left and right nodes
		node.ForkLeft = constructTree (ref inOrder,ref preOrder, start, inIndex - 1);
		node.ForkRight = constructTree (ref inOrder, ref preOrder, inIndex + 1, end);
		return node;
	}

	/* These will traverse the tree for the npc driver class. Current can store the last thing an npc 
	 * said to you, so that you can come and talk to them again if you need reminding of what they said.  
	 */
	public bool moveLeft(){
		if (current.ForkLeft != null) {
			current = current.ForkLeft;
			return true;
		}
		return false;
	}
	public bool moveRight(){
		if (current.ForkRight != null) {
			current = current.ForkRight;
			return true;
		}
		return false;
	}
	//used to move without a given left or right
	public bool moveNext(){
		if(getIsFork()){
			return false;
		}else if(moveLeft()){
			return true;
		}else if(moveRight()){
			return true;
		}
		else{
			return false;
		}
	}
}