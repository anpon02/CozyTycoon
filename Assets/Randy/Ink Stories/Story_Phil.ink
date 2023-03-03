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
    - CurrentStoryState    11:
        -> Extra
        
     ->END
}


== Day_1 ==
    Good morning. My name is Phil.
    Pleased to meet you.
    A new restaurant in the area is a nice change to see. Should stimulate the local economy.
    It’s also a walkable distance from my office. They say walking improves productivity, you know.
    Speaking of. I won’t distract you any longer.
-> DONE

== Day_2 ==
    Hello again.
    Sorry, I can’t talk very long.
    I’m at BeigeTech, across the street. Been a hell of a quarter and I need to be working double time to stay on top.
*   BeigeTech?
    Yes, that’s correct. Our company specializes in product management and communications.
    I won’t bore you with all the details.
    My job is reliable and pays well, even if it isn’t the most exciting title.
*   Double time?
    Yeah, I’ve been working late hours recently.
    Not too uncommon. We’ve got to stay ahead in this business, so everyone needs to do their part.
    Means I haven’t had much time to myself, but I’m sure the company will reward me for my loyalty.
-   I’ve got to get back to it. Lovely talking with you.

-> DONE

== Day_3 ==
    ... Sigh.
*   What’s wrong?
    Oh, personal things...
    ... I had a date last night. It didn’t go very well.
    One of my coworkers, Elaine. I thought since we work together she would have understood when I needed to take an important call.
    She was very upset with me for it. I don’t think there will be a second date.
* * Poor Ellen...
    ... Elaine. Her name was Elaine. Were you listening to me?...
    Uhg, my apologies. I shouldn’t be rambling on and on to someone like this. 
* * Poor Elaine...
    ... Yeah, you’re right. It wasn’t my intention, but I’m sure it felt like I wasn’t valuing our time together.
    Maybe dating just isn’t in the cards for me. I hardly have any free time, and when I do I’m usually exhausted.
    ... Sigh.
-   I need to focus on my work again. Don’t want to fall behind.
-> DONE

== Day_4 ==
    I love the smells of this place. It’s creating a relaxing environment for me to get things done in.
    The cafeteria is... not a relaxing environment. It’s loud, the food is bad, and it kind of smells like eggs.
    We used to have a coffee maker in there that could add flavorings to your coffee.
    I always got the Caramel flavorings. Just a little bit of sweetness, but it really helped wake me up with something delicious.
    ... Then they took the coffee maker out. Said the employees were spending too much time standing around it instead of being at their desks.
    Oh well. I guess I’m happy with a normal cup of coffee. Don’t want to be spending too much on frivolous things.
    I’m happy just being able to get outside the office for a little while. Stretch my legs.

-> DONE

== Day_5 ==
    Sigh. Just got off the phone with my younger brother.
    Peter’s back home in the Enchanted Grasslands. Keeps calling me to try and get me to go on some backpacking trip with him.
    We used to go hiking all the time when we were kids. It’s really beautiful out there- nice to gallop- er, run around.
    But I grew up and he stayed the same. I got a job and he still lives with our parents. It’s... mildly frustrating.
    You have to grow up at some point. Participate in the real world. Stop living like you’re a kid.
    ... Sigh. I haven’t been home in a while, though. Maybe I owe him a visit. I’ll go this weekend.
-> DONE

== Day_6 ==
    Sorry. Can’t talk.
    There’s a big promotion on the table- I’ll probably be working overtime all week to get it. Weekend too.
*   What about your brother?
    Peter? Oh. You remembered him. I wasn’t sure if you were paying attention to my rambles...
*   What about your sister?
    My... who? I don’t have a sister. Only one younger brother.
    I swear I’ve mentioned him before..
-   Uhg... I did promise him I was going to visit over the weekend. But I can’t anymore, I need to secure this promotion.
    I’m the most qualified for the job, and I’ve been at the company the longest. Taking personal time off right now will make it seem like I’m not serious about the position.
    Sorry. I’m rambling again. Back to work for both of us. 

