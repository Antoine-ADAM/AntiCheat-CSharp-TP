import json
IGNORE=[123,125]#['{','}']
SEPARATOR=" {}();?:"
#https://docs.microsoft.com/fr-fr/dotnet/csharp/language-reference/keywords/
#les plus utile à regarder dans une methode sans tester les variables (const, readonly,int,...) seule la stucture est testé
KEYS=["if","else","while","for","foreach","case","switch","default","is","in","ref","out","do","try","catch","throw","break","continue","goto"]
class AnalyseFrequenceMethode():
    def __init__(self, node):
        self.node=node
        self.operateurs={}
        self.keys={}
        presKeys=[]
        t=""
        for c in node.body:
            code=ord(c)
            #pas de caractère/chiffre seulement des "operateurs"
            if (code<48 or code>57) and (code<65 or code>90) and (code<97 or code>122) and code>32 and code<127 and not code in IGNORE:
                self.operateurs[c]=1+self.operateurs.setdefault(c,0)
            if c in SEPARATOR:
                if len(t)>1:
                    presKeys.append(t)
                t=""
            else:
                t+=c
        if len(t) > 1:
            presKeys.append(t)
        for e in presKeys:
            if e in KEYS:
                self.keys[e] = 1 + self.keys.setdefault(e, 0)
    def appendIfElseSucre(self):
        vif=self.keys.setdefault("if",0)+self.operateurs.setdefault('?',0)
        if vif!=0:
            self.keys["if"]=vif
        velse = self.keys.setdefault("else", 0) + self.operateurs.setdefault(':', 0)
        if velse != 0:
            self.keys["else"] = velse
    def countBoucle(self):
        return self.keys.setdefault("while",0)+self.keys.setdefault("for",0)+self.keys.setdefault("foreach",0)
    def countIfElseSwitch(self):
        return (self.keys.setdefault("if", 0) + self.keys.setdefault("case", 0) , self.keys.setdefault("else",0) + self.keys.setdefault("break",0))
    def countOperateur(self):
        return self.operateurs.setdefault('+',0)+self.operateurs.setdefault('-',0)+self.operateurs.setdefault('/',0)+self.operateurs.setdefault('*',0)+self.operateurs.setdefault('%',0)
    def countLogic(self):
        return self.operateurs.setdefault('=',0)+self.operateurs.setdefault('!',0)+self.operateurs.setdefault('<',0)+self.operateurs.setdefault('>',0)+self.keys.setdefault("is",0)+self.keys.setdefault("in",0)+self.operateurs.setdefault('&',0)+self.operateurs.setdefault('|',0)
    def __str__(self):
        return json.dumps({"operators":self.operateurs,"keys":self.keys})