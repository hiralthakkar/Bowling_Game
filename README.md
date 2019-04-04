# Bowling_Game
Bowling game implementation

Solution implemented in Visual Studio 2017. NuGet packages for test project are not included so these will have to be restored
before running the tests.

Here's my approach for implementation of the bowling game.

I have created 2 models/classes -

1. Roll - this class holds the number of pins knocked in each roll
2. Frame - this class holds the rolls done in a frame, along with few other properties which act are necessary for display 
and score calculation purposes

Game - This class contains 2 primary methods as required by the problem statement i.e. Roll and Score
I have added a few more internal methods to factor the code into smallest possible units. 


Unit Tests - 

I have also added a project with the unit tests. I have tried testing a couple of games with different score outcomes 
as well as a couple of tests which would result into exceptions.
