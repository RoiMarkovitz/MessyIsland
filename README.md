# MessyIsland

The game was created as my idea for a project in the course "Game Development" at the academy.

### I was asked to program a shooting game with the following requirements:
- Two teams in an outdoor area that has obstacles and hiding places.
- Each team has two fighters, in one team one of the fighters is the human player (first person). 
- At first the fighters have no weapons (pistol and grenades). 
- The weapons must be found in the game area and are hidden in random places.
- The players search for a weapon and as soon as they are near, it passes to theirs hands. The human player has to click on the weapon to pick it up.
- Bullets and grenades are unlimited.
- One of the players (the commander) determines the purpose of the movement while the other player follows him.
- In the team of the human player the NPC follows the human player.
- Each NPC decides on its own when to fire or throw a grenade.
- When one of the players is injured or killed and also when the team wins or loses an appropriate message will appear.

### Added features beside the above
- Main screen includes, among other things, the option to choose a name for the human player and the number of rounds per game.
- Multiple rounds per game.
- Managing gunfire by creating bullets that move in accordance with the laws of physics.
- Rich GUI that includes, among other things, an indication of which weapons the human player is holding and tracking the number of rounds and the result.
- A life meter above each player and turning the human's player screen red when injured.
- Audio is based on the player's distance from the audio source. The farther the player is, the weaker he hears (or does not hear at all) and vice versa.
- Managing a queue of kill messages on the screen, so that if there is more than one message in the queue, the next message will appear below the last message in the queue - otherwise it will appear first.

### NPC behaviour
1. Leader calculates the minimum distance to the nearest weapon and walks towards it while his follower follows him.
2. When the leader and his follower have all the weapons they go (led by the leader) to the center of the world where there is an invisible game object that grills locations in the world. The leader follows the locations until he sees an enemy.
3. In any case, as soon as the leader / follower sees an enemy (and they have some weapon) they fire at the enemy while walking towards him. If there is a gun the shooting is done when the distance is greater than a certain distance or smaller than another certain distance set in the system. If there is a grenade then it will be thrown at a distance set between the two distances set for the gunshot. 
4. If the enemy has disappeared from the sight of the leader, then the leader walks to a random location on the map or goes looking for weapons if his group lacks any.
5. If the enemy has disappeared from the sight of the follower, he continues to follow the leader.
6. In the event of a shooting in the direction of the leader / follower and the enemy is not in their field of vision, they will turn around in order to locate the source of the shooting.
7. If the leader dies and the follower is still alive, then the follower becomes the leader.

[Click to view my accompanying document for the project](/Accompanying%20document%20for%20the%20project.pdf)


### Some images from the game


Main Menu         
:-------------------------:
<img src="https://github.com/roi-c/MessyIsland/blob/main/game_images/MainMenu.png" alt="drawing" width="800"/>  

Game Options      
:-------------------------:
<img src="https://github.com/roi-c/MessyIsland/blob/main/game_images/Options.png" alt="drawing" width="800"/>  

Fight   
:-------------------------:
<img src="https://github.com/roi-c/MessyIsland/blob/main/game_images/Fight1.png" alt="drawing" width="800"/> 

Another Fight   
:-------------------------:
<img src="https://github.com/roi-c/MessyIsland/blob/main/game_images/Fight2.png" alt="drawing" width="800"/> 

Player Death View
:-------------------------:
<img src="https://github.com/roi-c/MessyIsland/blob/main/game_images/PlayerDeath.png" alt="drawing" width="800"/>  

World View  
:-------------------------:
<img src="https://github.com/roi-c/MessyIsland/blob/main/game_images/WorldView2.png" alt="drawing" width="800"/>  

Game End
:-------------------------:
<img src="https://github.com/roi-c/MessyIsland/blob/main/game_images/EndGame.png" alt="drawing" width="800"/> 

