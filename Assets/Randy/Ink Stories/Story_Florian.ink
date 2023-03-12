// Variable(s) to be overwritten by Unity
VAR CurrentStoryState = 8
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
    Ah, well met!
    My name is Florian. How nice it is to see a restaurant owner who takes the time to greet their guests. And one with a ‘welcome in’ sign outside. Very... handy... Ahem.
    Well, I’m pleased to meet your acquaintance! I’m sure I’ll be stopping in here for a bite every once in a while.
    A bite of... food, yes. Anyways. Thank you!
-> DONE

== Day_2 ==
    Hello again, friend! 
    How are you faring? You have an aura of confidence about you that I had not noticed on my visit prior.
    It seems you have started to feel at home in this new business of yours, what a wonderful thing!
    I felt a similar way a few months into my current job. I teach magic and history at the college up the road from here. Forty years since I started, if I remember correctly.
    I was entirely overwhelmed at first. So much to learn in so little time! I have the knowledge, yes, but teaching it to sleepyheaded college students is another beast entirely.
    And, yet, I found my footing somehow. But you always keep learning how to go about things better! I’ve spent a long time at my institution and have yet to fully master the art of teaching.
*   Forty years is a long time.
    ... Aha-!! Yes, I suppose it is. 
    I’ve... I’ve been told I don’t look my age. I suppose it's a bit hard to tell.
    I have a... very rigorous skincare routine.
    ... Ahem.
*   Four years is a long time.
    Oh, ahm.. Ahem. Off by a factor of ten, there.
    Is it that hard to believe I’ve been at the institution for forty years?
    ... Perhaps that was a joke? Ah, I’m sorry. 
    ... Anyways.
-   Thank you for the lovely chat. It’s quite pleasant talking to you.
*   Is there anything you’d like to see on the menu?
    Oho! You’re asking my opinion?
    Well, I must admit I’m rather fond of salads. Light meals to tide me over... uhm. Ahem. 
-> DONE

== Day_3 ==
    ... Yawn. 
    Ah... my apologies, I find myself quite tired in the daytime hours these days.
    Ah- not for any particular reason! Perhaps I should simply go to sleep at an earlier hour. I relax in my study with a good book and the hours get away from me.
    I used to sprinkle Fairy Dust into my meals. It has some magical properties, all the effects of caffeine without the unpleasant taste.
*   Do you not like coffee?
    Mmm... it is not my favorite taste, no.
    I much prefer a nice cup of tea. The aroma is much more preferable, and the taste is subtle. Coffee is blunt, it lacks elegance.
    I meander the streets of our good town and see its corners filled with coffee houses. 
    I remember in my youth being able to visit a tea room whenever I pleased. Light meals and a more refined atmosphere. It’s a shame I can’t find the same today.
    ... Aha! I shouldn’t talk so. It serves only to age me. 
*   What books do you like to read?
    Oh my... it’s a bit embarrassing, actually.
    I used to spend my time with more academic works. I love to learn everything this world has to offer- but lately I find them a bit tiresome.
    I work in academia, of course. I’m surrounded by it all my waking hours. So recently I’ve found myself becoming quite fond of... ahem. Romance novels.
    It’s funny, really. I used to look down on others for reading those sorts of books, but I understand their appeal now. A sort of escapism.
-   Oh, look at me, prattling on again. I’ll leave you to your work.
-> DONE

== Day_4 ==
    ... Oh... I’m sorry... I’m afraid I’m fighting my own battles today.

*   What’s wrong?
    Oh dear... I’m afraid I’ve kept an awful secret from you. It is my best kept secret, but it rips me apart inside to keep it from you...
* *     You’re a vampire?
	    Wha!? How could it be, you knew all along?!
	    And yet... despite knowing my dark secret... you treated me as a friend all the same...
	    Sniff sniff.. Oh, I’m alright. I’m simply overcome with emotion. 
* *     You’re a werewolf?
	    Excuse me?
	    No, no. I’m... ahem. A vampire.
	    Do I give the impression of someone who transforms into a large, hairy beast? Oh dear...
	    No, no. Werewolves are pack creatures. The path I walk is far more isolating.
-   It’s quite a lonely thing. Many people would look upon me with fear or disgust for confessing such a thing.
    The age of vampires has long since ended- my kind was hunted for sport some hundred years ago. Very few remain.
    And, yet, by comparison I am a relatively young vampire. I was turned sixty years ago by one of the surviving vampires. And my life has not known the warmth of companionship since. 
    How difficult it is to foster a friendship with someone knowing you cannot be your true self around them- and, yet, it is just as impossible to be true when some may look at you with fear and disgust for a thing you cannot help.
    ... Oh, but I’ve said too much already. I hope you will still welcome me in your establishment all the same.
    I do not hurt people, if you could even think such a thing of me to begin with. I’ve spent many years investigating an alternative to... the suspected diet of someone like me.
    The food you serve is one of these solutions. It does not sustain me as much as.. Ahem. But it serves until I have to feed again. 
    Ack. What an awful thing to have to discuss. Please discard such mental images- they do not define me. I hope we can continue to be friends. 
