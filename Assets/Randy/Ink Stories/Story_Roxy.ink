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
        -> Ending
    - CurrentStoryState >= 9:
        -> Extra
        
     ->END
}


== Day_1 ==
    HEY!!! How’s it goin’!!! Great to see a new food spot in town!!!
    That food in there smells GOOD. Can’t wait to try it!!!
    My name is ROXY. I was on my daily two mile jog and I saw a new store in town!!!
    I like my food HOT, you hear? Get to it!!!
-> DONE

== Day_2 ==
    GOOOOD morning! Working hard or hardly working? GAHAH!
*   How are you?
    I’m doing GREAT!!!
    My latest workout video BLEW UP!!! I’m almost at a million subscribers. Getting myself a POWER LUNCH to celebrate!
    You like working out? You should check out my channel! Turn those spaghetti noodle arms into PURE BEEF!!!
*   Got anything planned today?
    You KNOW IT! I’m on the grind train and I had the brakes removed!  After FUELING UP here it's back to my studio. Gotta record a new workout video for my TubeYou channel!!!
    A consistent schedule is KEY TO GROWTH!!! Remember that! 
- -> DONE

== Day_3 ==
    Grrr...
    ...
    Huh? Oh, sorry. I’m all fired up today. Can’t think good.
*   What’s wrong?
    GRAAAH!!! My stupid mom!!! She doesn’t understand what I do at all!
    I showed her how good my TubeYou channel was going and she still went off on me about getting a ‘real job.’
    It IS a real job!!! I make workout videos so people can get RIPPED at home! And people LOVE my energy!
    It’s just as much a job as anything else, right!?
* *     Yeah!
        YEAH!!! You get it! She’s just too old fashioned..
        Wants me to come home and be someone's wife... but I’m married to the GAINS!!!
* *     Erm... I dunno..
        WHAT!? CMON!!! I put time and effort into my videos!
        I gotta make the workout routines, film them, edit the videos... It’s more than just looking AWESOME in front of a camera!!!
-   WHATEVER! I don’t need to think about all that. I need NUTRIENTS!
-> DONE

== Day_4 ==
    HEY!!! Something in that kitchen smells GOOD!!!
    I didn’t know they made food this tasty outside of Magma Mountain!!! But yours come PRETTY CLOSE!!!
*   Magma Mountain?
    YEAH!! That’s the region I come from!!!
    There’s a traditional ingredient from my hometown called Volcano Spice. It’s fiery hot flavor that’s UNMATCHED!!! Plus, it’s GREAT for pre workout meals!!!
    It’s just too hot for most people to handle... they don’t sell it at grocery stores... I only ever get to have it when I go home... 
    I’d give anything to taste that SWEET SWEET spice again! My mouth is watering just thinking about it!
    GAH!! Cook faster!!! My stomach is growling for some good eats!!!
-> DONE

== Day_5 ==
    Grrr... I’m OFF my GAME today...
    My videos keep getting raided with HATEFUL COMMENTS. I’ve gotta bunch gym bros telling me I’m doing things wrong..
    That I should go back to the kitchen... AWFUL THINGS... 
    What am I supposed to do!! My mojo is GONE.
*   Take a break!
    ... NO!!! I can’t do that!
    Give in to the haters by giving up. NO!!! I just CAN’T DO THAT!!!
    I’m going to work TWICE as hard to prove them wrong. You’ll see!!!
*   Prove them wrong!
    ... GRAH!! YOU’RE RIGHT!
    I can’t get down and sad about it! That’s just giving the HATERS what they WANT!!!
    Oh, I’m all FIRED UP now!!! Thanks, little buddy! I gotta keep PUSHING THROUGH!!!
-   I gotta get some grub before I get back on track. Thanks, little guy!!!
-> DONE

== Day_6 ==
    Hey!!! Guess what?
    I’m gonna be going back to my hometown at the end of the month. It’s my moms BIRTHDAY!!!
    I’m not stoked about it. But FAMILY comes FIRST!!!
*   What’s your mom like?
    She! Is! Fine!
    . . .
    Last time we talked! We had a FIGHT!!!  Just how families are back home. she wanted me to go off and get MARRIED... wants grandkids and all that.
    But I’m an INDEPENDENT WOMAN! I’m starting my own fitness empire!!! No time for all that!!! Even though we don’t see eye to eye... I still LOVE MY MOM!!!
    GRAAH... Thinking too hard is hurting my head...

- -> DONE

== Day_7 ==
    Sniff sniff...
*   What’s wrong?
    GAH! Oh, it’s you. I was busy READING...
    I made a video addressing the NASTY COMMENTS I’ve been getting... I thought it might only make it worse...
    But all the ladies that follow me have been commenting REALLY NICE THINGS!! Sniff. Sniff.
    “You helped me believe in myself.” “I wouldn’t have been able to start getting healthier without your videos.” “You have to keep going, your workouts help me stay energized!”
    I think once my mom sees all these supportive comments, she’ll see my side!! She’ll see how AWESOME the fitness community can be!!! GRAAAH!! That settles it! I’m going home tonight, and I won’t leave until she understands that I NEED to do this!!!
    Thanks for all the help, little guy!!!
-> DONE

== Day_8 ==
    HEY little buddy!
    Just got back from Magma Mountain!
*   How’d it go with your mom?
    It went... BETTER THAN I EXPECTED!!!
*   How’d it go with your dad?
    Huh?... Get the wax out of your ears!!! I was visiting my MOM!!!
-   We talked for a long time. She tried convincing me to stay home, said I should be focused on starting a family.
    She told me she didn’t understand my job. But I didn’t back down!!! I told her about what I do!!!
    We did one of my workouts together! She was hesitant at first but by the end of it she was SMILING!!! Then I showed her all the NICE COMMENTS from my channel!!! And she looked... so... PROUD!!! 
    I think she finally understood!!! She told me to get back out there and to never give up!!! GAHAH!!
    So today I’m celebrating!!! Celebrating with a delicious meal!!!

-> DONE

== Ending ==
    HEY!!! Congratulations!!! First month under your belt!!
	
	*    Thank you! (Affection Ending)
	    NO PROBLEM!!! You got through the warm up, now you gotta keep it GOING!!!
    I know you can do it!!!
	    You know what? We should do a COLLAB!!!
	    YEAH!! You should come over to my place!!! You’ll show me how to make a DELICIOUS and HEALTHY meal, and I’ll show you how to get huge GAINS!!!
	    GAHAH! It’ll be GREAT!

	*    Thank you! (Neutral Ending)
	    NO PROBLEM!!! You got through the warm up, now you gotta keep it GOING!!!
    I know you can do it!!!
	    And your friend Roxy will be cheering you on the WHOLE TIME!!! And eating your DELICIOUS CUISINE!!!
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