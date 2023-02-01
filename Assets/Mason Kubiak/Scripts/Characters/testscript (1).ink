LIST Mood = horrible, bad, neutral, good, great
VAR CurrentMood = neutral

VAR CurrentStoryState = 0
-> Selector

== Selector ==
{
    - CurrentStoryState == 0:
        -> story_0
    - CurrentStoryState == 1:
        -> story_1
    - CurrentStoryState > 1:
        end of existing dialogue
     ->END
}

== story_0 ==
I am speeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeed #Textspeed: 4 #Linedelay: 0.25 # Voice: 12
am slow #Textspeed: 0.25 #Linedelay: 2
H-hey, how's it goin? #Speaker : ??? #Image : Guy/normal #Voice:12
* okay
* big oof
- Oh, don't mind me, I've got placeholder assets right now,
but I needed to get some work done and get some food
Oh, I'm a placeholder character, by the way. # Speaker: placeholder #Image: Guy/good
- ~CurrentMood++
~ CurrentStoryState++
-> DONE

== story_1 ==
This is the second story
The character feels { CurrentMood } right now
/*
* Choice that makes the character happy
    :D
    ~ CurrentMood++
* Choice that doesn't affect character
    The character looks at you strangely
* Choice that upsets the character
    D:
    ~ CurrentMood--
-     Character's mood/relationship: { CurrentMood }
*/
~ CurrentStoryState++
-> DONE


== Good_Food ==
text
~ CurrentMood++
-> DONE

== Bad_Food ==
dum
-> DONE