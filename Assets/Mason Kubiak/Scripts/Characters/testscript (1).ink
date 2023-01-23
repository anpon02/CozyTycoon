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
This is first story
// * okay
The character feels {CurrentMood} Right now.
End story ends here
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