# SafeHaven

The actual Unity game / Build of Safe Haven

## Coding guidelines

Go to [Coding Guidelines](https://github.com/AIM-GAME-PROJECT-group-b/SafeHaven/wiki/Coding-Guidelines) in the Wiki

## GIT and github rules/guidelines

### git clone

when pulling for the first time. you will notice that it takes a while to load up the project in the unity editor. This is because the /UserSettings/ folder is not included in the repository. This means that you don't have to commit your own layout but it also means that if you don't have one yet (which will only happen the first time you pull the repo) Unity will generate it for you. So after the first time loading up the project in the unity editor it should take way less longer to load up the project. Unless you have a potato for a computer. I can't help you with that.

### GIT Rules

1. the main branch is only for a working version of the unity project. You should ***NEVER*** push or create a pull request directly to this branch. **If you don't know what this means, no worries, you don't have acces to fuck stuff up anyway.**
2. the develop branch is used as an integration and test point before the main branch. If you have a working feature that you want to add to the game you should make a pull request to this branch. If you want to start working on a new feature then you should use this branch as a starting point for your own branch.
3. Branches should be named like this: ```[Branch_Type]-[Branch_Name]```. For example, if you want to add a flowing river to the project, your branch should be called ```feature-flowing_water```, or if you want to add a model of a brigde, your branch should be called ```art-small_brigde```
    The branch types are (work in progress):
   1. feature  
      Used for new features.
   2. bug  
      Used for bugfixes.
   3. art  
      Used for the import of new art assets.
   4. wip  
      Used for experiments, not something you nessesarily want to merge with the develop branch.
   5. git  
      Used for git shenanigans, only used by the git wizzards (∩｀-´)⊃━☆ﾟ.*･｡ﾟ
   6. dev  
      Used for internal integration per team. These branches are specific for each team so please don't make one
4. Do **NOT** use capital letter in your branch names. it makes it easier to type them out in git bash.

## Unity Folder Structure

In the unity project there are folder already made for everybody. You are allowed to make new folder but please do so with care and see if your files could go into an subfolder.  
The folders that are marked with * are folders that you can put subfolders in without worring about the GIT masters coming after you. If you need to make a subfolder somewhere else anyway, please notify the GIT master of this. You have been warned.  
The folder are setup as followed (WIP):

```
Assets  
│   This is the root folder in the unity project
│
└───Animations
│   │
│   └───Components *
│   │   │   Used for animation files
│   │
│   └───Controllers *
│       │   Used for the animation controllers
│
└───Audio
│   │
│   └───Music *
│   │   │   Used for Music files
│   │
│   └───Sounds *
│       │   Used for sound effect files
│
└───Materials
│   │
│   └───Characters *
│   │   │   Used for materials specific for the player character(s)
│   │
│   └───Enviroment *
│   │   │   Used for materials specific for the enviroment
│   │
│   └───NPC *
│       │   Used for materials specific for non playeble characters such as enemies and/or other types of NPCs
│
│
└───Models
│   │
│   └───Characters *
│   │   │   Used for models specific for the player character(s)
│   │
│   └───Enviroment *
│   │   │   Used for models specific for the enviroment
│   │
│   └───NPC *
│       │   Used for models specific for non playeble characters such as enemies and/or other types of NPCs
│
└───Prefabs
│   │
│   └───Characters *
│   │   │   Used for prefabs specific for the player character(s)
│   │
│   └───Enviroment *
│   │   │   Used for prefabs specific for the enviroment
│   │
│   └───NPC *
│       │   Used for prefabs specific for non playeble characters such as enemies and/or other types of NPCs
│
└───Scenes
│   | Used for all scenes
│
└───Scripts
│   │
│   └───Characters *
│   │   │   Used for scripts specific for the player character(s)
│   │
│   └───Enviroment *
│   │   │   Used for scripts specific for the enviroment
│   │
│   └───NPC *
│       │   Used for scripts specific for non playeble characters such as enemies and/or other types of NPCs
│
└───Settings
│   │   Used for settings regarding the URP and light settings
│   └───Input
│       │   Used for the .inputactions file from the new input system
│
└───Texures *
        │   Used for raw texture files
```
