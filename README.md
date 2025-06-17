Rapport de projet

Ce projet a été réalisé au cours de plusieurs séances de TP. Il a pour objectif de combiner plusieurs fonctionnalités de Unity, telles que les particules, les shaders, le système de NavMesh, etc.

Le joueur contrôle un personnage qui pointe du doigt la zone de fin. Une fois arrivé à la fin, un bouton cliquable (touche F) permet de faire apparaître un éclair suivi de flammes.

La zone de jeu comporte également des pièges qui déclenchent des éclairs. L’ensemble de la scène est généré aléatoirement au lancement, il est donc possible que le joueur ou l’agent IA se retrouve bloqué. N’hésitez pas à relancer la scène si cela se produit.

L’agent IA se déplace automatiquement vers la zone de fin en évitant les flammes et les arbres grâce au système de NavMesh. S’il est frappé par un éclair, il est renvoyé à son point de départ.
