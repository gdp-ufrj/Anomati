// [Ao norte da tela, um espectro negro se aproxima do personagem, de maneira lenta e gradual, e, ao finalmente ficar em sua frente, se revela como uma menina]

Oi moço… Quem é você? Eu nunca te vi aqui antes. #speaker:npc #portrait:npc_feliz #layout:right
-> main

== main ==
+ [Sou Lúgubre.]
    …Eu me chamo Lúgubre, agora me deixe em paz. #speaker:player #portrait:player #layout:left
    Lúgubre? E isso é nome? #speaker:npc #portrait:npc_feliz #layout:right
    …É. #speaker:player #portrait:player #layout:left
    Se você diz. Meu nome é Elisa, aliás! #speaker:npc #portrait:npc_feliz #layout:right
    Que roupa é essa? Você não sente calor, não? Mal dá pra ver você. #speaker:npc #portrait:npc_feliz #layout:right
    … #speaker:player #portrait:player #layout:left
    Você parece meio triste. Lágrimas não combinam com esta cidade, sabia? #speaker:npc #portrait:npc_triste #layout:right
    O que disse, menina? #speaker:player #portrait:player #layout:left
    Que lágrimas não combinam com essa cidade! É um velho ditado de Anomati. #speaker:npc #portrait:npc_feliz #layout:right
    -> DONE

+ [Vaza]
    Cuide da sua vida, menina. #speaker:player #portrait:player #layout:left
    Vou perguntar até você me falar seu nome! #speaker:npc #portrait:npc_bravo #layout:right
    -> main

+ [Repita, por favor]
    Fala alto que eu sou velho. #speaker:player #portrait:player #layout:left
    Eu perguntei quem é você! #speaker:npc #portrait:npc_feliz #layout:right
    -> main
