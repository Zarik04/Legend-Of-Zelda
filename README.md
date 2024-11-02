# Assignment 3: Top-Down Perspective Game Development (Legend of Zelda)

## **1. Project Overview**

**Objective**  
This project is a top-down perspective game inspired by *The Legend of Zelda*, created as part of a Game Development course assignment. The primary goal is to build a functional game that showcases core skills in procedural dungeon generation, event handling, interactive object management, and game presentation within Unity. By focusing on these essential game mechanics, the project emphasizes creating a seamless and engaging experience within a procedurally generated dungeon environment.

**Core Features**  
The game includes several key features designed to bring the game world to life:
- **Dungeon Generation**: Implements a procedural generation system to create interconnected rooms and corridors. This system dynamically generates at least three unique room types, ensuring a varied layout each time the game is played.
- **Event Handling and Interactions**: A robust event system enables player interactions with different game elements, such as doors, levers, and treasure chests. This feature is enhanced with audio feedback and visual prompts to improve user experience.
- **Presentation and Documentation**: The game is well-organized within a GitHub repository, containing a README with detailed technical documentation and a video walkthrough to demonstrate core gameplay features.

This project demonstrates a foundational approach to top-down game design, balancing creative design and technical implementation. Through procedural generation and an interactive event system, the game offers a dynamic experience, allowing players to explore unique dungeon layouts and engage with various objects in the game world.


## **2. Dungeon Generation - Procedural Generation**

#### **Description**

This dungeon generation system utilizes a **Depth-First Search (DFS)** algorithm with **backtracking** and **randomized room selection** to create a dynamic, interconnected dungeon layout. Using a grid of various room types, it ensures seamless connections via integrated corridors in each room prefab, resulting in a cohesive, immersive dungeon experience.

#### **Algorithm and Techniques**

1. **Depth-First Search (DFS) with Backtracking**:
   - The `MazeGenerator` method in `DungeonGenerator` creates the dungeon’s branching structure by visiting cells and adding paths using DFS. It starts from a specified cell and expands by visiting random neighboring cells until there are no more unvisited neighbors.
   - When dead-ends are reached, the algorithm backtracks to explore other paths. This approach generates both linear and branching paths, adding complexity to the dungeon layout.

2. **Randomized Room Selection**:
   - The `GenerateDungeon` method randomly assigns one of three room types—**Empty Room**, **Enemy Room**, or **Treasure Room**—to each cell, based on probability.
   - The `RuleManager` (through the `Rule` class in `DungeonGenerator`) enforces constraints on room placement, such as minimum counts for each type and placement restrictions for certain rooms (e.g., Treasure Rooms appear farther from the starting room). Room type probabilities can vary based on location in the grid.

3. **Grid Constraints and Boundary Management**:
   - The algorithm uses grid boundaries to prevent rooms from spawning out of bounds, and room placement respects defined boundaries and avoids overlap.
   - In `CheckNeighbors`, the script ensures that all neighboring cells are within the grid’s bounds before they are added as potential paths.

#### **Room Types and Placement Rules**

