using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class UIManager : MonoBehaviour{
	//List for storing health sprite pointers
	public List<Image> imageList = new List<Image>();

	//Prefab classes and sprites
	public Image HealthSprite;
	public Sprite oilLevel_1, oilLevel_2, oilLevel_3, oilLevel_4, 
				  oilLevel_5, oilLevel_6, oilLevel_7, oilLevel_8, oilLevel_9;

	//Object References and Pointers
	Image tempImage;
	GameObject oilLevel;
	Entity currentPlayer;
	GameObject PauseMenu;

	//variables that control UI placement 
	Vector3 drawHealthUnitAt = new Vector3(-500, -250, 0);
	int offsetHealthUnitBy = 50;

	//index control variables for health list and comparison for oil level
	int size, used;
	int[] oilLevelCompare = new int[8];

	void Start(){
		//set up object references
		currentPlayer = GameObject.FindGameObjectWithTag ("Player").GetComponent<Entity>();
		oilLevel = GameObject.Find("oilLevel");
		PauseMenu = GameObject.Find ("PauseMenu");
		managePauseMenu(false);

		//setting up health sprite list
		while(currentPlayer.MaxHealth > size){
			tempImage = Instantiate(HealthSprite, drawHealthUnitAt, Quaternion.identity) as Image;
			imageList.Add(tempImage);
			drawHealthUnitAt[0] += offsetHealthUnitBy;
			++size;
		}
		used = size;

		//setting up oilLevel array for comparison 
		for(int i = 0; i < 8; ++i){
			oilLevelCompare[i] = (currentPlayer.MaxOilLevel / 8) * (i + 1);
		}
	}
	void Update(){
		manageHealthList();
		manageOilLevel();


		if(Input.GetKeyDown ("r")){
			currentPlayer.TakeDamage(1);
		} 
		if(Input.GetKeyDown ("t")){
			currentPlayer.Heal(1);
		}

		if(Input.GetKey("y")){
			currentPlayer.BurnOil(5);
		}

		if(Input.GetKeyDown("p")){
			if(Time.timeScale != 0){
				Time.timeScale = 0;
				managePauseMenu(true);
			}else{
				Time.timeScale = 1;
				managePauseMenu(false);
			}
		}

		if(!currentPlayer){
			currentPlayer = GameObject.FindGameObjectWithTag ("Player").GetComponent<Entity>();
			for(int i = 0; i < size; ++i){
				tempImage = imageList.ElementAt(i);
				tempImage.GetComponent<HealthNode>().setSpriteFull();
			}
		}
	}

	void manageHealthList(){
		if(currentPlayer.Health < used){
			tempImage = imageList.ElementAt(used - 1);
			tempImage.GetComponent<HealthNode>().setSpriteEmpty();
			--used;
		}else if(currentPlayer.Health > used){
			tempImage = imageList.ElementAt (used - 1);
			tempImage.GetComponent<HealthNode>().setSpriteFull();
			++used;
		}else if(currentPlayer.Health == used){
			tempImage = imageList.ElementAt (used - 1);
			tempImage.GetComponent<HealthNode>().setSpriteFull();
		}
	}

	void manageOilLevel(){
		if(currentPlayer.OilLevel > oilLevelCompare[7]){
			oilLevel.GetComponent<Image>().overrideSprite = oilLevel_1;
		}else if(currentPlayer.OilLevel > oilLevelCompare[6]){
			oilLevel.GetComponent<Image>().overrideSprite = oilLevel_2;
		}else if(currentPlayer.OilLevel > oilLevelCompare[5]){
			oilLevel.GetComponent<Image>().overrideSprite = oilLevel_3;
		}else if(currentPlayer.OilLevel > oilLevelCompare[4]){
			oilLevel.GetComponent<Image>().overrideSprite = oilLevel_4;
		}else if(currentPlayer.OilLevel > oilLevelCompare[3]){
			oilLevel.GetComponent<Image>().overrideSprite = oilLevel_5;
		}else if(currentPlayer.OilLevel > oilLevelCompare[2]){
			oilLevel.GetComponent<Image>().overrideSprite = oilLevel_6;
		}else if(currentPlayer.OilLevel > oilLevelCompare[1]){
			oilLevel.GetComponent<Image>().overrideSprite = oilLevel_7;
		}else if(currentPlayer.OilLevel > oilLevelCompare[0]){
			oilLevel.GetComponent<Image>().overrideSprite = oilLevel_8;
		}else{
			oilLevel.GetComponent<Image>().overrideSprite = oilLevel_9;
		}
	}

	void managePauseMenu(bool setMenu){
		PauseMenu.SetActive(setMenu);
	}










}