-> DONE

== Day_7 ==
    Where do you get your ingredients?
    Probably local, I’m guessing. All the meals here have tasted very fresh.
    My family makes cheeses and stuff out in the Grasslands. There's a Three-Cheese Blend in particular that’s quite popular.
    My brother and I would eat it with pasta all the time. It’s really quite good.
    Your cooking is about as close to a home-cooked meal that I’ve got in a while. I don’t have time to cook nice things for myself, so I usually eat takeout or microwave meals.
    There’s the occasional fancy corporate dinner. I shouldn’t complain.
*   How are the Grasslands?
    It’s a beautiful region. One of the only places that hasn’t been modernized, everything there is very traditional.
    Lots of centaurs running around with their manes- uh, hair flowing in the wind. Small, tight knit communities, you know.
    I left because I wanted to be a part of something bigger than that, I wanted to be someone important.
    ... But I’m not.
    Sometimes I wonder if I made the right decision.
*   How is your brother?
    He’s... mad at me right now. 
    It’s fine. Always the same. He never understands that I have a real job, that I can’t just up and leave whenever I want. Then he’s mad for a while, but he always comes around.
    He said we haven’t gone backpacking in five years, though. 
    I didn’t realize it had been that long...
    ...
-   Ah. You always get me to ramble. 
    ... Thanks for listening.
-> DONE

== Day_8 ==
    Uhg.. I have a headache...
    I worked overtime and took the morning shift today...
*   Why?
    The promotion. I’ve got to do everything I possibly can.
    If I’m not giving a hundred percent, someone else will be, and then they’ll get the job instead of me.
    Once I can secure this promotion it’ll be easier. I’ll be working less hours for more pay.	
    That’s the dream, right?
    Sorry to cut our conversation short, I just really need to focus right now.
-> DONE

== Day_9 ==
    ...
    ...
    ... Sigh...
*   Are you ok?
    I don’t know.
    Remember that big promotion I’ve been talking about? I... didn’t get it.
    It went to Josh. I should’ve figured. He’s the boss's son, I never stood a chance...
    ...
     I don’t know what to do now.
* *     Try again?
	    Try again? Are you being serious? Have I ever sounded happy talking about my job?
	    ... Sorry. I didn’t mean to snap. It’s been a long day.
	    I feel so lost... I don’t know where I’m supposed to go from here.
* *     Try something else?
	    Something else?... But I’ve been working at this company for ten years. To abandon it now...
	    ... But look where ten years has got me. Passed up for a promotion I lost sleep over.
	    Maybe you’re right.

-   I’ll think about it. Nothing else I can do right now.

-> DONE

== Day_10 ==
    Good morning.
*   How are you?
    I’m alright.
    I’m wrapping up a few things, but then I’m going home to pack. I’ll be going to the Grasslands to backpack with my brother.
    He seemed really happy I changed my mind...
    I don’t know if quitting my job is the right move right now, but I think being out in nature will give me time to think and reflect.
    I’ll clear my head and make a decision after that. 
-> DONE

== Day_11 ==
    Congratulations on the month!
    Most small businesses fail within their first month. So, statistically speaking, it’s only going to get better from here.

    Thank you!
    Of course. 
    ... Uhm. I wanted to ask you something. It’s alright if you say no.
    I’m going back to my hometown to go on a backpacking trip with my brother. I was wondering if you’d want to come.
    You’ve been so kind to me, and my brother loves meeting new people. I’m sure there's lots of unique ingredients in the Enchanted Woods too.
    I’d like to spend time with friends. I haven’t in a long time.

    Thank you!
    Of course.
    Keep up the good work. I like having a spot so close to work, it would be a shame to see you go out of business.
    You’ll always have me as a loyal customer no matter what.
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