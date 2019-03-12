# n-Puzzle solver
Artificial Intelligence, n-Puzzle problem solver, approach: bidirectional BFS

![Example of an n-Puzzle (image from the book 'Artificial Intelligence: A Modern Approach')](n-Puzzle.jpg)

**Figure 1** - Example of an n-Puzzle, image from the book 'Artificial Intelligence: A Modern Approach'

## Requirements:
- [.NET Core runtime](https://dotnet.microsoft.com/download)

## Instructions:
1. Clone the repository
2. Run Terminal/Console in the root directory of the project
3. Type in 'dotnet run'
4. It's up and running (when using text file input - see the example_input.txt file for the structure) - Console UI should be self explanatory

## Input file structure:
- First line contains max iterations count - in case the problem has no solution (proposed value is 1000).
- Second line contains n-Puzzle width.
- Third line contains n-Puzzle height.
- The other lines are the initial and final state of the puzzle.

## Note:
Empty place in the puzzle is represented by number **0**. All numbers at positions in the puzzle have to be distinct (there cannot be more positions than 1 with the same number).
