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
    - CurrentStoryState >= 11:
        -> Extra
        
     ->END
}


== Day_1 ==
    Oh! Hello there! 
    My name’s Sssally, how are you?
    I work at the flower shop up over yonder ‘round the curb. Wasss walkin’ on back to my house when I noticed your little establishment.
    So if ya ever need any sorts of flowery decorationsss, come and give me a visit!
-> DONE

== Day_2 ==
    Hey there again sssweetie!
    Aren’t you up and at ‘em! Makesss me real happy to sssee.
    Isss it just you here? Must be awful quiet back in that kitchen if you ain’t talkin’ to nobody.
    My whole family runsss our flower shop. I got a sssister and three brothersss. And my ma and my pa, of course.
    We’re all real clossse. We eat dinner together every night. Itsss loud, but it’sss real lovely.
    There’s thissss Bread Bowl my mama makes thats somethin’ special, I wonder if you ever tried your hand at makin’ somethin’ like that?
    It’s just sssoup in a sssourdough bowl instead’a normal one. But somethin’ about it is just so delicious.
    You ssshould try it! You could stop by, but maybe itsss more fun making it for yourself.
    Oh, look at me, trying to tell you how to do your job. Thanksss for indulging me.
*   Is there anything you’d like to see on the menu?
    Oh! Well, I’m a big fan of sssoups. Easssier for me, sssince I don’t gotta cut up everythin’ into little bitsss.
    Any kinda sssoup, really!

-> DONE

== Day_3 ==
    Howdy, little guy.
    How goesss the restaurant?
    The shopsss been good. I got to arrange a whole buncha bouquetsss today. 
    I love putting together flowersss like that! My favoritesss the lilies. I try to put them in everything, no matter the occasssion. 
    But itsss fun arranging flowers for different thingsss. Birthdaysss, holidaysss, first datesss. Putting it together feelsss sort of personal.
    Isss it like that for cooking too? 
*   Not really
    I sssee...
    I guesss if you have a lot of ordersss you can’t take the time to make it ssspecial for each person...
    Ssseems like a missed opportunity...
*   Yeah
    Hehehe. I guesss we have something in common, then.
    Arranging flowers and arranging a dish... they’re both an art, I think.
    Sssomething you can connect to other people with.
-   Oh, dear... I don’t mean to go on and on. Thanksss for the chat.
-> DONE

== Day_4 ==
    I’m glad you ssset up shop so close to my house. 
    It’sss fun to sssee all the new shopsss and buildingsss that have shown up in the past few yearsss.
    They’re building a new apartment building next to my ssstore. Must be on account’a the fact this town keepsss getting bigger.
    A lotta folksss in this town come here to leave their familiesss...
    I guesss I understand some people don’t have the bessst relationship with their folksss... but I can’t imagine living without my family.
    I would be missing’ out on a lotta homecooked meals without them, that’sss for sure!
*   What food does your family make? 
    Oh, all sortsa stuff. 
    My pa always sprinkles a little Cinnamon into everythin’ he makes. Saysss it makesss it better.
    ... Sometimesss that meansss my ma gotta step in ‘cause there’s some things cinnamon just doesssn’t belong in. But itsss one of those thingsss, you know?
    Now whenever I taste Cinnamon it remindsss me of my pa. 
    Anyhow, was just a little sssomethin’ I was thinkin on the way over here.
-> DONE

== Day_5 ==
    ...
    ...
*   Hello?
    ... Huh?
    Oh, sorry sugar. I guess I’m a bit out of sortsss today.
    Uh, don’t wanna get too into it, but...
    My pa ended up in the hospital last night. He’s a real old fella and... uhm...
    ... Sorry, sweetheart, I think I’m just gonna sit here all quiet like today. I’m afraid I won’t be any good to talk to.
-> DONE

== Day_6 ==
    Oh... Hey there, dear.
*   How’s your dad?
    Oh... thank you for assskin, darlin. It meansss a lot that you came all the way over here to check on me.
*   How’s your mom?
    ... Well, she’s not doin’ great right now, but at least she’s healthy. It’s my pa who’s in the hospital right now.
-   He’sss... not doin’ well. It’s the same illness he’s had a long while. He keeps fightin’ it but it keepsss coming back n’...
    I’m trying to focus on the positives right now. I got all my other family back home to help me through it, n’ our ssshop is doin’ real well.
    We knew thisss day would come eventually. I ssshould be grateful that I’ll be able to sssay my goodbyes, lotta folk lose their family outta nowhere and...
    ...
    ... Even so, itsss still hard... But I’m looking on the bright side. I got nice friends like you to talk to.
    Thank you for bein’ here for me.
