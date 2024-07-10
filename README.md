# Creating a first person sword game in Unity

## What is my idea?

There are several games with sword combat, where its only based on animations, and i had a idea, no one really tried, so i came up with a flexible sword combat mechanic:

1. You can move sword freely, if you press the left mouse button
2. If you move the mouse to a specific point, the arm rotates to start an attack
3. If the mouse moves to the other direction of the specific point, which starts the attack, the player is able to swing with the sword and damage the target.

## How did i create the enviroment and the objects?

I bought them at the unity asset store, to save time and not to learn modeling, as im interested in programming and logic. Thats why i can't upload the complete project in Github, because the license of the assets are not allowed to publish openly.

## The basics

1. setup a small enviroment with a ground, where the player can stand and walk
2. create a character and a camera, which is placed in his head, so the player sees the world from the perspective of the character
3. add rotation functionality to the character, not the camera itself with clamping
4. add movement functionality to the character
5. add a simple Enemy with a collider and an damage animation
6. choose a sword from the assets and place it on the camera

## How i started my idea

I started very simple and programmed these steps:

1. if the player holds the left mouse button, the player will rotate very slowly. Not entirely, so there is a feeling of swinging while moving the sword.
2. on left mouse button click the sword is in the center of the camera, with a offset on the z axis, so the sword is not in the players face. The sword moves, if you move the mouse. Here nothing happens and the sword looks upwards, because the movement of the sword here will only decide from which direction the player wants to attack the enemy and the player has to be on a good position, to swing and attack the enemy.
3. first I only implemented checking on the y axis, and created an animation, where the sword goes down, to simulate a strike from upwards to downwards. It worked suprisingly well, the mouse reached a specific heigth, activated the animation, and the animation duration was correlated to the speed of the swinging of the player. So if the player swings his sword slowly, the animation will play slowly. If if the player doesnt move the mouse, the animation will pause until the player swings his sword. I changed this mechanic later on, to a better solution.
4. now the very basic attacking is working, so we can implement a basic enemy, which we can hit and see a reaction (animation) from him.
5. so first I choose a character, which I liked from the assets
6. I created a navmesh surface on the ground, where ais can orient themselves, see objects where they cant move, and calculate the path to the destination.
7. we have our navmesh surface, so I created the navmesh agent onto my enemy character, and I created a script, where I set the destination of the enemy as the player, so the enemy calculates which path is the best for the enemy to the player, and where he can now follow him without weird bugs.
8. we have to set a stop distance, so the enemy doesnt go into the player.
9. now we have an enemy, which can follow us! We imported a small animation from mixamo, and used it on the enemy character, and in the animator controller we added a trigger, which can be called, to trigger this specific animation.
10. now the enemy is ready to be hit, so we add a collider to the players sword, and a collider to the enemy character to check if both are colliding. I implemented a script on the player sword, which uses the OnTriggerEnter event of unity, which can "see" if a collision is happening.
11. when the sword is colliding with something, we want to check if the collision is not itself or the character
12. now our sword can detect if it touches anything
13. we create the health script, which allows a character to have both health and not be immortal
14. the health script has a take damage function, which lowers the health of the character and which can be called from other scripts, because we can use it in the sword detection script.
15. now we want to add a functionality, that can let the character die, if his health is equal or under zero
16. now if the sword collides with the enemy, it checks if the collision object has a "HealthScript" component and then we call the takedamage function. I did this because of two reasons: 1) thats a good way to filter the collision object to a destroyable object with health 2) we would get an error, if we call a function from an object, that doesnt even have a HealthScript component.
