using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApplication22
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int score;

        int[,] cellValue = new int[4, 4];
        Label[,] cellContainer = new Label[4, 4];
        int rowIndex, columnIndex;

        Random randomVariable;
        bool canCheck2048;

        Stack<int> valueBuffer;
        Stack<int> scoreBuffer;
        int movesCount, undoCount;

        //Variable to assign random values to the new coordinates
        DispatcherTimer animationTimer;

        //inititalizes the components of the window
        public MainWindow()
        {
            //Initializes the components for the game window
            InitializeComponent();

            //Initializes all the data that needs to be initialized only once when the game starts
            InitializeData();

            //Calls the function inorder to initialize the nexessary data required for the game
            //Sets all the boxes to 0
            //Then Sets 2 boxes to some random value
            //Displays the first view of the game window
            NewGame();
        }

        //Initialized the data items that are required for the game
        //Only the items that have to be initialized only once are initialized here
        void InitializeData()
        {
            //Initializes the random class object
            randomVariable = new Random();

            //Sets the default font size to 25
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    cellContainer[i, j] = FindName("Label" + i + j) as Label;
                    cellContainer[i, j].FontSize = 20;
                    cellContainer[i, j].FontWeight = FontWeights.UltraBold;
                    cellContainer[i, j].HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                    cellContainer[i, j].VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                }
            }

            //Initializes the object valueBuffer and the scoreBuffer of the stack format
            valueBuffer = new System.Collections.Generic.Stack<int>();
            scoreBuffer = new System.Collections.Generic.Stack<int>();

            //Initialize the dispatcher animationTimer class
            animationTimer = new DispatcherTimer();
            animationTimer.Interval = TimeSpan.FromMilliseconds(5);
            animationTimer.Tick += AnimationTimer_Tick;
        }

        //Initialization of the global variables which need to be refreshed for each game
        //Sets all the boxes to 0
        //Then Sets 2 boxes to some random value
        //Displays the first view of the game window
        public void NewGame()
        {
            //Initialize the score to 0
            score = 0;

            //Initialize the movesCount and no_of_undo data memebers to zero and updates the valueBuffer labelObject content
            movesCount = 0;
            undoCount = 0;

            //When Continuer is initialized to zero the Contorl will Check for the win condition
            //If it is one it will not check for the win condition
            canCheck2048 = true;

            //Empty the stacks that are used for UNDO
            valueBuffer.Clear();
            scoreBuffer.Clear();

            //Allocate memory for the data labelValue and allocate memory to it
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    cellValue[i, j] = 0;
                }
            }

            //Create Two Boxes Randomly
            //Box 1 Coordinates and value
            rowIndex = randomVariable.Next() % 4;
            columnIndex = randomVariable.Next() % 4;
            cellValue[rowIndex, columnIndex] = randomVariable.Next() % 2 * 2 + 2;

            //Box 2 Coordinates and value
            rowIndex = (rowIndex + 2) % 4;
            columnIndex = (columnIndex + 2) % 4;
            cellValue[rowIndex, columnIndex] = randomVariable.Next() % 2 * 2 + 2;

            //Display Initial Data 
            UpdateLabels();

            GC.Collect();
        }

        //Saves the score and the game data for valueBuffer action
        //We can get the number of undos by dividing the no of items in the valueBuffer by 16 
        public void SaveGameData()
        {
            //Save the game data for valueBuffer
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    valueBuffer.Push(cellValue[i, j]);
                }
            }
            scoreBuffer.Push(score);
        }

        //Gets the key press from the user
        //Manipulates the data labelValue based on the key pressed by the gamer
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //If there are more moves or empty spaces for new boxes the function will continue
            //If not the function will terminate displaying the game status
            if (IsGameOver() == false)
            {
                //Saves the data for valueBuffer
                SaveGameData();

                if (GameEngine(e.Key) == true)
                {
                    //Increments the number of moves by one
                    movesCount++;

                    //Insert a new element in any of the empty boxes
                    NewInsertPosition();

                    //Display the new frame after inserting a new box  
                    UpdateLabels();
                }
                else
                {
                    //We have to delete the frames which have been redundant, so that we can use the undo button only for valid changes
                    Button_Click(UndoButton, e);
                    undoCount--;
                }
            }
        }

        //Displays the game data
        //Assigns attributes of the button
        public void UpdateLabels()
        {
            //recounts and displays the number of the undos
            UndoButton.Content = "Undo : " + valueBuffer.Count() / 16;

            //Display the score in the label
            Score.Content = "Score : " + score;

            //The two for loops are used for traversing the labelObject using the two dimensional button labelValue
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //If the value of the button becomes zero the button will show nothing
                    //Otherwise it will set the background of the button to a Brush colour based on the value in it and display the value in it
                    if (cellValue[i, j] == 0)
                    {
                        cellContainer[i, j].Background = Brushes.Transparent;
                        cellContainer[i, j].Content = null;
                    }
                    else
                    {
                        switch (cellValue[i, j])
                        {
                            case 2:
                                cellContainer[i, j].FontSize = 20;
                                cellContainer[i, j].Background = Brushes.AntiqueWhite;
                                break;
                            case 4:
                                cellContainer[i, j].FontSize = 20;
                                cellContainer[i, j].Background = Brushes.Wheat;
                                break;
                            case 8:
                                cellContainer[i, j].FontSize = 20;
                                cellContainer[i, j].Background = Brushes.Orange;
                                break;
                            case 16:
                                cellContainer[i, j].FontSize = 20;
                                cellContainer[i, j].Background = Brushes.OrangeRed;
                                break;
                            case 32:
                                cellContainer[i, j].FontSize = 20;
                                cellContainer[i, j].Background = Brushes.Red;
                                break;
                            case 64:
                                cellContainer[i, j].FontSize = 20;
                                cellContainer[i, j].Background = Brushes.MediumVioletRed;
                                break;
                            case 128:
                                cellContainer[i, j].FontSize = 20;
                                cellContainer[i, j].Background = Brushes.SteelBlue;
                                break;
                            case 256:
                                cellContainer[i, j].FontSize = 20;
                                cellContainer[i, j].Background = Brushes.Goldenrod;
                                break;
                            case 512:
                                cellContainer[i, j].FontSize = 20;
                                cellContainer[i, j].Background = Brushes.Gold;
                                break;
                            case 1024:
                                cellContainer[i, j].FontSize = 16;
                                cellContainer[i, j].Background = Brushes.Yellow;
                                break;
                            case 2048:
                                cellContainer[i, j].FontSize = 16;
                                cellContainer[i, j].Background = Brushes.Red;
                                break;
                            case 4096:
                                cellContainer[i, j].FontSize = 16;
                                cellContainer[i, j].Background = Brushes.Tomato;
                                break;
                            case 8192:
                                cellContainer[i, j].FontSize = 16;
                                cellContainer[i, j].Background = Brushes.Chocolate;
                                break;
                            case 16384:
                                cellContainer[i, j].FontSize = 13;
                                cellContainer[i, j].Background = Brushes.Blue;
                                break;
                            case 32768:
                                cellContainer[i, j].FontSize = 13;
                                cellContainer[i, j].Background = Brushes.ForestGreen;
                                break;
                            case 65536:
                                cellContainer[i, j].FontSize = 13;
                                cellContainer[i, j].Background = Brushes.Crimson;
                                break;
                            case 131072:
                                cellContainer[i, j].FontSize = 12;
                                cellContainer[i, j].Background = Brushes.DarkTurquoise;
                                break;
                        }
                        cellContainer[i, j].Content = cellValue[i, j];
                    }
                }
            }
        }

        //Checks whether the game can be further continued or not
        //returns 1 if it can be contiued else it returns 0
        public bool IsGameOver()
        {
            bool isGameOver = true;

            for (int i = 0; i < 4; i++)
            {

                //This loop checks whether an empty box is present in the labelValue of the boxes
                //It traverses each box and checks its contents and returns false if a box with 0 as its value is encountered
                for (int j = 0; j < 4; j++)
                {
                    if (cellValue[i, j] == 0)
                    {
                        isGameOver = false;
                        break;
                    }
                }

                //It checks whether two successive blocks have the same value so that they can be merged
                //checking the j and j+1 row and column independantly
                for (int j = 0; j < 3; j++)
                {
                    if (cellValue[i, j] == cellValue[i, j + 1] || cellValue[j, i] == cellValue[j + 1, i])
                    {
                        isGameOver = false;
                        break;
                    }
                }

                //If the player hasn't reached the 2048 level the compuuter will check for the 2048 and after finding it
                //it will ask the user whether to contiue or not
                //if the user replies yes the game will continue and this condition will not be checked again
                //other wise a new game must be started
                if (canCheck2048 == true)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (cellValue[i, j] == 2048)
                        {
                            if (MessageBox.Show("Congrats!!! You Won the game '2048' !!!" + "\n\nGame Statistics : " + "\n\n\tTotal Score   : " + score + "\n\tTotal Move   : " + (movesCount - undoCount) + "\n\tTotal Undo   : " + undoCount + "\n\n\t\t\tDo you want to start a new game?", "Game Statistics", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            {
                                isGameOver = false;
                                canCheck2048 = false;
                            }
                            break;
                        }
                    }
                }
            }

            if (isGameOver == true)
            {
                //Condition and Details of the Loose message box
                if (MessageBox.Show("Sorry!!! You loose!! please try again!!!" + "\n\nGame Statistics : " + "\n\n\tTotal Score   : " + score + "\n\tTotal Move   : " + (movesCount - undoCount) + "\n\tTotal Undo   : " + undoCount + "\n\n\t\t\tDo you want to start a new game?", "Game Statistics", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    NewGame();
                }
            }

            //returns the status of the game
            return isGameOver;
        }

        //Searches and Inserts a new block of value in the Button Array
        //It uses the built in function next of the object randomVariable defined in the random() class
        public void NewInsertPosition()
        {
            //Reset the height and width to their original values if they are not set by the dispatcher animationTimer
            cellContainer[rowIndex, columnIndex].Width = 60;
            cellContainer[rowIndex, columnIndex].Height = 60;

            ////This code will be slow when the 4*4 labelValue is almost full but gives an almost random new position every time
            ////Because this will sometimes keep on going to the used slot, so the latency will be higher
            //do
            //{
            //    rowIndex = randomVariable.Next() % 4;
            //    columnIndex = randomVariable.Next() % 4;
            //} while (labelValue[rowIndex, columnIndex] != 0);

            //This will also generate the required row and column index but it will be faster and more accurate 
            //Requires only 2 pass one for getting unused cell and the other for assiging value to a random unused cell
            //But will use a little more memory than the previous algorithm
            int[] possibleSlots = new int[16];
            int slots = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (cellValue[i, j] == 0)
                    {
                        possibleSlots[slots++] = i * 4 + j;
                    }
                }
            }
            slots = possibleSlots[randomVariable.Next() % slots];
            rowIndex = slots / 4;
            columnIndex = slots % 4;

            //Animate the new Position
            cellValue[rowIndex, columnIndex] = randomVariable.Next(0, 10000) % 2 * 2 + 2;
            cellContainer[rowIndex, columnIndex].Height = 0;
            cellContainer[rowIndex, columnIndex].Width = 0;
            animationTimer.Start();
        }

        void AnimationTimer_Tick(object sender, EventArgs e)
        {
            //Increment the button width by 5 if the condition is met else stop the animationTimer
            if (cellContainer[rowIndex, columnIndex].Width != 60)
            {
                cellContainer[rowIndex, columnIndex].Width += 5;
                cellContainer[rowIndex, columnIndex].Height += 5;
            }
            else
            {
                animationTimer.Stop();
            }
        }

        // merges the same valued adjacent blocks together
        // Removes the empty space in the back direction like from 2 4 0 0 to 0 0 2 4
        // It checks the ith and the i-1th element
        // Calculates Scores Based on the Merges
        // Removes the empty space in the back direction like from 2 4 0 0 to 0 0 2 4
        public bool MakePartialMove(int[] checkArray)
        {
            int temp = 0;
            bool isMoved = false;

            //Removes the spaces i.e, 0
            //it convertes something like this 2 0 1 0 to 0 0 2 1
            for (int i = 3, j = 3; i >= 0; i--)
            {
                if (checkArray[i] != 0)
                {
                    temp = checkArray[i];
                    checkArray[i] = 0;
                    checkArray[j--] = temp;

                    isMoved = true;
                }
            }

            for (int i = 3; i > 0; i--)
            {
                if (checkArray[i] == checkArray[i - 1])
                {
                    checkArray[i] += checkArray[i - 1];
                    score += checkArray[i];
                    checkArray[i - 1] = 0;

                    isMoved = true;
                }
            }

            //Removes the spaces i.e, 0
            //it convertes something like this 2 0 1 0 to 0 0 2 1
            //This is used to remove the new spaces that arise from 
            for (int i = 3, j = 3; i >= 0; i--)
            {
                if (checkArray[i] != 0)
                {
                    temp = checkArray[i];
                    checkArray[i] = 0;
                    checkArray[j--] = temp;
                }
            }

            return isMoved;
        }

        //Based on the preview key event handler function argument e
        //The function decomposes the 4x4 arrary into 4 1*4 labelValue
        //Saves this 1*4 labelValue into a temp 1d labelValue each time
        //calls the MakePartialMove(int[] checkArray) function for each of those 1d arrays
        //stores the resultant buffer back into the corresponding source labelValue each time
        //Returns 1 when there is a change else it returns 0
        public bool GameEngine(Key keyPressed)
        {
            //Initialization of the temporary labelValue
            int[] updateArray = new int[4];

            //Initializes checkArray flag to ensure that the window refresh function is not called unnecessarily
            //It is one of the main key features that optimizes the code
            bool isSourceChanged = false;


            //Decomposes the 4*4 labelValue into 4 1*4 labelValue consisting of the individual comlumns or rows
            //Calls MakePartialMove function
            //Finalizes the change by copying the modified 1d Value back to the source
            //if there has been any change in the source the flag is assigned 1 to prevent the screen refresh
            switch (keyPressed)
            {
                case Key.Up:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 3; j >= 0; j--)
                        {
                            updateArray[3 - j] = cellValue[j, i];
                        }
                        MakePartialMove(updateArray);
                        for (int j = 3; j >= 0; j--)
                        {
                            if (cellValue[j, i] != updateArray[3 - j])
                            {
                                cellValue[j, i] = updateArray[3 - j];
                                isSourceChanged = true;
                            }
                        }
                    }
                    break;
                case Key.Down:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            updateArray[j] = cellValue[j, i];
                        }
                        MakePartialMove(updateArray);
                        for (int j = 0; j < 4; j++)
                        {
                            if (cellValue[j, i] != updateArray[j])
                            {
                                cellValue[j, i] = updateArray[j];
                                isSourceChanged = true;
                            }
                        }
                    }
                    break;
                case Key.Right:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            updateArray[j] = cellValue[i, j];
                        }
                        MakePartialMove(updateArray);
                        for (int j = 0; j < 4; j++)
                        {
                            if (cellValue[i, j] != updateArray[j])
                            {
                                cellValue[i, j] = updateArray[j];
                                isSourceChanged = true;
                            }
                        }
                    }
                    break;
                case Key.Left:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 3; j >= 0; j--)
                        {
                            updateArray[3 - j] = cellValue[i, j];
                        }
                        MakePartialMove(updateArray);
                        for (int j = 3; j >= 0; j--)
                        {
                            if (cellValue[i, j] != updateArray[3 - j])
                            {
                                cellValue[i, j] = updateArray[3 - j];
                                isSourceChanged = true;
                            }
                        }
                    }
                    break;
            }

            return isSourceChanged;
        }

        //This is used to handle the button click events
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                //This is used to quit the current game and start a new one
                //Resets the game by calling the game initializer function
                case "ResetButton":
                    NewGame();
                    break;

                //This event handler handles the valueBuffer button click event
                //Undos the game by popping the data collected from the valueBuffer stack
                //Undos the score by popping the data collected from the scoreBuffer stack
                case "UndoButton":
                    //Increments the no. of undos done so far in this game
                    undoCount++;

                    //Pops from the valueBuffer stack and reassigns the main labelValue
                    //Pops the score from the scoreBuffer stack
                    if (valueBuffer.Count() != 0)
                    {
                        for (int i = 3; i >= 0; i--)
                        {
                            for (int j = 3; j >= 0; j--)
                            {
                                cellValue[i, j] = valueBuffer.Pop();
                            }
                        }
                        score = scoreBuffer.Pop();
                    }

                    //counts the number of undos remaining
                    UndoButton.Content = "Undo : " + valueBuffer.Count() / 16;
                    Score.Content = "Score : " + score;

                    //Displays the resulting frame
                    UpdateLabels();
                    break;
            }
        }

        ////This envent handler upon termination of the window displays a Thank You message in a message box
        //private void Window_Closed(object sender, EventArgs e)
        //{
        //    //show the message box
        //    MessageBox.Show("Thank You!!! Visit Again!!!", "© Kappspot");
        //}
        //// Discarded code on using new and improved versions 
        ////Removes the empty space in the back direction like from 0 0 2 4 to 2 4 0 0
        //public void RemoveSpaceBackwards(int[] checkArray)
        //{
        //    for(int i=0, j=0; i<3; i++)
        //    {
        //        if (checkArray[i] == 0 || checkArray[i] == checkArray[i + 1])
        //        {
        //            checkArray[i] += checkArray[i + 1];
        //        }
        //        else if (checkArray[i] != 0)
        //        {
        //            int temp = checkArray[i];
        //            checkArray[i] = 0;
        //            checkArray[j++] = temp;
        //        }
        //    }

        //    ////j is declared here so that the swap operation may be performed after the execution of the inner for loop
        //    //int j;

        //    ////starting from the 1st element it checks for the empty box till the 3rd element
        //    ////if an empty box if found it searches from that box for a non empty box and then gets the value of that box and then assigns its value to 0
        //    //for (int i = 0; i < 3; i++)
        //    //{
        //    //    //checks for the empty block
        //    //    if (checkArray[i] == 0)
        //    //    {
        //    //        //checks for the next non-empty block
        //    //        for (j = i + 1; j < 3; j++)
        //    //        {
        //    //            //if that block is found the loop is broken
        //    //            if (checkArray[j] != 0)
        //    //            {
        //    //                break;
        //    //            }
        //    //        }

        //    //        //swaps the non-empty block and the empty block
        //    //        checkArray[i] = checkArray[j];
        //    //        checkArray[j] = 0;
        //    //    }
        //    //}
        //}

        ////Removes the empty space in the back direction like from 2 4 0 0 to 0 0 2 4
        //public void RemoveSpaceForward(int[] checkArray)
        //{
        //    for (int i = 3, j = 3; i >= 0; i--)
        //    {
        //        if (checkArray[i] != 0)
        //        {
        //            int temp = checkArray[i];
        //            checkArray[i] = 0;
        //            checkArray[j--] = temp;
        //        }
        //    }

        //    ////j is declared here so that the swap operation may be performed after the execution of the inner for loop
        //    //int j;

        //    ////starting from the 3th element it checks for the empty box till the 1st element
        //    ////if an empty box if found it searches from that box for a non empty box and then gets the value of that box and then assigns its value to 0
        //    //for (int i = 3; i > 0; i--)
        //    //{
        //    //    //checks for the empty block
        //    //    if (checkArray[i] == 0)
        //    //    {
        //    //        //checks for the next non-empty block
        //    //        for (j = i - 1; j > 0; j--)
        //    //        {
        //    //            //if that block is found the loop is broken
        //    //            if (checkArray[j] != 0)
        //    //                break;
        //    //        }

        //    //        //swaps the non-empty block and the empty block
        //    //        checkArray[i] = checkArray[j];
        //    //        checkArray[j] = 0;
        //    //    }
        //    //}
        //}        

        ////starting from the last element to the first element it searches for a compatible pair
        ////Removes the empty space in the back direction like from 0 0 2 4 to 2 4 0 0
        ////Adds the two adjacent values to the previous block
        ////Assigns 0 to the next block and the removes the redundant spaces
        ////Removes the empty space in the back direction like from 0 0 2 4 to 2 4 0 0
        //public void MergeBackwards(int[] checkArray)
        //{
        //    for (int i = 0, j = 0; i < 4; i++)
        //    {
        //        if (checkArray[i] != 0)
        //        {
        //            int temp = checkArray[i];
        //            checkArray[i] = 0;
        //            checkArray[j++] = temp;
        //        }
        //    }

        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (checkArray[i] == checkArray[i + 1])
        //        {
        //            checkArray[i] += checkArray[i + 1];
        //            score += checkArray[i];
        //            checkArray[i + 1] = 0;
        //        }
        //    }

        //    for (int i = 0, j = 0; i < 4; i++)
        //    {
        //        if (checkArray[i] != 0)
        //        {
        //            int temp = checkArray[i];
        //            checkArray[i] = 0;
        //            checkArray[j++] = temp;
        //        }
        //    }

        //    ////Removes the sapce in the backward direction
        //    //RemoveSpaceBackwards(checkArray);

        //    ////starting from the first element to the last element it searches for a compatible pair
        //    ////Swaps the two adjacent values and the removes the redundant spaces
        //    //for (int i = 0; i < 3; i++)
        //    //{
        //    //    if (checkArray[i] == checkArray[i + 1])
        //    //    {
        //    //        checkArray[i] += checkArray[i + 1];
        //    //        checkArray[i + 1] = 0;
        //    //        score += checkArray[i];
        //    //        RemoveSpaceBackwards(checkArray);
        //    //    }
        //    //}

        //    ////Removes the final redundant space after the final swap to make sure that there are no empty spaces in between the blocks after the merging
        //    //RemoveSpaceBackwards(checkArray);
        //}

        // * merges the same valued adjacent blocks together
        // * Removes the empty space in the back direction like from 2 4 0 0 to 0 0 2 4
        // * It checks the ith and the i-1th element
        // * Calculates Scores Based on the Merges
        // * Removes the empty space in the back direction like from 2 4 0 0 to 0 0 2 4
        // */
        //public void MergeForwards(int[] checkArray)
        //{
        //    int temp = 0;

        //    for (int i = 3, j = 3; i >= 0; i--)
        //    {
        //        if (checkArray[i] != 0)
        //        {
        //            temp = checkArray[i];
        //            checkArray[i] = 0;
        //            checkArray[j--] = temp;
        //        }
        //    }

        //    for (int i = 3; i > 0; i--)
        //    {
        //        if (checkArray[i] == checkArray[i - 1])
        //        {
        //            checkArray[i] += checkArray[i - 1];
        //            score += checkArray[i];
        //            checkArray[i - 1] = 0;
        //        }
        //    }

        //    for (int i = 3, j = 3; i >= 0; i--)
        //    {
        //        if (checkArray[i] != 0)
        //        {
        //            temp = checkArray[i];
        //            checkArray[i] = 0;
        //            checkArray[j--] = temp;
        //        }
        //    }

        //    ////Removes the sapce in the forward direction
        //    //RemoveSpaceForward(checkArray);

        //    ////starting from the last element to the first element it searches for a compatible pair
        //    ////Adds the two adjacent values to the next block
        //    ////Assigns 0 to the previous block and the removes the redundant spaces
        //    //for (int i = 3; i > 0; i--)
        //    //{
        //    //    if (checkArray[i] == checkArray[i - 1])
        //    //    {
        //    //        checkArray[i] += checkArray[i - 1];
        //    //        checkArray[i - 1] = 0;
        //    //        score += checkArray[i];
        //    //        RemoveSpaceForward(checkArray);
        //    //    }
        //    //}

        //    ////Removes the final redundant space after the final swap to make sure that there are no empty spaces in between the blocks after the merging
        //    //RemoveSpaceForward(checkArray);
        //}
        //Based on the preview key event handler function argument e
        //The function decomposes the 4x4 arrary into 4 1*4 labelValue
        //Saves this 1*4 labelValue into a temp 1d labelValue each time
        //calls the merge_backward() or merge_forward() appropriately each time
        //stores the resultant buffer back into the corresponding source labelValue each time
        //Returns 1 when there is a change else it returns 0
        //public bool GameEngine(Key keyPressed)
        //{
        //    //Initialization of the temporary labelValue
        //    int[] updateArray = new int[4];

        //    //Initializes checkArray flag to ensure that the window refresh function is not called unnecessarily
        //    //It is one of the main key features that optimizes the code
        //    bool flag = false;

        //    //Decomposes the 4*4 labelValue into 4 1*4 labelValue consisting of the individual comlumns
        //    //Calls merge backwards function
        //    //Finalizes the change by copying the modified 1d labelValue back to the source
        //    //if there has been any change in the source the flag is assigned 1 to prevent the screen refresh
        //    if (keyPressed == Key.Up)
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            for (int j = 3; j >= 0; j--)
        //            {
        //                updateArray[3 - j] = labelValue[j, i];
        //            }
        //            MakePartialMove(updateArray);
        //            for (int j = 3; j >= 0; j--)
        //            {
        //                if (labelValue[j, i] != updateArray[3 - j])
        //                {
        //                    labelValue[j, i] = updateArray[3 - j];
        //                    flag = true;
        //                }
        //            }
        //        }
        //    }

        //    //Decomposes the 4*4 labelValue into 4 1*4 labelValue consisting of the individual comlumns
        //    //Calls merge forward function
        //    //Finalizes the change by copying the modified 1d labelValue back to the source
        //    //if there has been any change in the source the flag is assigned 1 to prevent the screen refresh
        //    if (keyPressed == Key.Down)
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            for (int j = 0; j < 4; j++)
        //            {
        //                updateArray[j] = labelValue[j, i];
        //            }
        //            MakePartialMove(updateArray);
        //            for (int j = 0; j < 4; j++)
        //            {
        //                if (labelValue[j, i] != updateArray[j])
        //                {
        //                    labelValue[j, i] = updateArray[j];
        //                    flag = true;
        //                }
        //            }
        //        }
        //    }

        //    //Decomposes the 4*4 labelValue into 4 1*4 labelValue consisting of the individual rows
        //    //Calls merge backwards function
        //    //Finalizes the change by copying the modified 1d labelValue back to the source
        //    //if there has been any change in the source the flag is assigned 1 to prevent the screen refresh
        //    if (keyPressed == Key.Left)
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            for (int j = 3; j >= 0; j--)
        //            {
        //                updateArray[3 - j] = labelValue[i, j];
        //            }
        //            MakePartialMove(updateArray);
        //            for (int j = 3; j >= 0; j--)
        //            {
        //                if (labelValue[i, j] != updateArray[3 - j])
        //                {
        //                    labelValue[i, j] = updateArray[3 - j];
        //                    flag = true;
        //                }
        //            }
        //        }
        //    }

        //    //Decomposes the 4*4 labelValue into 4 1*4 labelValue consisting of the individual rows
        //    //Calls merge forwards function
        //    //Finalizes the change by copying the modified 1d labelValue back to the source
        //    //if there has been any change in the source the flag is assigned 1 to prevent the screen refresh
        //    if (keyPressed == Key.Right)
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            for (int j = 0; j < 4; j++)
        //            {
        //                updateArray[j] = labelValue[i, j];
        //            }
        //            MakePartialMove(updateArray);
        //            for (int j = 0; j < 4; j++)
        //            {
        //                if (labelValue[i, j] != updateArray[j])
        //                {
        //                    labelValue[i, j] = updateArray[j];
        //                    flag = true;
        //                }
        //            }
        //        }
        //    }

        //    //returns the flag
        //    //returns true if there is any change and if the screen needs to be updated
        //    //returns false if the screen doesn't need any refreshing as there is no change in the value of the blocks or when a wrong key has been entered
        //    return flag;
        //}
    }
}