# Modular Dialogue System for Unity

A modular, event-driven dialogue system built in Unity, designed to provide a clean separation between dialogue management, UI presentation, and text animation.

The system uses **ScriptableObjects**, **C# events**, **Unity's Input System**, and **UniTask** to create a flexible dialogue architecture that can be extended without tightly coupling gameplay logic to UI components.

## Features

- **Event-Driven Architecture:** Dialogue progression and UI presentation communicate through events, keeping systems independent.
- **Typewriter Effect:** Displays dialogue character by character, with configurable typing speed.
- **Instant Text Completion:** Players can instantly reveal the current sentence by pressing the dialogue skip key.
- **Input System Integration:** Uses Unity's Input System to handle player dialogue interactions.
- **ScriptableObject-Based Conversations:** Dialogue content is stored in ScriptableObjects, making conversations easy to create and configure through the Inspector.
- **Localized Dialogue:** Integrates with Unity Localization to support localized dialogue content.
- **Dialogue Events:** Supports configurable events when sentences start and finish, with optional delays for controlling dialogue flow.
- **Asynchronous Operations:** Uses Cysharp UniTask for lightweight asynchronous dialogue operations.

## Creating a Conversation

Conversations are built using **ScriptableObjects**, allowing dialogue content and gameplay events to be configured directly through the Unity Inspector without hardcoding individual conversations.

### 1. Create a Conversation

In the Unity Project window, right-click and navigate to:

`Create → Dialogue → SOConversation`

<img width="450" height="396" alt="image" src="https://github.com/user-attachments/assets/b858b282-3dff-45c1-a767-822e2fca740d" />


This creates a new conversation asset, which contains an ordered list of dialogue sentences and an optional event triggered when the conversation ends.

### 2. Create Dialogue Sentences

Right-click in the Project window and navigate to:

`Create → Dialogue → SOSentence`

<img width="400" height="303" alt="image" src="https://github.com/user-attachments/assets/0ce711ca-f3c4-4c3d-9318-0b7a47805f29" />



Each sentence contains the following properties:

| Property | Description |
|---|---|
| **Sentence Owner** | Determines whether the speaker is an NPC or the Player. |
| **NPC Name** | Defines the speaker's name. Automatically set to "Player" when the sentence owner is Player. |
| **Sentence** | A `LocalizedString` containing the dialogue text. |
| **On Sentence Start** | An optional gameplay event triggered when the sentence begins, with a configurable delay. |
| **On Sentence End** | An optional gameplay event triggered when the sentence ends, with a configurable delay. |

A custom `SOSentenceEditor` enhances the Inspector experience by displaying the NPC name field only when the sentence owner is an NPC.

### 3. Assemble the Conversation

Open your `SOConversation` asset and add your previously created `SOSentence` assets to its conversation list.

Arrange the sentences in the desired order. The DialogueManager will process them sequentially.

### 4. Configure Gameplay Events

Each sentence can trigger custom gameplay events when it starts or ends.

These events can be used for various interactions, such as:

- Playing character animations.
- Triggering sound effects.
- Activating objects or environmental interactions.
- Triggering gameplay logic.
- Synchronizing character actions with dialogue.

Both sentence start and end events support configurable delays, allowing additional control over dialogue pacing.

The conversation asset also provides an optional `OnEndConversation` event for triggering actions when the entire conversation finishes.

To create a GameEvent, in the Unity Project window, right-click and navigate to:

`Create → SOEvents → GameEvent`

<img width="400" height="370" alt="image" src="https://github.com/user-attachments/assets/3bb2b89c-b98d-4da4-b034-6df33feeddc6" />


Then attach a GameEventListener script to the game object that you want to listen to that event, and attach the action you want it to take:

<img width="450" height="305" alt="image" src="https://github.com/user-attachments/assets/3158961d-fba6-4a60-af7d-9aa19cf0da3e" />


### 5. Start the Conversation

Once configured, pass the conversation asset to the DialogueManager:

```csharp
DialogueManager.Instance.InitializeConversation(myConversation);
```

The DialogueManager initializes the conversation, triggers the initialization event, and begins processing the sentences in their configured order.

**That's it!** Your conversation is ready to be played, with localized dialogue, configurable gameplay events, and automatic dialogue progression.

## Architecture

The system is divided into three primary components, each with a distinct responsibility.

### 1. DialogueManager

The central component responsible for controlling dialogue progression.

Responsibilities:
- Initializes conversations.
- Keeps track of the current sentence.
- Handles player input for advancing dialogue.
- Triggers sentence start and end events.
- Controls conversation completion.
- Communicates with other systems through C# events.

The DialogueManager intentionally has no direct dependency on UI components. Instead, it exposes events that allow other systems to respond to dialogue-related requests.

### 2. SetUpDialogueSentence

Responsible for connecting dialogue data to the UI.

Responsibilities:
- Enables and disables the dialogue UI.
- Updates the speaker's name.
- Assigns localized dialogue content.
- Starts the typewriter animation.
- Responds to autocomplete and sentence-completion requests from the DialogueManager.

This component acts as the bridge between the dialogue system and its visual presentation.

### 3. TypewriterEffect

A standalone component responsible for animating dialogue text.

Responsibilities:
- Reveals text character by character.
- Supports configurable typing speed.
- Detects whether the current animation has finished.
- Allows the current sentence to be instantly completed.
- Uses CancellationTokenSource to safely stop previous asynchronous typing operations.

The typewriter implementation is independent of the dialogue progression logic.

## Player Interaction

The dialogue system uses a two-stage skip interaction:

| Player Input | Behaviour |
|---|---|
| First Space press | Instantly completes the current typewriter animation. |
| Second Space press | Advances to the next sentence. |

Once a sentence is completed, its end event is triggered, followed by any configured delay before the next sentence begins.

This provides a familiar dialogue experience while keeping text animation and dialogue progression separate.

## Event-Driven Communication

One of the main architectural goals of this system is to maintain loose coupling between components.

The DialogueManager exposes events that other components can subscribe to, rather than directly referencing UI elements or animation components.

For example:

- `OnInitializeConversation`: Notifies listeners that a conversation has started.
- `OnSentenceChange`: Notifies listeners when a new sentence should be displayed.
- `OnFinishConversation`: Notifies listeners that the conversation has ended.
- `OnRequestAutocompleteSentence`: Requests the current sentence to be displayed immediately.
- `OnCheckSentenceFinished`: Allows the presentation system to report whether the current sentence has finished typing.

This approach makes it possible to change or extend the presentation layer without having to rewrite the dialogue progression logic.

## Technologies Used

- **Unity:** Game engine.
- **C#:** Core programming language.
- **ScriptableObjects:** Data-driven conversation management.
- **Unity Input System:** Player input handling.
- **Unity Localization:** Localized dialogue content.
- **TextMeshPro:** Dialogue text rendering.
- **Cysharp UniTask:** Asynchronous operations and cancellation handling.

## Design Goals

This project focuses on a few important software engineering principles:

- **Separation of Concerns:** Each component has a clearly defined responsibility.
- **Loose Coupling:** Systems communicate through events rather than direct references wherever possible.
- **Modularity:** Individual components can be modified or replaced independently.
- **Data-Driven Design:** Conversations are configured through ScriptableObjects rather than hardcoded dialogue sequences.
- **Extensibility:** The architecture provides a foundation for additional dialogue features, such as branching conversations, character portraits, dialogue choices, and gameplay interactions.

## Project Status

This project is an ongoing implementation of a modular dialogue framework for Unity. Its architecture is designed to support further expansion while maintaining clear separation between dialogue logic and presentation.
