// Variables to be overwritten by Unity
VAR CurrentMood = 0
VAR CurrentStoryState = 0

-> Selector

== Selector ==
{
    - CurrentStoryState == 0:
        -> Day_1
    - CurrentStoryState == 1:
        -> Day_2
    - CurrentStoryState == 2:
        -> Day_3
     ->END
}

== Day_1 ==
    Oh... uhm, hi. # Speaker: Luca # Voice: 12 #Image: Luca/placeholder
    Anyways. See ya. # Finished
-> DONE


== Day_2 ==

    ... #Speaker: Luca #Textspeed: 0.75 #Voice:12
    ... #Textspeed: 0.75
    ... Huh? Oh, sorry- uh, my music was at max. What’s up?
*   What music were you listening to?
    Oh, geez. It’s not normie music. I dunno if you would know it.
	Uh... man, lemme think of something you might actually know so you don’t look at me like I’m crazy.
	Oh, shit, yeah- you know the Lightning Legends soundtrack? I’ve been listening to that a lot lately. 
	They’re doing a remake of that game, actually. I mean, like, they’re calling it a remake but it’s obviously just a remaster. 
	The only thing that’s changing is the graphics, like, the gameplay is pretty much already perfect so no chance they mess with it. 
	But we’ll probably get a totally new cover of the OST, so I’ve been listening to the old one. 
	... Oh man. I’m sorry. I was totally rambling like an absolute cringelord...
-> DONE



== Day_3 ==
    Oh. It’s you again. Do you run this place by yourself? #Speaker: Luca #Voice:12
    Normally when I go to food places there's a different, depressed looking teenager working the register every time.
    ... That’s kinda cool I guess. Seems like a lot of work.
*   Did you see the new Lightning Legends announcement? 
	Dude, yeah- like, holy shit?
	I was wondering why they were revamping such an old ass game. 
	And then they announced that the new MMO was gonna drop after the remake, like, my poor wallet dude. Oh my god. 
	The world is never gonna see me when that drops. I’m gonna hole up in my room grinding until 4am like I’m 17 again.
*   Did you see the new Lightspeed Legends announcement?
	... Huh?...
	Oh, you mean Lightning Legends?... Uh, yeah. Pretty stoked. 
	... I don’t mean to gatekeep, but you probably should figure out the name of the game if you wanna get into it.
- -> DONE


== Good_End ==
    You’re pretty okay to hang out with, I guess. #Speaker: Luca #Voice:12
    We could… hang out and game sometime… if you want... or whatever. Y’know.
-> DONE

== Bad_End ==
    Uh, geez… I feel kinda bad saying this but, like, I’m probably just gonna stay in and eat ramen from now on. #Speaker: Luca #Voice:12
-> DONE