-> DONE

== Day_7 ==
    Hi, darlin.
*   How are you doing?
    Oh, not great. Had a real tough night yesterday.
    Sssayin’ goodbye and all that. I won’t bog your mood down with all the detailsss, but I been feeling awful low this morning.
    You know, when I wasss younger, I used to think that I could handle everything on my own. 
    I didn’t wanna hafta rely on anybody when I got older. I wasss all young and rebelliousss and thought I would get outta here the sssecond I could.
    But I was wrong. My family’sss the most important thing to me, ssspecially now. 
    We jussst gotta be there for each other right now, I suppossse. Not much more we can do than that. 
    ... I feel a bit better talkin’ about it all out loud. Thank you, sssweetheart.
-> DONE

== Day_8 ==
    Good mornin’. Sssmells awful nice in here today.
*   How are you feeling?
    Oh, more the sssame. Some thingsss just take time, I reckon.
    It’sss been nice havin’ someone else cooking for me sometimes. Takesss a weight off my shoulders to just swing by here and pick somethin’ tasty up.
    Plus, there’s a lotta awful nice folksss that show up here. You got a nice little operation goin’ on here, darlin’.
    I don’t wanna be all sssappy about it. Just mean that having sssomethin’ ssstable when everything is so uncertain is nice.
    Thanks for that, sssweetheart.
*   Don’t mention it.
    No, I mean it! 
    I oughta bring you sssome flowers from my shop for being sssuch a friend! 
    Nice blue onesss, maybe. Or some yellow ones. 
    I’m already picturing it now... heheh! 
    I’ll let you get back to work while I ssscheme up a nice arrangement for you.
- -> DONE

== Day_9 ==
    Howdy, darlin’. 
    Hope you’ve been well. I’m feelin’ a bit easssier today.
    I’m gonna ssspend some time with my family tonight, make a big ol’ dinner.
    We stopped havin’ our big family dinners after my pa got admitted to the hospital.
    Everythin’ was just happenin’ ssso fast, you know? Ended up ordering a lotta food, n’ our neighbors brought dishes over to help out...
    We haven’t really been talkin’. Much. Everyone’s doin’ their own thing, trying to get by in their own way.
    But I wasss missin’ sssittin’ round the table and seein’ em all, so I’m makin’ a big ol’ pot of sssoup. Went out and bought sssome bread bowlsss too!
    Foodsss such a part of our everyday life. I hope we get back on track with havin’ family dinnersss again.
    For now, though, I’m excited to taste what you got cookin’ today!
-> DONE

== Day_10 ==
    Well, good mornin’ sweetheart. 
    It's sssure good to see ya again. 
    I’m feelin’ a lot better today. Ended up havin’ that big family dinner I wasss talkin’ ‘bout.
    We talked a lot ‘bout whatsss been goin’ on. Ssspent a few hoursss sittin’ round the table. Wasss sad but... nice in the end.
    My Ma felt awful that we haven’t been there for each other like we should.. Wanted usss all to go out to the movies together this weekend.
    I feel a lot better havin’ talked about it all with them.
    I’m tryna focusss on moving on now, I think.
    Think ‘bout the thingsss that make me happy. Like the flower ssshop! And ssspending time with nice people like you.
    I hope I’m not repeatin’ myself too much, but ssspendin' time with you and tryin' out all these new dishesss has been one of the best things that's happened to me these past few weeks.
    I’ve been through sssome tough times lately, but I'm lookin' forward to keepin' up with you.
-> DONE

== Day_11 ==
    Hi, darlin! Congratulationsss!
*   Thank you! (affection ending)
    I brought ya a little bouquet. Nothin’ all fancy, jussst to celebrate.
    Wanted to asssk you somethin’ while I got you here.
    I wasss wondering if you wanted to come over to my place for dinner. You been sssuch a friend to me. I wanted to do a little sssomething for you too.
    I wasss thinkin’ you let me cook for you for once. You know my family’sss big on cookin’, I’m sure you’d love it!
    It’ll be fun. Maybe you’ll get inssspired, haha!
    In any case, thanks for bein’ sssuch a friend. It means a whole awful lot to me.
*   Thank you! (neutral ending)
    I brought ya a little bouquet. Thought it might look nice on the counter.
    You don’t gotta put it up there, jussst wanted to do sssomethin’ nice.
    Thanksss for always cookin’ up those deliciousss meals!
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