
import random
import json

from src.model.Player import Player
from src.model.card.Sanity import Sanity
from src.model.card.saneCard import *
from src.model.card.insaneCard import *
from src.model.ai.AIActionsEnum import AIActionsEnum

class Agent(Player):
    
    def __init__(self,saneToken, insaneToken, hand, discard, knockedOut, knockableOut, immune,
                 epsilon=0.1, alpha=0.2, gamma=0.9):
        super().__init__(saneToken, insaneToken, hand, discard, knockedOut, knockableOut, immune)

        #python dictionnary / Q table
        self.q = self._readJsonQTable()

        self.epsilon = epsilon
        self.alpha = alpha
        self.gamma = gamma
        
        self.lastAction = None
        self.score = 0

        self.winReward = 400
        self.knockedReward = -200
        self.immuneReward = 150
        
    #Get Q value for a (state,action) key
    def getQ(self, state, action):
#        print("getQ : " + str(self.q.get((state, action), 0.0)))
        return self.q.get((state, action), 0.0)


    #Update Q table
    def learnQ(self, state, action, reward, value):
        oldv = self.q.get((state, action), None)
        if oldv is None:
            self.q[(state, action)] = reward
        else:
            #Q function
            self.q[(state, action)] = oldv + self.alpha * (value - oldv)
    
    #Epsilon-Greedy strategy, choose the best action (or random) for a state in a list of available actions        
    def chooseAction(self, state,actions):
#        print("choose action : " + str(len(actions)))
        if (len(actions) <= 0):
            print("actions error")
            return None
        
        if random.random() < self.epsilon:
            action = random.choice(actions)
        else:
            q = [self.getQ(state, a) for a in actions]
            maxQ = max(q)
            count = q.count(maxQ)
            if count > 1:
                best = [i for i in range(len(actions)) if q[i] == maxQ]
                i = random.choice(best)
            else:
                i = q.index(maxQ)

            action = actions[i]
        return action
    
    def learn(self, state1, action1, reward, state2,actions):
        maxqnew = max([self.getQ(state2, a) for a in actions])
        self.learnQ(state1, action1, reward, reward + self.gamma*maxqnew)

    def printQ(self):
        print(self.q)