-> DONE

== Day_5 ==
    Hello again.
    Try as I might, I cannot rid from my mind the thought that I may be going about my own life the wrong way.
    For the last sixty years I’ve kept to myself, keeping my acquaintances at arm's reach. 
    It’s incredibly difficult, you must understand. To know that all the bonds I make are only temporary- that I will outlive all the people who become dear to me.
    But, now, I’ve isolated myself out of fear of losing someone. My only friend is you, truly.
    If you consider me that?..
*   Not really..
    Oh...
    Well, maybe it was presumptuous of me to think you would consider me a friend.
    We’ve only spoken in your restaurant, after all...
*   Of course.
    Oh, goodness, it might not mean much for you to say but it means everything for me to hear.
    Being able to talk to you these past days has opened my eyes, I think. Reminded me how nice it is to keep up with a friend, have an actual relationship...
-   I’ll continue to try to get to know you better. These visits to your restaurant have been a breath of fresh air. 
-> DONE

== Day_6 ==
    Hello again, my friend!
    I do so apologize for the way I have acted recently. I’m in much better spirits today.
    Even better seeing you! I hope you have been faring well.
*   How is the school?
    Oh, as lovely as ever.
    After our conversation last, I was feeling a bit more optimistic about the whole ‘getting to know people’ business. 
    I offered to buy coffee for my coworker and we spent the morning chatting. It was delightful.
    ... Well, until the sun peeked out from behind the clouds and I had to swiftly move to the other side of the table.
    I brushed it off with grace, though! Claimed there was a bee attempting to ruin my day, he laughed it off. 
    I still feel bad not telling the whole truth, but... It’s progress. Perhaps soon I will be able to have that conversation with my peers. 
*   How did you become a vampire?
    Oh, my... What a personal question...
    Haha! I’m only joking. It’s a bit of a funny story, really.
    I was trying to make a living as an artist. Although it was fulfilling, it was not the most profitable job.
    With no place to call home, I took a job at a cemetery. The work was quiet and I was given board, I simply had to keep watch of the place.
    Well, of course, I was terribly curious about some of the older structures. I ventured too far into one of them and discovered a coffin...
    I’m sure you can guess what happened after. I had disturbed some old vampire who was in a state of hibernation, and he desired a meal after his long period of rest. 
    I’m not sure where he went off to...
-   Oh, well. I’ll worry about that another time. Right now I’m taking in the lovely ambience of your store.
    Thank you as always for your time!
-> DONE

== Day_7 ==
    Oh, my friend, I need your advice.
    You’ve been nothing but helpful thus far, so I come to you with news...
    My, ahm.. Partner, the one from before I became a vampire. He must have found my name through the school.
    I arrived at work to find a letter addressed to me on my desk. It was an invitation to meet again over brunch. 
    Oh, what do I do? No doubt he’s aged all these years, but I’ll show up looking as young as the day I left him...
*   Keep it a secret!
    Oh, that’s no good.
    Keeping it a secret is exactly what got me into this trouble in the first place. I hardly think it would be advisable to continue down that route. 
    If I do end up going, I ought to be honest...
*   Tell him the truth!
    Oh, you’re right, aren’t you?
    It’s exactly what I need to be told, but it pains me to hear it all the same. 
    I can only hope he’ll forgive me, understand why I left the way I did...
    I don’t think it can go back to the way it was, but perhaps we could still be friends. 
-   I need time to think it over... And a nice, warm meal. 
-> DONE

== Day_8 ==
    Good day, friend. 
    I did end up going to meet my old partner after all.
    I was up all night deliberating- well, you know, later than usual... I decided ultimately that hiding away would only continue to hurt the both of us.
*   How did it go?
    I expected him to be angry with me. Understandably so, I left so suddenly, but...
    ...despite how I left, he told me he was happy to see me again. He welcomed me with open arms. We talked like no time had passed between us. 
    ... Sniff sniff.
    Oh, it's a wonderful thing to connect with others. I’m a fool for hiding myself away for so long.
    But I shouldn’t despair, I’m in a much better place now, aren’t I? I have you as a friend, and I think that’s wonderful. 
-> DONE

== Ending ==
    I bid you good day! What a momentous occasion- a full month! What a testament to your hard work and determination!
*   { AffectionEnding } Thank you!
	    No, thank you. 
	    I must remark that you have provided me with much more than simple meals.
	    In my endeavor to better connect with my fellow creatures, I would like to invite you to tea. 
	    It would bring me much happiness to get to know you more outside of a restaurant setting. I hope that we can become good friends. 
*   { !AffectionEnding } Thank you!
	    Thank you! 
	    You have brought joy to this community through the culinary arts.
	    I wish you the best of luck in all your future endeavors!
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