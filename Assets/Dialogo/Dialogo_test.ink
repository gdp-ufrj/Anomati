Oi! Você parece novo por aqui. #speaker:npc #portrait:npc_feliz #layout:right
-> main

=== main ===
Como você está se sentindo depois da sua chegada?

+ [Estou animado.]
    Que bom ouvir isso! Espero que aproveite cada momento. #portrait:npc_feliz

+ [Estou confuso.]
    Ah... sinto muito. Esse lugar pode ser estranho no começo. #portrait:npc_triste

+ [Isso não é da sua conta.]
    Ei, não precisa ser grosseiro! Só queria ajudar. #portrait:npc_bravo

- Desculpa, estou só tentando entender tudo. #speaker:player #portrait:player #layout:left

Está tudo bem. Quer saber mais sobre este lugar? #speaker:npc #portrait:npc_feliz #layout:right

+ [Sim, por favor.]
    Ótimo! Posso te mostrar onde ficam as coisas. #portrait:npc_feliz
    -> main

+ [Não, prefiro descobrir por conta própria.]
    Entendo. Se mudar de ideia, estarei por aqui. #portrait:npc_feliz
    -> END