1. **Room Types**:
   - **Empty Room**: Standard rooms with minimal features.
   - **Enemy Room**: Rooms containing enemies, created using character models from the [Starter Assets - Third Person Character Controller](https://assetstore.unity.com/packages/essentials/starter-assets-thirdperson-updates-in-new-charactercontroller-pa-196526).
   - **Treasure Room**: Rooms with treasures from the [Stylized Treasure Chest by Gamertose](https://assetstore.unity.com/packages/3d/props/stylized-treasure-chest-by-gamertose-87463#content).

2. **Placement Rules**:
   - The `Rule` class defines constraints, including room distribution and placement requirements. It helps prevent consecutive Enemy Rooms and ensures Treasure Rooms are placed farther from the starting point, encouraging exploration.
   - Each room’s properties are configured in the `RuleManager` to maintain a balanced layout that enhances gameplay flow and prevents monotony.

#### **Graphical Room Connections**

1. **Corridor Generation**:
   - Instead of using separate corridors, each room prefab includes built-in corridor sections that serve as entrances/exits. `RoomBehaviour` toggles these corridors based on connections, ensuring rooms are visually connected on placement.

2. **Aligning Entrances/Exits**:
   - The `UpdateRoom` method in `RoomBehaviour` manages doors and walls based on connection status. Each door and wall’s state (open or closed) aligns perfectly with neighboring rooms, providing seamless pathways without additional pathfinding.

#### **Implementation Details**

1. **DungeonGenerator Script**:
   - The `MazeGenerator` method creates the dungeon layout, using DFS and checking neighbors with `CheckNeighbors` to add paths. Once the layout is complete, `GenerateDungeon` assigns room types.
   - `GenerateDungeon` instantiates rooms based on the placement rules defined in `RuleManager` and aligns each room with integrated corridors using `RoomBehaviour`.

2. **RoomBehaviour Script**:
   - This script handles each room’s entrances and exits using `UpdateRoom`, ensuring that the correct doors are active based on the dungeon’s layout. 
   - The room’s entrances and exits adjust dynamically according to `status`, allowing the corridors to align with neighboring rooms for a cohesive layout.

#### **Code Highlights**

Here are some key sections from the `DungeonGenerator` and `RoomBehaviour` scripts:

- **Room Selection Logic** (from `DungeonGenerator`):
    ```csharp
    int randomRoom = -1;
    List<int> availableRooms = new List<int>();

    for (int k = 0; k < rooms.Length; k++) {
        int p = rooms[k].ProbabilityOfSpawning(i, j);
        if (p == 2) {
            randomRoom = k;
            break;
        } else if (p == 1) {
            availableRooms.Add(k);
        }
    }

    if (randomRoom == -1 && availableRooms.Count > 0) {
        randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
    }
    ```

- **Room Update Method** (from `RoomBehaviour`):
    ```csharp
    public void UpdateRoom(bool[] status) {
        for (int i = 0; i < status.Length; i++) {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }
    ```

This implementation enables smooth dungeon generation with diverse room types, ensuring that dungeons feel organically interconnected while retaining structure. The setup creates an immersive dungeon experience that’s balanced for exploration, combat, and rewards.


## **3. Event Handling and Interactions**

### **Interaction System and Third-Person Player**

#### **Description**

The player character in this project is set up using the [Starter Assets - Third Person Character Controller](https://assetstore.unity.com/packages/essentials/starter-assets-thirdperson-updates-in-new-charactercontroller-pa-196526). This asset provides a flexible third-person controller, including support for Unity's new Input System, which I customized to handle player interactions. The Input System simplifies the setup of interactive events, allowing the player to trigger actions with various in-game objects, such as doors and treasure chests.

The interaction system is based on proximity triggers and uses Unity’s **Input System** to capture and respond to player actions, creating a smooth and immersive gameplay experience.

#### **Player Interactions**

In this project, I implemented two primary interactions:
1. **Player and Door Interaction**
2. **Player and Treasure Chest Interaction**

Each interaction is managed by its respective script, **DoorInteraction.cs** and **TreasureChestInteraction.cs**, which handle specific conditions, animations, and feedback when the player engages with these objects.

### **1. Player and Door Interaction**

The **DoorInteraction** script manages the interaction between the player and doors within the dungeon. This script triggers a door opening animation and provides feedback to the player based on their proximity and input.

#### **Code Overview**

Key functionalities within the `DoorInteraction` script include:
- **Proximity Detection**: The script detects when the player is within a certain distance of the door.
- **Opening the Door**: When the player interacts with the door (using a specified input key), the door opens, playing an assigned animation.
- **Feedback**: The player receives feedback through a UI prompt, guiding them to interact when near the door.

#### **Key Code Snippet**

Here is an essential part of the **DoorInteraction** script:

```csharp
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Animator doorAnimator; // Animator component to control door animation
    private bool isPlayerNear = false;

    void Update()
    {
        // Checks if player is near and presses the interaction button (e.g., "E" key)
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            doorAnimator.SetTrigger("Open"); // Triggers the door opening animation
        }
    }

    // Detects when player enters the interaction zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            // Show UI prompt for interaction
        }
    }

    // Detects when player exits the interaction zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            // Hide UI prompt for interaction
        }
    }
}
```

This snippet showcases:
- **Trigger Detection**: Using `OnTriggerEnter` and `OnTriggerExit` to detect the player’s presence near the door.
- **Animation Control**: Activating the door’s opening animation through an Animator trigger.
- **Input Check**: The player presses the “E” key to interact, with feedback provided via UI elements (e.g., a prompt to “Press E to Open Door”).

### **2. Player and Treasure Chest Interaction**

The **TreasureChestInteraction** script handles interactions with treasure chests, allowing the player to open chests and receive rewards. Similar to the door interaction, it uses proximity detection and an input-based trigger.

#### **Code Overview**

Key functionalities within the `TreasureChestInteraction` script include:
- **Proximity Detection**: Recognizes when the player is close enough to interact with the chest.
- **Opening the Chest**: Activates the treasure chest’s opening animation and triggers any associated rewards or effects.
- **Feedback**: Provides feedback, such as sound effects or UI prompts, to indicate that the chest is ready to be opened.

#### **Key Code Snippet**

Here’s a key section from the **TreasureChestInteraction** script:

```csharp
using UnityEngine;

public class TreasureChestInteraction : MonoBehaviour
{
    public Animator chestAnimator; // Animator for chest opening animation
    private bool isPlayerNear = false;

    void Update()
    {
        // Player interaction when near the chest and presses the "E" key
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            chestAnimator.SetTrigger("OpenChest"); // Triggers the chest opening animation
            // Additional code to give the player rewards, e.g., items or points
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            // Show UI prompt for interaction
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            // Hide UI prompt for interaction
        }
    }
}
```

This snippet highlights:
- **Trigger Detection**: The player’s proximity to the chest is detected through `OnTriggerEnter` and `OnTriggerExit`.
- **Reward System**: After the chest opens, additional code can be added to provide the player with in-game rewards or items.
- **UI and Sound Feedback**: Visual and auditory cues help guide the player through the interaction.

#### **Implementation Notes**

Both **DoorInteraction** and **TreasureChestInteraction** scripts rely on Unity’s Input System and Animator components, leveraging the **Starter Assets - Third Person Character Controller** to manage character movement and input responsiveness.


## **4. Project Structure and Asset Overview**

This project is organized into a series of folders designed to house all the elements necessary for the dungeon generation and player interaction system. Here’s a breakdown of the folder structure and contents, along with key assets included in the project:

1. **Prefabs**:  
   - Contains prefabs for different room types used to generate the dungeon layout. Each room prefab is pre-configured with walls, doors, and potential enemy or treasure placements, allowing for procedural generation within the game.

2. **Scenes**:  
   - **Sample Scene**: This is the main scene for the project, containing the essential elements to initialize gameplay:
     - **MainCamera** and **PlayerFollowCamera**: These are set up to follow the player and provide the player’s perspective within the dungeon.
     - **PlayerArmature**: Equipped with a third-person controller from the **StarterAssets** package, it provides controls and animations for the player character.
     - **Generator**: This object runs the dungeon generation system, creating rooms as child objects at runtime based on procedural generation.

3. **Scripts**:  
   - Contains all custom scripts responsible for various aspects of gameplay, including dungeon generation, room behaviors, event handling, and player interactions. Key scripts include:
     - **DungeonGenerator**: Handles the procedural layout and placement of rooms.
     - **RoomBehaviour**: Manages the visual setup of each room, including door and wall visibility.
     - **Interaction Scripts**: Handle player interactions with doors and treasure chests, creating dynamic gameplay.

4. **Sounds**:  
   - Houses sound effects (sfx) used in the game, specifically for enhancing player interactions with game objects like doors and treasure chests, providing a more immersive experience.

5. **StarterAssets**:  
   - Includes all essential character assets, animations, and controls needed to manage the player’s third-person perspective and interactions. This package provides 3D character models, player and enemy animations, and controls used to bring the player and enemies to life in the dungeon.

6. **StylizedHandPaintedDungeon**:  
   - This asset contains room components, such as walls, doors, floors, and decorative elements (like lamps), which are combined within prefabs to create the dungeon’s visual style. These assets allow for a cohesive, hand-painted look across all dungeon rooms.

7. **TreasureChest**:  
   - Contains a variety of treasure chest models, materials, and textures, which are used in **Treasure Rooms** to reward players. The diversity in chest models adds to the aesthetic value and provides visual rewards to players for their exploration efforts.

Each folder serves a clear purpose, ensuring that all assets and code are neatly organized and accessible, contributing to efficient project management and streamlined gameplay development. This modular structure supports easy additions, modifications, or future expansions. 


## **5. Future Enhancements**

To expand the game’s functionality and provide a more engaging experience, several potential improvements could be considered in future iterations:

1. **Enhanced Enemy AI**:  
   - Develop advanced AI for enemies, enabling them to patrol rooms, pursue the player, and exhibit more complex behaviors based on the player’s actions. Integrating state-based AI (e.g., idle, patrol, attack) can create a more dynamic and challenging environment.

2. **Combat System Implementation**:  
   - Integrate a combat system with varied attack mechanics, allowing the player to interact with enemies in real-time. This could include basic melee and ranged attacks, combo moves, and power-ups to make combat more engaging.

3. **Additional Room Variants and Puzzles**:  
   - Introduce new types of rooms and puzzle mechanics to increase gameplay diversity. These could include locked rooms that require specific keys, puzzle rooms where the player must solve riddles to proceed, and mini-boss rooms with unique enemy encounters.

4. **Improved Dungeon Randomization**:  
   - Refine the dungeon generation algorithm to provide more varied layouts each time the game is played. This could involve introducing a larger pool of room prefabs and modifying the procedural generation rules to prevent predictability and enhance replayability.

5. **Player Progression System**:  
   - Implement a progression system with elements like experience points, skill upgrades, and unlockable abilities. This would encourage players to continue exploring the dungeon, rewarding them with improved stats and capabilities over time.

6. **Loot and Inventory System**:  
   - Add an inventory system where players can collect items from treasure chests and defeated enemies. Loot could include health potions, weapons, or unique items that grant special abilities or buffs, enhancing the game's RPG elements.

7. **Multiplayer Mode**:  
   - Explore the possibility of a multiplayer or co-op mode where players can explore the dungeon together. This would require networking functionalities and synchronization of player actions but could significantly increase the game’s appeal and playtime.

8. **Enhanced Visuals and Animations**:  
   - Improve the visual appeal by adding more detailed textures, particle effects, and animations for player actions, enemy encounters, and environment interactions. This could include adding lighting effects, shadow casting, and dynamic visual elements like flickering torches or environmental hazards.

9. **Storyline and Quests**:  
   - Incorporate a storyline or quest system to provide players with a narrative structure, guiding them through the dungeon with specific goals. A storyline with non-playable characters (NPCs) could add depth and motivate players to progress.

10. **Extended Sound Design and Background Music**:  
   - Add ambient background music and soundscapes for various areas of the dungeon to heighten immersion. Dynamic sound cues based on player actions and environmental interactions (like footsteps or echoes in larger rooms) could also add to the atmosphere.

---

These enhancements would add considerable depth and replay value to the game, making it a more comprehensive and immersive experience for players. By prioritizing features based on development resources, the project can evolve iteratively while continuously engaging users.


### **6. References**

For further guidance on implementing core aspects of this project, such as dungeon generation, player mechanics, and interaction systems, the following tutorials provide valuable insights:

1. **Dungeon Generation and Room Prefabs**  
   - [Procedural Dungeon Generation in Unity](https://www.youtube.com/watch?v=gHU5RQWbmWE&t=2s)  
   - [Building Room Prefabs for Dungeon Generation](https://www.youtube.com/watch?v=gI6G49t--RY&t=3s)

2. **Third Person Controller, Player Physics, and Mechanics**  
   - [Unity Third Person Character Controller Tutorial](https://www.youtube.com/watch?v=_Er4eqhhDTo&t=176s)  
   - [Setting Up Player Physics and Mechanics in Unity](https://www.youtube.com/watch?v=keKi5j88dEo&t=222s)

3. **Player Interaction**  
   - [Unity Player Interaction System Tutorial](https://www.youtube.com/watch?v=6DyHULHqbP8&t=39s)  

These tutorials can serve as foundational resources, offering step-by-step explanations on setting up dungeons, implementing third-person controls, and configuring player interactions to enrich the gameplay experience.