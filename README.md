# AFL VR Tactical Training

This software is built in collaboration with an Australian Football League team as part of Farah Farizi's (repository owner) research supporting her PhD candidature. Software is deployable in Meta Quest 1, 2, 3, and Pro devices, and will require two controllers to operate.

[Latest prototype video](https://drive.google.com/file/d/1sexfwHZHr3-1ksgPxO-reH3E9gBr9Pkk/view?usp=sharing)

## Software Overview
* An Internet connection is required
* User can join as a coach or as a player. At least one coach is required to start the application
* Users with the coach role can set up a scenario by positioning the starting and ending points of non-playable characters (NPCs) on the field. These scenarios can be saved within the system. When the "Play Scenario" button is selected, the NPCs will move from their starting points to their designated endpoints.
* Users can interact with the ball using the grip/squeeze button on the controllers. Pressing this button again will launch the ball.
* If the ball is launched while aiming at an NPC, the NPC will automatically attempt to catch it.
* Coaches can instruct NPCs to hold the ball and kick it in a specific direction or pass it to another character.


### Latest update:
* Coach users can join the experience from a desktop device. This allows them to set up and save the scenario with a mouse.
* For experiment purposes, user characters' body objects are hidden and only hand objects can be seen. The user characters' body is still within the unity project and can be activated if required.
* The board marker system is deactivated.
  

### Dependencies

* Unity 2022.3.27f
* Windows system
* Autohand assets
* Normcore
* TextMeshPro 3.0.7



## Common issues and advise
* The user body rig system is not perfect, the scaling of the user's height will often cause the character to look like they are slouching. The previous application version was tested with professional athletes that are mostly above 165cm and this eliminates the issues.

  
## Authors
* Farah Farizi - https://github.com/farahfarizi24 / farahdfarizi@gmail.com
* Ben Monahagan - https://github.com/monoganog
* Mike Wang - https://github.com/wangxinling
* Amshuuman Amshu
