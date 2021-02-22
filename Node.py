import json

from AnalyseFrequenceMethode import AnalyseFrequenceMethode


class Node():
    def __init__(self, header, body, argu):
        self.header=header
        self.body=body
        self.argu=argu
        self.childrens=[]
        self.analyseFrequence=None

    @staticmethod
    def analyse(text):
        th=""
        tb=""
        pa=0
        blocks=[]
        antiVar=False
        for c in text:
            if antiVar:
                if c in " ;":
                    continue
                else:
                    antiVar=False
            if c == '{':
                if pa !=0:
                    tb+=c
                pa+=1
            elif c=='}':
                pa-=1
                if pa == 0:
                    th.strip()
                    tb.strip()
                    if th!="" and tb!="":
                        blocks.append((th,tb))
                    th=""
                    tb=""
                    antiVar=True
                else:
                    tb+=c
            elif pa==0:
                th+=c
            else:
                tb+=c
        res=[]
        for (header,body) in blocks:
            info=[]
            argu=[]
            par=False
            t=""
            k=0
            error=False
            for c in header:
                if c == '=' or c == ';':
                    error=True
                    break
                if par:
                    if c == ')':
                        argu.append(t)
                        break
                    elif c=='<':
                        k+=1
                    elif c=='>':
                        k-=1
                    elif c == ',' and k==0:
                        argu.append(t)
                        t=""
                    elif c!=' ':
                        t+=c
                else:
                    if c == '(':
                        if t!= "":
                            info.append(t)
                        t=""
                        par=True
                    elif c == ' ':
                        if t!= "":
                            info.append(t)
                        t=""
                    else:
                        t+=c
            if not par and t!="":
                info.append(t)
            if not error and len(info)>0:
                res.append(Node(info,body,argu))
        return res

    def __hash__(self):
        txt=""
        for c in self.body:
            code = ord(c)
            if code>20 and code<127:
                txt+=c
        return hash(txt)

    def __str__(self):
        return "Header: " + str(self.header) + " | Param: " + str(self.argu) + " | Body: " + str(self.body)

    def strRecursive(self,fn=lambda c: c.header[-2]+" "+c.header[-1], level=0):
        res=("   "*level)+"-|"+fn(self)
        level+=1
        for c in self.childrens:
            res+="\n"+c.strRecursive(fn, level)
        return res

    def getAll(self, path=""):
        path+="/"+self.header[-2]+"."+self.header[-1]
        res=[]
        if len(self.childrens)==0:
            res.append((path,self))
        else:
            for n in self.childrens:
                res+=n.getAll(path)
        return res

    def getJson(self):
        return json.dumps({"hash":self.__hash__(),"operateurs":self.analyseFrequence.operateurs,"keys":self.analyseFrequence.keys})

    @staticmethod
    def getAllNodes(nodes):
        res=[]
        for n in nodes:
            res+=n.getAll()
        return res

    @staticmethod
    def recursiveAnalyse(text):
        res=Node.analyse(text)
        for e in res:
            e.childrens=Node.recursiveAnalyse(e.body)
            if len(e.childrens)==0:
                e.analyseFrequence=AnalyseFrequenceMethode(e)
        return res