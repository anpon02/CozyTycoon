// Variable(s) to be overwritten by Unity
VAR CurrentStoryState = 0

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
        -> Day_9
    - CurrentStoryState == 9:
        -> Day_10
    - CurrentStoryState == 10:
        -> Day_11
    - CurrentStoryState > 11:
        -> Extra
        
     ->END
}


== Day_1 ==
// Story Content goes here

-> DONE

== Day_2 ==
// Story Content goes here

-> DONE

== Day_3 ==
// Story Content goes here

-> DONE

== Day_4 ==
// Story Content goes here

-> DONE

== Day_5 ==
// Story Content goes here

-> DONE

== Day_6 ==
// Story Content goes here

-> DONE

== Day_7 ==
// Story Content goes here

-> DONE

== Day_8 ==
// Story Content goes here

-> DONE

== Day_9 ==
// Story Content goes here

-> DONE

== Day_10 ==
// Story Content goes here

-> DONE

== Day_11 ==
// Story Content goes here

-> DONE

== Extra ==
// Extra Content goes here if we need it

-> DONE

== Good_End ==
// If this is still in use. text goes here

-> DONE

== Bad_End ==
// If this is still in use. text goes here

-> DONE