#        keys = self.q.keys()
#        states = list(set([a for a,b in keys]))
#        actions = list(set([b for a,b in keys]))
#        
#        dstates = ["".join([str(int(t)) for t in list(tup)]) for tup in states]
#        print (" "*4) + " ".join(["%8s" % ("("+s+")") for s in dstates])
#        for a in actions:
#            print ("%3d " % (a)) + \
#                " ".join(["%8.2f" % (self.getQ(s,a)) for s in states])
        
    #Return the State defined by a tuple 
    def calcState(self):
        return self.saneToken, self.insaneToken,self.knockedOut,self.immune
    
    #Return the reward for the actual state
    def calcReward(self):
        if self.saneToken >= 3 or self.insaneToken >= 2:
            return self.winReward
        if self.knockedOut == True:
            return self.knockedReward
        if self.immune == True:
            return self.immuneReward
        return 0
    
    #Q learning calculation function, return choosen card effect
    def update(self,gameManager):
        reward = self.calcReward()
        state = self.calcState()
        listOfActions = self._buildListOfActions(gameManager)
        if len(listOfActions) <= 0:
            raise ValueError("List of actions length null or less than 0.")
        action = self.chooseAction(state,listOfActions)
        if self.lastAction is not None and action is not None:
            self.learn(self.lastState, self.lastAction, reward, state,listOfActions)
        self.lastState = state
        self.lastAction = action
        #TODO complete
        return action
        
        
    #Actions (playable card) change every turn so we need to retrieve the list of available action
    def _buildListOfActions(self,gameManager):
        listOfActions = []
        print("--------------------------------------")
        print("build list of action , hand length : " + str(len(self.hand)))
        if len(self.hand) <= 1:
            raise Exception("At least two cards should be present.")
        for card in self.hand:
            print("card : " + card.name)
            #Check if the card in the hand is playable
            if gameManager.checkPlayableCard(card):
                #Check if the card can be played with INSANE effect
                if card.hasInsane():
                    if self.stateOfMind() == Sanity.INSANE :
                        #TODO Improve this part
                        if isinstance(card,Cthulhu):
                            listOfActions.append(AIActionsEnum.CthulhuInsane.value)
                            listOfActions.append(AIActionsEnum.CthulhuSane.value)
                        if isinstance(card,DeepOnes):
                            listOfActions.append(AIActionsEnum.DeepOnesInsane.value)
                            listOfActions.append(AIActionsEnum.DeepOnesSane.value)
                        if isinstance(card,GoldenMead):
                            listOfActions.append(AIActionsEnum.GoldenMeadInsane.value)
                            listOfActions.append(AIActionsEnum.GoldenMeadSane.value)
                        if isinstance(card,HoundOfTindalos):
                            listOfActions.append(AIActionsEnum.HoundOfTindalosInsane.value)
                            listOfActions.append(AIActionsEnum.HoundOfTindalosSane.value)
                        if isinstance(card,LiberIvonis):
                            listOfActions.append(AIActionsEnum.LiberIvonisInsane.value)
                            listOfActions.append(AIActionsEnum.LiberIvonisSane.value)
                        if isinstance(card,MiGo):
                            listOfActions.append(AIActionsEnum.MiGoInsane.value)
                            listOfActions.append(AIActionsEnum.MiGoSane.value)
                        if isinstance(card,MiGoBraincase):
                            listOfActions.append(AIActionsEnum.MiGoBrainCaseInsane.value)
                            listOfActions.append(AIActionsEnum.MiGoBrainCaseSane.value)
                        if isinstance(card,Nyarlathotep):
                            listOfActions.append(AIActionsEnum.NyarlathotepInsane.value)
                            listOfActions.append(AIActionsEnum.NyarlathotepSane.value)
                        if isinstance(card,TheShiningTrapezohedron):
                            listOfActions.append(AIActionsEnum.TheShiningTrapezohedronInsane.value)
                            listOfActions.append(AIActionsEnum.TheShiningTrapezohedronSane.value)
                
                    if self.stateOfMind() == Sanity.SANE:
                        if isinstance(card,Cthulhu):
                            listOfActions.append(AIActionsEnum.CthulhuSane.value)
                        if isinstance(card,DeepOnes):
                            listOfActions.append(AIActionsEnum.DeepOnesSane.value)
                        if isinstance(card,GoldenMead):
                            listOfActions.append(AIActionsEnum.GoldenMeadSane.value)
                        if isinstance(card,HoundOfTindalos):
                            listOfActions.append(AIActionsEnum.HoundOfTindalosSane.value)
                        if isinstance(card,LiberIvonis):
                            listOfActions.append(AIActionsEnum.LiberIvonisSane.value)
                        if isinstance(card,MiGo):
                            listOfActions.append(AIActionsEnum.MiGoSane.value)
                        if isinstance(card,MiGoBraincase):
                            listOfActions.append(AIActionsEnum.MiGoBrainCaseSane.value)
                        if isinstance(card,Nyarlathotep):
                            listOfActions.append(AIActionsEnum.NyarlathotepSane.value)
                        if isinstance(card,TheShiningTrapezohedron):
                            listOfActions.append(AIActionsEnum.TheShiningTrapezohedronSane.value)
                
                #Sane only
                else:
                    if isinstance(card,CatsOfUlthar):
                        listOfActions.append(AIActionsEnum.CatsOfUltharSane.value)
                    if isinstance(card,ElderSign):
                        listOfActions.append(AIActionsEnum.ElderSignSane.value)
                    if isinstance(card,GreatRaceOfYith):
                        listOfActions.append(AIActionsEnum.GreatRaceOfYithSane.value)
                    if isinstance(card,Investigators):
                        listOfActions.append(AIActionsEnum.InvestigatorSane.value)
                    if isinstance(card,ProfessorHenryArmitage):
                        listOfActions.append(AIActionsEnum.ProfessorHenryArmitageSane.value)
                    if isinstance(card,RandolphCarter):
                        listOfActions.append(AIActionsEnum.RandolphCarterSane.value)
                    if isinstance(card,TheNecronomicon):
                        listOfActions.append(AIActionsEnum.TheNecronomiconSane.value)
                    if isinstance(card,TheSilverKey):
                        listOfActions.append(AIActionsEnum.TheSilverKeySane.value)
            else:
                #The Shining Trapezogedron && The SIlver Key special case
                print("not playable")
                if card.hasInsane():
                    if self.stateOfMind() == Sanity.INSANE :
                        if isinstance(card,TheShiningTrapezohedron):
                            listOfActions.append(AIActionsEnum.TheShiningTrapezohedronInsane.value)
                            listOfActions.append(AIActionsEnum.TheShiningTrapezohedronSane.value)
                    if self.stateOfMind() == Sanity.SANE:
                        if isinstance(card,TheShiningTrapezohedron):
                            listOfActions.append(AIActionsEnum.TheShiningTrapezohedronSane.value)
                else:
                    if isinstance(card,TheSilverKey):
                        listOfActions.append(AIActionsEnum.TheSilverKeySane.value)
                        
        return listOfActions

    @staticmethod
    def _readJsonQTable() :
        data = {}
        try :
            with open("AIQtable.txt") as f :
                dic = ""
                for i in f.readlines() :
                    dic += i
                data = eval(dic)
        except FileNotFoundError :
            pass

        return data

    @staticmethod
    def _writeJsonQTable(data) :
        with open("AIQtable.txt", 'w') as f :
            f.write(str(data))
