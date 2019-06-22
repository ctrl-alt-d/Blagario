# Blagario project

This is just an experimental lab to test to make a multiplayer web game without javascript (or almost without). Components:

* blazor ( netcore 3 preview 6 )
* html
* css

Maybe a more simple game (pong, snake, space invaders )would be enough to make the lat ... but ... let's try.

### How it works?

* All elements ( cell, viruses, world, pellets, W) are simple C# classes.
* Each element has a `Tic` method who makes game move on.
* They are a `HostedService` who calls the `Tic`s.
* `World` is injected as `AddSingleton`: one World for all people.
* `Cell` is injected as Transient: one Cell for each gamer.
* Mouse is tracked by blazor ( `@onmousemove`'s `UIMouseEventArgs` )

### Help requested:

PR's to **ToDo** list are welcome. Todo list:

* **Pellets**: Manage pellets. Complexity: low.
* **Split**: Split on *space* key press. Complexity: medium.
* **Physics**: Splited cell parts atraction and rejoin. Complexity: high.
* **Zoom**: Each user only can view their close area in a zoom view. Complexity: high.
* **W**: raise `w`s on *w* key press. Complexity: medium.
* **Eat pellets**: cell should eat pellets. Complexity: low.
* **Eat viruses**: cell should eat viruses. Complexity: low.
* **Eat cells**: cell should eat other cells. Complexity: low.
* **Soft movement**: cell should move to mouse softly (not zig zag).  Complexity: medium.
* Write here other todo's.

![screenshot](./screenshots/blagario.gif)

