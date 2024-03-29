// Variable(s) to be overwritten by Unity
VAR CurrentStoryState = 1
VAR AffectionEnding = false

// Story starts here, and the proper ink storylet is chosen with help from Unity
-> Selector
== Selector ==
{
    - CurrentStoryState == 0:
        -> Day_1
    - CurrentStoryState == 1:
        -> Day_2
    - CurrentStoryState == 2:
        -> Day_3
    - CurrentStoryState == 3:
        -> Day_4
    - CurrentStoryState == 4:
        -> Day_5
    - CurrentStoryState == 5:
        -> Day_6
    - CurrentStoryState == 6:
        -> Day_7
    - CurrentStoryState == 7:
        -> Day_8
    - CurrentStoryState == 8:
        -> Ending
    - CurrentStoryState >= 9:
        -> Extra
        
     ->END
}


== Day_1 ==
    Huh? What do you want? #Speaker: ?????
    Oh. You work here. Right.
    ... What are you looking at?... I already paid. 
*   What’s your name?
    Who’s asking?
    What are you, a cop? Mind your own business. 
-> DONE

== Day_2 ==
    Sorry about yesterday. I was a bit jumpy. Had a bunch of guys tailing me. 
    Uh. My name’s Tripp. 
    Just a head’s up- your new little shop is in Yellow Jacket territory, but three blocks away is Sewer Snake territory. This is a nicer part of town so I doubt that’ll ever matter. Just keep your head up. 
*    ... What?...
	Uh. Anyways. This place smells nice. 
-
*   Is there anything you’d like to see on the menu?
    What, like, new menu items?
    Man, I love soup... I would kill a dude for soup. 
    ... That was a joke. Don’t look so freaked.


-> DONE

== Day_3 ==
    Ow...
    Uhg. Sorry. Got into a fight on the way over here.
    You should see the other guy. Got me pretty good in the eye, I’ll admit. But he was a chump. 
*   A Sewer Rat?
    Uhg, no- Sewer Snakes. You gotta pay attention to this stuff or it’ll come back to bite you.
*   A Sewer Snake?
	Good. You pay attention.
-   Needed to go into their part of town to get my aunt’s medicine and they like to start shit for no reason.  I wasn’t doing nothin’ and they try to rough me up anyways. Like I look I’ll just roll over and take it.
    Uhg. Whatever. Worked up a hell of an appetite... You keep quiet about all this stuff, yeah? 
    If anyone asks, I was never here.

-> DONE

== Day_4 ==
    Uhg. The high school kids are out this time of day. 
    Waste of time. School, yknow- Like, what are they even teaching in there that you’re gonna use in your everyday life? I haven’t done a single algebra problem since I dropped out. 
    ... Beating people up for money isn’t any better, I guess. But at least I’m not stuck in a desk listening to Mrs. Higgs talk about trigonometry.
    By the by, you ever heard of Scorpion Paste before? It’s mostly for soups- makes it taste amazing.
    I don’t know if it’s made of actual scorpions. I think it’s called that ‘cause it makes your tongue sting a little. But all the best foods sting.

    This place on 3rd street used to sell it - my aunt and I would go there all the time
*   Your aunt?
    Uh. Yeah. I live with my aunt. 
    She’s taken care of me ever since my mom... uh.
    I’m sure you don’t need to hear all that. I live with my aunt. Maybe one day she’ll have enough time off from her stupid fucking job to swing by.
    ... She’s real nice. Too nice to be living downtown.
-> DONE

== Day_5 ==
    Uh. Hey. 
    You know where to stash a motorcycle?
    No.. of course you don’t. Stupid question.
    Uh... I had some beef with this guy. Was walkin’ this morning and lo and behold- left his brand new motorcycle right there in the open.
    ... Locked up, obviously. With the keys in his pocket. But right there. So I roughed the guy up for the keys and took it.
    My arm hurts something awful but I got away. Poor guy wasn’t even conscious when I drove it off.
    ...
    I gotta go home and explain this to my aunt...
*   She’ll probably be worried.
    Oh.. You’re right.
    She’s a sweet lady n’ all but... She cares about me still... Don’t know why but..
*   She’ll probably be angry.
    Uhg, I know that already. Don’t need to remind me.
-   Whatever. I got a free new ride. Would like to see that guy come and get it off of me.
    Pretty good day, all in all.
-> DONE

== Day_6 ==
    ... Hey. Sorry if you could hear yelling outside.
    Was on the phone with my boss. 
    ...
    You always got that face people make when they’re judging me but are too polite to say it.
    Cut me some slack, alright?
    I don’t have great options, not without a highschool diploma. 
    I gotta repay my aunt for all she’s done to take care of me. I make alright money doing this- enough to cover our rent. I can’t exactly be too picky.
    Plus, I gotta gecko to feed.
    His name is Scooter. He’s my little son... Needs lots and lots of love and even more food. 
    Uh. Anyways. It isn’t all bad. I get to hang out in nice places like this sometimes n’ forget about it all, yknow.
-> DONE

== Day_7 ==
    ... Uhg. Sorry. Not in a super talk-y mood.
    Had a big fight with my aunt. I mean, this happens sometimes, but it was really bad today.
    Normally she brings up me getting a ‘real job’ so we don’t have to talk about the elephant in the room. Kinda skirt around the fact that I get paid to steal and hurt.
    But today she just... man, she broke down. I came home with a busted up eye and needed it iced and she just started crying.
    Saying that I don’t care if something happens to me. That she could never forgive herself if I got hurt real bad out there n... 
    I just felt awful. I never thought about it like that. I always just wanted to pay her back for taking care of me. 
    I didn’t think that me doing that still hurt her.
    I feel like an asshole. 
    ...
    ... Guess I was in a talk-y mood. I don’t blame you if you tuned all that out but.. Uh, thanks? N... Whatever. Moment over.
-> DONE

== Day_8 ==
    Hey. 
    Uh.. was supposed to do a job today. But I didn’t.
    My boss wanted me to snag a bike from someone. Real easy thing to piss them off, tell them to get out of our part of town. So I walked the whole ways down there.
    Y’know, I was standing there. And I could’ve taken it. But I got to thinking about what my Aunt said and all and I just...
    ... I don’t know. I didn’t want to let her down again. I left. 
    I’m sure the other Yellow Jackets will be mad at me, but I don’t care. They all know I could beat their asses. They better leave me the hell alone.
    I’m not ever doing their dirty work again. I’m out.
    ... So, uh. Is this a celebration meal? I’m kind of eating my feelings tonight, yknow. Nice food to distract from the horrible decisions.
    ... Thanks for always listening and all. 
-> DONE

== Ending ==
    What’s good? You made it a whole month. Way to stick with it...
*   { AffectionEnding } Thank you!
    I’m not sure if you care, but I’m trying to turn over a new leaf or... whatever.
    ... I was wondering if I could work here. Uh. If you’re hiring...
    I could deliver food on my motorcycle. Or, y’know, learn how to cook stuff so it’s not just you back here. Uh.
    You’ve been really nice to me. And I want a new start. I’ll do my best, I promise. 
*   { !AffectionEnding } Thank you!
    Uh, yeah. No problem.	
    I’m not sure if you care, but I’m trying to turn over a new leaf or... whatever.
    Glad to have a nice place like this to come to. Y’know, to get away from it all. 
    Thanks for that. 
- -> DONE

== Extra ==
// Extra Content goes here if we need it

-> DONE

== Good_End ==
// If this is still in use. text goes here

-> DONE

== Bad_End ==
// If this is still in use. text goes here

-> DONE