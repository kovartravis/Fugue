using UnityEngine;
using System.Collections;

public class DialogueDriver : MonoBehaviour{

	Dialogue[] preOrder = new Dialogue[]{
						  new Dialogue("Hello, what is your favorite pet?","Dogs", "Cats"),
						  new Dialogue("Wrong answer, try again", "Cats", "Dogs"),
						  new Dialogue("Good job!"),
				       	  new Dialogue("You got it wrong again,\n let me help you", "Cats", "Cats"),
						  new Dialogue("Finally!"),
						  new Dialogue("Took you long enough!"),
					   	  new Dialogue("Congrats, you got it right on your first try"),
		                  new Dialogue("Meow"),
		                  new Dialogue("Be sure to try the other options!")
						  };
	Dialogue[] inOrder = new Dialogue[]{
						 new Dialogue("Good job!"),
						 new Dialogue("Wrong answer, try again", "Cats", "Dogs"),
		             	 new Dialogue("Finally!"),
		          	     new Dialogue("You got it wrong again,\n let me help you", "Cats", "Cats"),
						 new Dialogue("Took you long enough!"),
						 new Dialogue("Hello, what is your favorite pet?","Dogs", "Cats"),
						 new Dialogue("Congrats, you got it right on your first try"),
						 new Dialogue("Be sure to try the other options!"),
						 new Dialogue("Meow")
						};

	binaryTree current, root; //this holds the binary tree for dialogue 
	bool playerInRange, engaged; //these hold data about the state of the game
	string messageInRange = "Hey, press [enter] to talk to me, \n" +
							"and [f] and [g] to make choices!"; //message to prompt player to enter dialogue
	int wait = 0;

	void Awake () {
		current = new binaryTree(ref inOrder,ref preOrder);
	}
	
	void Update(){
		if(wait > 0){
			wait--;
		}
	}
	/*Out of Date!
	void OnGUI() {
		if(playerInRange && !engaged){
			GUI.Box(new Rect(Screen.width /2-50, Screen.height /2-200, 250f, 40f), messageInRange);
			if(Input.GetKeyDown("enter") && wait == 0){
				engaged = true;
				wait = 25;
			}
		}
		if(playerInRange && engaged){
			if(!current.getIsFork()){
				GUI.Box(new Rect(Screen.width /2-50, Screen.height /2+50, 250f, 40f), current.getMessage());
				if(Input.GetKeyDown("enter") && wait == 0){
					current.moveNext ();
					wait = 25;
				}
			}else{
				GUI.Box(new Rect(Screen.width /2-50, Screen.height /2+50, 200f, 40f), current.getMessage());
				GUI.Box(new Rect(Screen.width /2 - 50, Screen.height /2 + 90, 100f, 25f), current.getOptionMessageLeft());
				GUI.Box(new Rect(Screen.width /2 + 50, Screen.height /2 + 90, 100f, 25f), current.getOptionMessageRight());
				if(Input.GetKeyDown("f") && wait == 0){
					current.moveLeft();
					wait = 25;
				}
				if(Input.GetKeyDown("g") && wait == 0){
					current.moveRight();
					wait = 25;
				}
			}
		}
	}
	*/

	//may be out of date
	void OnTriggerEnter2D(){
		playerInRange = true;
	}
	void OnTriggerExit2D(){
		playerInRange = false;
		engaged = false;
	}
}
