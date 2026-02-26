<h1 align="center">
    Outward Basic Chat Commands
</h1>
<br/>
<div align="center">
  <img src="https://raw.githubusercontent.com/GymMed/Outward-Basic-Chat-Commands/refs/heads/main/preview/images/Logo.png" alt="Logo"/>
</div>

<div align="center">
	<a href="https://thunderstore.io/c/outward/p/GymMed/Basic_Chat_Commands/">
		<img src="https://img.shields.io/thunderstore/dt/GymMed/Basic_Chat_Commands" alt="Thunderstore Downloads">
	</a>
	<a href="https://github.com/GymMed/Outward-Basic-Chat-Commands/releases/latest">
		<img src="https://img.shields.io/thunderstore/v/GymMed/Basic_Chat_Commands" alt="Thunderstore Version">
	</a>
	<a href="https://github.com/GymMed/Outward-Mods-Communicator/releases/latest">
		<img src="https://img.shields.io/badge/Mods_Communicator-v1.2.0-D4BD00" alt="Mods Communicator Version">
	</a>
</div>

<div align="center">
	Provides basic user chat commands
</div>

## How to use it

Firstly, install [Chat Commands
Manager](https://github.com/GymMed/Outward-Chat-Commands-Manager).
After that, you can use the commands provided by this mod directly in chat.

### Built-in Commands

<details>
    <summary>Follow</summary>
Uses Unity’s <code>NavMeshPath</code> to calculate a path and simulates character
movement input to begin following another character. Following is automatically
canceled when the follower starts moving manually.<br>

**Usage:**
- Type <code>/follow</code> to follow a random nearby character (starting from
  the local player).
- Type <code>/follow characterName</code> to follow a specific character.
</details>

<details>
    <summary>Max Chat Messages</summary>
Changes the amount of messages chat panel can contain.<br>

**Usage:**
- Type <code>/maxChatMessages</code> to show chat panel messages limit.
- Type <code>/maxChatMessages --amount=90</code> to set max amount of how many messages chat panel can contain.
</details>

<details>
    <summary>Set Character Visuals</summary>
Changes how your current character looks.<br>

**Usage:**
- Type <code>/setVisuals 1 15 11 5 2</code> To change race = 1, hairStyle = 15, hairColor = 11, headVariation = 5 and gender = 2.
</details>

<details>
    <summary>Skills data</summary>
Shows skills information.<br>

**Usage:**
- Type <code>/skills</code> to show currently all available skills in the game.
- Type <code>/skills --type="1"</code> to show specific type. Available: 0 - All, 1 - Active, 2 - Passive, 3 - Cosmetic.
</details>

<details>
    <summary>🧩<strong>【 Time 】</strong></summary>

<details>
    <summary>Set Time</summary>
Sets current time.<br>

**Usage:**
- Type <code>/setTime 15:55</code> to set game time to 15 hours and 55 minutes.
</details>

<details>
    <summary>Set Minutes</summary>
Sets current hour minutes.<br>

**Usage:**
- Type <code>/setMinutes 55</code> to set game current hour minutes 55 minutes.
</details>
</details>


<details>
    <summary>🧩<strong>【 Enchantments 】</strong></summary>

<details>
    <summary>Enchantments Data</summary>
Provides information about all enchantments.<br>

**Usage:**
- Type <code>/enchantments</code> to print all enchantments in detailed way.
- Type <code>/enchantments --short="true"</code> to print all enchantments count.
</details>

<details>
    <summary>Enchantments Recipes Data</summary>
Provides information about all enchantments recipes.<br>

**Usage:**
- Type <code>/enchantmentRecipes</code> to print all enchantments recipes in detailed way.
- Type <code>/enchantmentRecipes --short="true"</code> to print all enchantments recipes count.
</details>

<details>
    <summary>Enchantments Recipes Items Data</summary>
Provides information about all enchantments recipes items.<br>

**Usage:**
- Type <code>/enchantmentRecipeItems</code> to print all enchantments recipes items in detailed way.
- Type <code>/enchantmentRecipeItems --short="true"</code> to print all enchantments recipes items count.
</details>

<details>
    <summary>Broken Enchantments Data</summary>
Provides information about all incorrectly added enchantments by modders.<br>

**Usage:**
- Type <code>/brokenEnchantments</code> to print all incorrectly added enchantments in detailed way.
- Type <code>/brokenEnchantments --short="true"</code> to print all incorrectly added enchantments count.
</details>
</details>

<details>
    <summary>🧩<strong>【 ModsCommunicator 】</strong></summary>

<details>
    <summary>Config to Xml</summary>
Saves your current configuration as an XML file for the ModsCommunicator mod.
Intended mainly for mod developers. Regular users should only lock the values
they need, as locking everything may lead to unintended gameplay changes.<br>

**Usage:**
- Type <code>/cfgToXml</code> to store configs to default location:
  <code>"BepInEx\config\gymmed.Mods_Communicator\Basic_Chat_Commands\PlayerConfigs.xml"</code>.
- Type <code>/cfgToXml --filePath="yourPath"</code> to store configs to your provided path.
</details>

</details>

## How to set up

To manually set up, do the following

1. Create the directory: `Outward\BepInEx\plugins\OutwardBasicChatCommands\`.
2. Extract the archive into any directory(recommend empty).
3. Move the contents of the plugins\ directory from the archive into the `BepInEx\plugins\OutwardBasicChatCommands\` directory you created.
4. It should look like `Outward\BepInEx\plugins\OutwardBasicChatCommands\OutwardBasicChatCommands.dll`
   Launch the game.

### If you liked the mod leave a star on [GitHub](https://github.com/GymMed/Outward-Basic-Chat-Commands) it's free
