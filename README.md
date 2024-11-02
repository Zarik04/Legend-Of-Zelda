# Assignment 3: Top-Down Perspective Game Development (Legend of Zelda)

## 1. Project Overview

**Objective**  
This project is a top-down perspective game inspired by *The Legend of Zelda*, created as part of a Game Development course assignment. The primary goal is to build a functional game that showcases core skills in procedural dungeon generation, event handling, interactive object management, and game presentation within Unity. By focusing on these essential game mechanics, the project emphasizes creating a seamless and engaging experience within a procedurally generated dungeon environment.

**Core Features**  
The game includes several key features designed to bring the game world to life:
- **Dungeon Generation**: Implements a procedural generation system to create interconnected rooms and corridors. This system dynamically generates at least three unique room types, ensuring a varied layout each time the game is played.
- **Event Handling and Interactions**: A robust event system enables player interactions with different game elements, such as doors, levers, and treasure chests. This feature is enhanced with audio feedback and visual prompts to improve user experience.
- **Presentation and Documentation**: The game is well-organized within a GitHub repository, containing a README with detailed technical documentation and a video walkthrough to demonstrate core gameplay features.

This project demonstrates a foundational approach to top-down game design, balancing creative design and technical implementation. Through procedural generation and an interactive event system, the game offers a dynamic experience, allowing players to explore unique dungeon layouts and engage with various objects in the game world.


## 2. Dungeon Generation

### **Procedural Generation**

#### **Description**

This project’s dungeon generation system utilizes a **Depth-First Search (DFS)** algorithm with **backtracking** and **randomized room selection** to create an interconnected dungeon layout. Using pre-designed assets, rooms are placed on a grid and connected through aligned corridors, creating a visually appealing and diverse dungeon environment.

1. **Algorithm and Techniques**:
   - **Depth-First Search (DFS) with Backtracking**:
     - The dungeon generation begins from a starting room, expanding the layout by visiting random neighboring cells, creating a branching network of rooms. Backtracking enhances layout variety, resulting in both linear and non-linear paths.
   - **Randomized Room Selection**:
     - Each room cell is randomly assigned one of three room types—**Empty Room** (default), **Enemy Room**, or **Treasure Room**—based on predefined probabilities. The **RuleManager** script enforces constraints on room distribution, ensuring a balanced experience and strategic placement of specific room types.
   - **Grid Constraints and Boundary Management**:
     - Boundary checks prevent out-of-bounds generation, while room placements are validated to avoid overlap. Additional rules, like separating consecutive Enemy Rooms, are managed by the RuleManager.

2. **Room Types and Placement Rules**:
   - **Room Types**:
     - **Empty Room (Default)**: Basic rooms created from the [Stylized Hand Painted Dungeon Free](https://assetstore.unity.com/packages/3d/environments/stylized-hand-painted-dungeon-free-173934) asset, providing transitions between rooms.
     - **Enemy Room**: Includes enemy models based on characters from the [Starter Assets - Third Person Character Controller](https://assetstore.unity.com/packages/essentials/starter-assets-thirdperson-updates-in-new-charactercontroller-pa-196526) to add combat challenges.
     - **Treasure Room**: Features rewards such as a [Stylized Treasure Chest](https://assetstore.unity.com/packages/3d/props/stylized-treasure-chest-by-gamertose-87463#content) by Gamertose, incentivizing exploration.
   - **Placement Rules**:
     - Room type constraints are enforced through the RuleManager, ensuring balanced types and preventing Enemy Room clustering. Treasure Rooms are placed further from the starting room to reward exploration.

3. **Graphical Room Connections**:
   - **Corridor Generation**:
     - Each room prefab includes integrated corridor sections from the [Stylized Hand Painted Dungeon Free](https://assetstore.unity.com/packages/3d/environments/stylized-hand-painted-dungeon-free-173934) asset, which align with neighboring rooms for seamless connectivity.
   - **Aligning Entrances/Exits**:
     - The prefab corridors align perfectly with adjacent rooms, forming natural pathways without requiring extra pathfinding logic.

#### **Implementation**

1. **DungeonGenerator Script**:
   - This script manages the core dungeon generation, using the DFS algorithm to create and connect rooms with integrated corridor sections for continuity.

2. **RoomBehavior Script**:
   - Handles room-specific interactions, like spawning enemies in Enemy Rooms or activating treasure chests in Treasure Rooms, providing dynamic experiences for players.

3. **RuleManager Script**:
   - Enforces constraints on room distribution, ensuring a balanced gameplay experience through structured room placement.

#### **Generation Process Overview**

1. **DFS Algorithm and Expansion**:
   - Starting from an initial grid position, the DungeonGenerator uses DFS to create a fully connected dungeon, backtracking when necessary.

2. **Room Assignment and Constraints**:
   - Room types are assigned per RuleManager guidelines, with Enemy and Treasure Rooms placed strategically.

3. **Graphical Corridor Connection**:
   - Integrated corridor sections ensure seamless visual and navigational connectivity between rooms.

This procedural generation system, combined with high-quality assets, produces a cohesive, balanced dungeon structure that feels both organic and rewarding for exploration.


## 3. Event Handling and Interactions

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
