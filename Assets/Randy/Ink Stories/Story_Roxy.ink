// Variables to be overwritten by Unity
VAR CurrentMood = 0
VAR CurrentStoryState = 0

-> Selector

== Selector ==
{
    - CurrentStoryState == 0:
        -> Day_2
    - CurrentStoryState == 1:
        -> Day_3
    - CurrentStoryState == 2:
        No current dialogue
     ->END
}


== Day_2 ==
HEY!!! How’s it goin’!!! Great to see a new food spot in town!!!
That food in there smells GOOD. Can’t wait to try it!!!
*   How are you?
    I’m doing GREAT!!!
	My latest workout video BLEW UP!!! I’m almost at a million subscribers. Getting myself a POWER LUNCH to celebrate!
	You like working out? You should check out my channel! Turn those spaghetti noodle arms into PURE BEEF!!!

*   Do you like any special ingredients in your food?
    As a matter of fact I DO!!! 
    There’s a traditional ingredient from my hometown called Volcano Spice. It’s fiery hot flavor that’s UNMATCHED!!! Plus, it’s GREAT for pre workout meals!!!
	It’s just too hot for most people to handle… they don’t sell it at grocery stores… I only ever get to have it when I go home… 
    I’d give anything to taste that sweet sweet spice again! My mouth is watering just thinking about it!
-   HAHA! Is that my food you’re cooking up? I can smell it! Cook faster little guy! Go! Go! Go!
-> DONE


== Day_3 ==
    Grrr...
    ...
    Huh? Oh, sorry. I’m all fired up today. Can’t think good.
*   What’s wrong?
	GRAAAH!!! My stupid mom!!! She doesn’t understand what I do at all!
    I showed her how good my TubeYou channel was going and she still went off on me about getting a ‘real job.’
	It IS a real job!!! I make workout videos so people can get RIPPED at home! And people LOVE my energy!
    WHATEVER! I don’t need that. I need NUTRIENTS!
-> DONE

== Good_End ==
YEAH!!! This food is THE BOMB!!! I gotta get you on my channel sometime for an AWESOME COLLAB!!!
-> DONE

== Bad_End ==
    GRAAAH! This place isn’t getting me FIRED UP!!! You gotta get it together, little man!!!
-> DONE