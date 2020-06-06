using System;
using UnityEngine;

public class Hacker : MonoBehaviour {
    private string userName;
    private GameStage gameStage;
    private GameLevel levelSelected;
    private string answer;
    private System.Random random;
    private string[] easyPassword = {"apple", "pear", "grape"};
    private string[] mediumPassword = {"buildings", "houses", "apartments"};
    private string[] hardPassword = {"applesOnBuildings", "pearsOnHouses", "grapesInApartments"};

    // Start is called before the first frame update
    void Start() {
        gameStage = GameStage.NAME_SELECTION;
        random = new System.Random();
        this.UserName();
    }

    void UserName() {
        Terminal.WriteLine("Type your codename: ");
    }

    void ShowMainMenu() {
        Terminal.WriteLine("Greetings, " + userName);
        Terminal.WriteLine("What would you like to hack into?\n");
        
        Terminal.WriteLine("Press <1> for the local library");
        Terminal.WriteLine("Press <2> for the police station");
        Terminal.WriteLine("Press <3> for the NASA\n");

        Terminal.WriteLine("Enter your selection:");
    }
  
    void OnUserInput(string input) {
        Terminal.ClearScreen();
        switch (this.gameStage) {
            case GameStage.NAME_SELECTION: 
                NameHandler(input);
                break;
            case GameStage.MAIN_MENU:
                MenuHandler(input);
                break;
            case GameStage.PLAYING:
                PlayingHandler(input);
                break;
            case GameStage.END_GAME:
                EndGameHandler(input);
                break;
            default:
                UnknownErrorHandler();
                break;
        }
    }

    private void NameHandler(string nameInput) {
        userName = nameInput;
        this.gameStage = GameStage.MAIN_MENU;
        ShowMainMenu();
    }

    private void MenuHandler(string levelInput) {

        int numericLevel;
        bool isNumeric = int.TryParse(levelInput, out numericLevel);
        if (isNumeric && Enum.IsDefined(typeof(GameLevel), numericLevel)) {
            levelSelected = (GameLevel) numericLevel;
            Terminal.WriteLine("User selected: " + levelSelected.ToString());
            gameStage = GameStage.PLAYING;
            
            SelectAnswer();
            AskForAnswer();
        } else {
            Terminal.WriteLine("Wrong command received.");
            ShowMainMenu();
        }
    }

    private void AskForAnswer() {
        Terminal.WriteLine("Please, type the password:");
        Terminal.WriteLine("HINT: " + answer.Anagram());
    }

    private void UnknownErrorHandler() {
        Terminal.WriteLine("Invalid command. Restarting machine.");
        this.gameStage = GameStage.NAME_SELECTION;
        UserName();
    }
    
    private void SelectAnswer() {
        switch(this.levelSelected) {
            case GameLevel.LOCAL_LIBRARY:
                this.answer = this.easyPassword[random.Next(0,easyPassword.Length)];
                break;
            case GameLevel.POLICE_STATION:
                this.answer = this.mediumPassword[random.Next(0,mediumPassword.Length)];
                break;
            case GameLevel.NASA:
                this.answer = this.hardPassword[random.Next(0,hardPassword.Length)];
                break;
            default:
                UnknownErrorHandler();
                break;
        }

        Debug.Log("Password selected: " + this.answer);
    }

    private void PlayingHandler(string userAnswer) {
        if (userAnswer.ToLower() == answer.ToLower()) {
            this.gameStage = GameStage.END_GAME;
            Terminal.WriteLine("Congratulations.");
            Terminal.WriteLine("You cracked the system!");
            ShowReward();
            Terminal.WriteLine("Type \'exit\' to leave or anything else to restart:");
        } else {
            Terminal.WriteLine("You're wrong.");
            AskForAnswer();
        }
    }

    private void EndGameHandler(String input) {
        Terminal.WriteLine("We received your command: " + input);
        if (input.ToLower() == "exit") {
            Terminal.WriteLine("Thank you for playing with us.");
            Terminal.WriteLine("See you next time.");
            gameStage = GameStage.NAME_SELECTION;
            UserName();
        } else {
            Terminal.WriteLine("Process is restarting!");
            gameStage = GameStage.MAIN_MENU;
            ShowMainMenu();
        }
    }

    private void ShowReward() {
        switch(levelSelected) {
            case GameLevel.LOCAL_LIBRARY:
                Terminal.WriteLine("You won a book...");
                DrawBook();
                break;
            case GameLevel.POLICE_STATION:
                Terminal.WriteLine("You won a gun...");
                DrawGun();
                break;
            case GameLevel.NASA:
                Terminal.WriteLine("You won a rocket");
                DrawSpaceShip();
                break;
            default:
                UnknownErrorHandler();
                break;
        }
    }

    private void DrawBook() {
        Terminal.WriteLine(@"
      _________ 
    /         / )
   /         / /
  /         / /
 /________ / /
(_________( /
");
    }

    private void DrawGun() {
        Terminal.WriteLine(@"
 ____________
(       ____()
|   |_|| 
|  |
|__|
");
    }

    private void DrawSpaceShip() {
        Terminal.WriteLine(@"
    __
   (  )       
  (    )       
  | () |
  |    |
  | () |
  |    |
  |____|
((  ||  ))
");
    }
}
