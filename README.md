# Introduction

This game will be used to submit a technichal task for a Unity Developer position in Rookbird Games. The guidelines for the projects was that a game based on classic Spave Invaders with Match 3 games genres mechanics have to be developed. 

## Mechanics

### Player: 

- Movement: As the original game, player is able to move from left to right on the button of the screen and behind barriers. Player cant move through screen bounds

### Enemies:

- Aesthetics: Sprites assets received from Rookbird Games was used in game. There is only one type of enemy, instead of original Space Invaders that have 3. On the other hand, enemies have different colours to implement Match 3 mechanics.

- Match 3 mechanics: Enemies can be Blue, Red, Green or Yellow, if a enemy is destroyed by player. The adjacents enemies (vertically/horizontally, diagonally dont work) will be destroy at same time. Only five enemies can be destory with one shoot as maximum, the enemy in center and the four adjacents enemies from him. This combo is hard to do but the reward is very high.

- Points: If a single enemy is destroy it will give 10 score points. But, if multiple enemies are destroy by a single shoot, the score will increase following a Fibonacci rule:

> E enemies = E * FibNumber(E+1) * 10

```
1 enemy = 1 * 10 = 10 points
2 enemies = 2 * 20 = 40 points
3 enemies = 3 * 30 = 90 points
4 enemies = 4 * 50 = 200 points
5 enemies = 5 * 80 = 400 points
```

- Behavior: Enemies starts moving to right at a fixed speed, bouncing from camera bounds; when the first enemy in the certain row collide, will trigger all the enemies in that row to change their direction. Enemy rows have an offset as the original game but bigger to able the player to kill the maximum allowed chain enemies. Bounds change enemies direction and drop them all a line down when the first collides. Enemies randomly shoot to player

### Level:

- Barriers: Four barriers will appear in front of the player to protect him from enemies shoots. Barriers are sliced into 5 pieces and each one have 5 health points. Piece's health are independent and can be destroyed individually. When barriers are damaged barriers integrity changes visually

- Next Level: Level is complete when every enemy is destroyed, then enemies and barriers will respawn as same as the beggining of the match. Score and lives are maintained through levels.

- Game Over: The player die from enemies shoots, and only have 3 lives until match ends. Player can restart the match when is finished

### Easter Egg 🍳

- Main title have the original 1978 Arcade font title and background.

![Original 1978 Arcade](https://4.bp.blogspot.com/-odtGdwE8zyM/VVMNi08Gg5I/AAAAAAAAFrE/iMfnlFZYWy8/s1600/FirstVersions_SpaceInvaders_cabinet.png)

- Sound effects are the same as original game

- When player is destroyed animation is the same as original game

![Original player destroyed animation](https://1.bp.blogspot.com/-U-1UguR1Azw/VVMMGxKgncI/AAAAAAAAFqw/LfaQ-2AyxVY/s1600/FirstVersions_SpaceInvaders_table5.png)


## Development:

### Singleton
Design Pattern used in development was Singleton. A game manager was use called "Board Manager", this object will lead all the important game mechanics.

### Enemies Spawn
A matrix aliens[,] are created at the Start of the match and filled with random enemies gameobjects by Board Manager and add one to counter totalEnemies for each one instantiated. This matrix would be used to detect the position of a shooted enemy with method FindAlien(), and returns the position as a Vector2Int, then KillAdjacentAlien() validate if there is an object in determinate position and check if the adjacents enemies can be destroy if there are the same colour

### Score and Level Reset
When an enemy of same colour is detected by KillAdjacentAlion() it add one to a counter named chainEnemies. This will be used by UpdateScore() to know how many enemies are chained and with a method that returns an array with the 6 first Fibonnacci numbers, can be applied the formula given by Rookbird Guidelines and update the score.

> score += chainEnemies * Fibonacci()[chainEnemies + 1] * 10;

As PlayerPrefab is used for HighScore, it will persist even when the game is closed and oponed again. Score will override HighScore if match ends and is greater than Highscore

For each enemy destroyed totalEnemies reduce in one, when 0 is reached Board Manager will call CreateInitialBoard() and CreateBarriers(), maintaining other gameobjects as same.

### Enemies Movement

ChangeDirection() and TranslateEnemiesDown() will be trigger if aliens collide with bounds. ChangeDirection() is trigger when the first alien of a row collide, changing the velocity direction of all alien on that row. And TranslateEnemiesDown() will trigger when a enemy collide with a barrier that is able to call the method and collapse all alien as a group.

```
   public void ChangeDirection(int row)
    {
        int w = aliens.GetLength(0);

        for (int i = 0; i < w; i++)
        {
            if (aliens[i, row] != null)
            {
                aliens[i, row].GetComponent<Alien>().speed *= -1;
            }
        }
    }
```
```
    public void TranslateEnemiesDown()
    {
        foreach (GameObject alien in aliens)
        {
            if (alien != null)
            {
                alien.transform.Translate(Vector3.down);
            }
        }
    }
```
### Miscelaneous

Board Manager also can:
-Spawn Player when is destroyed
-Call GameOver when match ends

## To Improve

Due to aliens mechanics, sometimes the offset between row become huge when only remain a few enemies on match. Even when enemies are so far away between them can be destroyed if the colour match as result of they are adjacent in matrix aliens. This can be a double edge mechanic, because player can explote this and make a 5 enemies chain more easier if botton enemies get displaced. But, visually is not great to see an enemies being destroyed far away where bullets hit

Increase difficulty as less enemies left or when a level is completed, to engage player into the flow zone

## Conclusion

It was a nice and fun test, remake a classic game and give it some new mechanics was an amazing experience. I tried to replicate and take almost all the original game mechanics but had to change them a little bit to adapt them to this project. Guidelines provide by Rookbird games was challenge, but was easy to drive since I already coded a Match 3 game and knew how logic works before start coding. Thanks to Lucas and the team for this oportunity!
